using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Code
{
    public class TcpListenerX: TcpListener
    {
        public TcpListenerX(int port) : base(IPAddress.Any, port) { }
        public bool IsActive { get { return base.Active; } }
    }

    public class Server
    {
        TcpListenerX server;
        CancellationTokenSource cts;
        Task<TcpClient> ThreadForClient;

        AES _aes = new AES();
        RSA _rsa = new RSA().GenerateNewKeys();
        NoCryption withoutCryption = new NoCryption();

        IDataProvider db;
        public Server(IDataProvider dp)
        {
            db = dp;
        }

        public string[] showAllUserInfo()
        {
            return db.PrintAllUsersInfo();
        }
        public string[] showAllTransactions()
        {
            return db.PrintAllTransactions();
        }
        public long getTotalBalance()
        {
            return db.TotalBalance();
        }

        public static async Task<string[]> GetIPAddress()
        {
            List<string> ips = new List<string>();
            ips.Add("127.0.0.1");
            try
            {
                ips.AddRange((await Dns.GetHostAddressesAsync(Dns.GetHostName())).Select(x => x.ToString()).ToArray());
            }
            catch { }
            return ips.ToArray();
        }

        public int Port { get; set; }
        public bool IsActive { get { return server != null && server.IsActive; } }

        static bool _AESEnable = true;
        public void EnableAES(bool t)
        {
            _AESEnable = t;
        }
        static bool _RSAEnable = true;
        public void EnableRSA(bool t)
        {
            _RSAEnable = t;
        }

        public async void StartAsync(int port)
        {
            cts = new CancellationTokenSource();
            server = new TcpListenerX(port);

            try
            {
                active();
                try
                {
                    await acceptClientsAsync();
                }
                catch { }
            }
            catch { log("----some error----"); }
            finally { Stop(); }
        }
        public void Stop()
        {
            if (!IsActive)
                return;
            try
            {
                cts.Cancel();
                try
                {
                    ThreadForClient.Wait(10, cts.Token);
                }
                catch { }
            }
            finally
            {
                diactive();
            }
        }

        async Task acceptClientsAsync()
        {
            while (!cts.IsCancellationRequested)
            {
                try
                {
                    ThreadForClient = server.AcceptTcpClientAsync();
                    TcpClient client = await ThreadForClient.ConfigureAwait(false);

                    ThreadPool.QueueUserWorkItem(async (obj) => await handelQueryAsync((TcpClient)obj), client);
                }
                finally { }
            }
        }

        public bool ThisCAServer_tag=false;
        public async Task handelQueryAsync(TcpClient client)
        {
            log("new client connected");
            var username = "someone";

            using (client)
            {
                if (!cts.IsCancellationRequested)
                {
                    try
                    {
                        WriteString(client, withoutCryption, "welcome");
                        WriteString(client, withoutCryption, RSA.KeyToString(_rsa.PublicKey));
                        RSA rsa = new RSA(_rsa).SetClientPublicKey(RSA.StringToKey(await ReadStringAsyc(client, withoutCryption)));

                        var aeskey = helper.GenerateRandomString();
                        WriteString(client, rsa, aeskey);

                        AES aes = new AES().SetKey(aeskey.StringToBytes());

                        username = await handelClientAsync(client, rsa, aes);
                    }
                    catch { }
                }
            }
            log(username + " disconnected");
        }

        private async Task<string> handelClientAsync(TcpClient client, RSA rsa, AES aes)
        {
            aes.Enable(_AESEnable);
            string query = await ReadStringAsyc(client, aes);
            string username = query, pwd="";
            if (query.StartsWith("register user["))
            {
                string error="",res="";
                var c = (await ReadBytesAsyc(client, aes)).ToX509();

                if (CertificationFactory.Verify(c, ref error))
                {
                    username = query.SubStringFromTo(query.IndexOf("[") + 1, query.IndexOf("]"));
                    if (db.FindByName(username) != null)
                        res = "this username used, try somthing else";
                    else
                    {
                        pwd = query.SubStringFromTo(query.IndexOf("[", query.IndexOf("[") + 1) + 1, query.IndexOf("]", query.IndexOf("]") + 1));
                        db.AddAccount(username, pwd);
                        res = "ok";
                    }
                }
                else
                    res = "your certificate not valid";

                WriteString(client, aes, res);
                return username;
            }
            else
                pwd = await ReadStringAsyc(client, aes);
            var me = db.FindByName(username);

            if (me == null || me.Password != pwd)
                WriteString(client, aes, "no");
            else
            {
                log(me.Username + " logged in");
                WriteString(client, aes, "ok");
                for (string command = "", res = "" ; true ; )
                {
                    try
                    {
                        aes.Enable(_AESEnable);

                        log("wait '" + me.Username + "' to send...");
                        command = await ReadStringAsyc(client, aes);
                        log(me.Username + " sent : " + command);

                        if (command == "bye")
                            break;
                        else if (command == "get total")
                            res = db.TotalBalance().ToString();
                        else if (command == "get me")
                            res = me.Balance.ToString();
                        else if (command == "get your public key")
                            res = RSA.KeyToString(rsa.PublicKey);
                        else if (command.StartsWith("get ")) // get user1
                        {
                            var v = db.FindByName(command.Substring("get ".Length));
                            res = (v == null) ? "null" : v.Balance.ToString();
                        }
                        else if (command == "transfer") //transfer\r\n100 to user3
                        {
                            rsa.Enable(_RSAEnable);
                            var amountAndUser = await ReadStringAsyc(client, rsa);
                            WriteString(client, aes, "ok");
                            log(me.Username + " sent : " + amountAndUser);
                            log("Varify signature for transaction");
                            var signature = await ReadBytesAsyc(client, aes);

                            if (rsa.Varify(amountAndUser.StringToBytes(), signature) == false)
                            {
                                log("signature wrong");
                                res = "error: signature wrong";
                            }
                            else
                            {
                                log("signature ok");

                                int amount = int.Parse(amountAndUser.SubStringFromTo(0, amountAndUser.IndexOf(" to ")));
                                string toUser = amountAndUser.Substring(amountAndUser.IndexOf(" to ") + " to ".Length);
                                if (db.TransferTo(me, toUser, amount, ref res))
                                    res = me.Balance.ToString();
                            }
                        }
                        else
                            res = "unkown command \"" + command + "\", try 'help'";
                    }
                    catch
                    {
                        log("error while reading - " + command);
                        res = "error, can't understand command!";
                    }

                    WriteString(client, aes, res);
                }
            }
            return username;
        }


        public async Task<byte[]> ReadBytesAsyc(TcpClient client, ICryptable crpto)
        {
            List<byte> data = new List<byte>();

            byte[] buffer = new byte[4096];
            int readed = 0;
            do
            {
                //var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
                //var amountReadTask = ns.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                //var completedTask = await Task.WhenAny(timeoutTask, amountReadTask).ConfigureAwait(false);
                //if (completedTask == timeoutTask){MessageBox.Show("request timed out");break;}

                readed = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, cts.Token);
                if (readed > 0)
                    for (int i = 0 ; i < readed ; i++)
                        data.Add(buffer[i]);
            } while (client.GetStream().DataAvailable);

            return crpto.Decrypt(data.ToArray());
        }
        public async Task<string> ReadStringAsyc(TcpClient client, ICryptable crpto)
        {
            return (await ReadBytesAsyc(client, crpto)).BytesToString().DeletZerosFromEnd();
        }

        public void WriteString(TcpClient client, ICryptable crpto, string str)
        {
            log("sending " + str);
            WriteBytes(client, crpto, str.StringToBytes());
        }
        public void WriteBytes(TcpClient client, ICryptable crpto, byte[] a)
        {
            byte[] data = crpto.Encrypt(a);
            client.GetStream().WriteAsync(data, 0, data.Length);
        }

        #region Logger

        public delegate void Logger(string str);
        public event Logger StateMonitor;
        void log(string x)
        {
            if (StateMonitor != null)
                StateMonitor(x);
        }
        void active()
        {
            if (!IsActive)
                server.Start();
            log("Server Up");
        }
        void diactive()
        {
            if (IsActive)
                server.Stop();
            log("Server Down");
        }

        #endregion

    }
}