using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Code
{
    public class Client
    {
        TcpClient client;
        CancellationTokenSource cts;
        Task thread;

        X509Certificate2 certificate;



        AES aes = new AES();
        RSA rsa = new RSA().GenerateNewKeys();
        NoCryption notcryptor = new NoCryption();

        public void EnableAES(bool t)
        {
            aes.Enable(t);
        }
        public void EnableRSA(bool t)
        {
            rsa.Enable(t);
        }

        public bool Active { get; private set; }

        public async Task<bool> StartAsyc(string serverIp, int port)
        {
            cts = new CancellationTokenSource();
            client = new TcpClient();
            try
            {
                thread = client.ConnectAsync(serverIp, port);
                await thread;

                Active = await ReadStringAsyc(notcryptor) == "welcome";
                rsa.SetClientPublicKey(RSA.StringToKey(await ReadStringAsyc(notcryptor)));
                WriteString(notcryptor, RSA.KeyToString(rsa.PublicKey));

                aes.SetKey((await ReadStringAsyc(rsa)).StringToBytes());
            }
            catch
            {
                Active = false;
            }

            return Active;
        }
        public void Stop()
        {
            WriteString(aes, "bye");

            cts.Cancel();
            try
            {
                thread.Wait(5000, cts.Token);
            }
            catch { }

            client.Close();

            Active = false;
        }
        public async Task<bool> LoginAsyc(string user, string pwd)
        {
            WriteString(aes, user);
            WriteString(aes, pwd);
            return await ReadStringAsyc(aes) == "ok";
        }

        public async Task<bool> GetCertificate(string user)
        {
            certificate = (await SendSingleRequest("127.0.0.1", 2223, ("get cert for " + user).StringToBytes())).ToX509();
            return certificate != null;
        }
        
        public X509Certificate2 getCert()
        {
            return certificate;
        }

        public async Task<string> RegisterAsyc(string user, string pwd)
        {
            return await SendCommand("register user[" + user + "] pwd[" + pwd + "]");
        }

        public async Task<string> SendCommand(string p)
        {
            if (p.StartsWith("transfer "))
            {
                WriteString(aes, "transfer");
                var newP = p.Substring("transfer ".Length);
                WriteString(rsa, newP);
                await ReadStringAsyc(aes);
                WriteBytes(aes, rsa.Sign(newP.StringToBytes()));
            }
            else if(p.StartsWith("register user["))
            {
                WriteString(aes, p);
                WriteBytes(aes, certificate.ToBytes());
            }
            else
                WriteString(aes, p);
            return await ReadStringAsyc(aes);
        }

        public void WriteBytes(ICryptable crpt, byte[] a)
        {
            byte[]data = crpt.Encrypt(a);
            client.GetStream().WriteAsync(data, 0, data.Length, cts.Token);

            Thread.Sleep(100);
        }
        public void WriteString(ICryptable crpt, string a)
        {
            WriteBytes(crpt, a.StringToBytes());
        }

        public async Task<byte[]> ReadBytesAsyc()
        {
            List<byte> data = new List<byte>();

            byte[] buffer = new byte[4096];
            int readed = 0;
            do
            {
                readed = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, cts.Token);
                for (int i = 0 ; i < readed ; i++)
                    data.Add(buffer[i]);
            } while (client.GetStream().DataAvailable);

            return data.ToArray();
        }
        public async Task<string> ReadStringAsyc(ICryptable crpt)
        {
            return crpt.Decrypt(await ReadBytesAsyc()).BytesToString().DeletZerosFromEnd();
        }

        public string showKeys()
        {
            return string.Format(
                "My public Key:\r\n{0}\r\n---------------------------------------------\r\n" +
                "Remote Public Key:\r\n{1}\r\n-------------------------------------------\r\n" +
                "Shared AES Key:\r\n{2}\r\n",
                RSA.KeyToString(rsa.PublicKey),
                RSA.KeyToString(rsa.ClientPublicKey),
                aes.GetKeyAsString());
        }
        public string showCertificate()
        {
            if (certificate == null)
                return "there is no certificate";
            return certificate.ToString2();
        }



        public static async Task<byte[]> SendSingleRequest(string address, int port, byte[] msg)
        {
            byte[] response = null;
            using (TcpClient requester = new TcpClient(address, port))
            {
                using (var dataStream = requester.GetStream())
                {
                    byte[] buf;
                    RSA rsa = new RSA().GenerateNewKeys();

                    buf = new byte[397];
                    await dataStream.ReadAsync(buf, 0, buf.Length);
                    rsa.SetClientPublicKey(RSA.StringToKey(buf.BytesToString()));
                    buf = RSA.KeyToString(rsa.PublicKey).StringToBytes();
                    await dataStream.WriteAsync(buf, 0, buf.Length);

                    await dataStream.WriteAsync(rsa.Sign(msg), 0, 128);
                    await dataStream.WriteAsync(msg, 0, msg.Length);

                    buf = new byte[4096];
                    byte[] sign = new byte[128];
                    await dataStream.ReadAsync(sign, 0, 128);
                    int r = await dataStream.ReadAsync(buf, 0, buf.Length);
                    response = buf.Cute(0, r);

                    if (rsa.Varify(response, sign) == false)
                        throw new System.Exception("signture invalid");
                }
            }
            return response;
        }
    }
}