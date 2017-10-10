using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Code;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace CertificateAuthority
{
    public partial class CAForm: Form
    {
        public CAForm()
        {
            InitializeComponent();
        }

        private async void CAForm_Load(object sender, EventArgs e)
        {
            txtIP.Text = (await Server.GetIPAddress()).First();
            if (helper._DEBUG_)
                 button1_Click(sender, e);
        }

        TcpListenerX server;
        static bool started = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (started == false)
            {
                button1.Text = "stop";
                ThreadPool.QueueUserWorkItem((obj) => Start());
                started = true;
            }
            else
            {
                button1.Text = "start";
                if (server != null)
                server.Stop();
                started = false;
            }
        }

        RSA _rsa = new RSA().GenerateNewKeys();
        void Start()
        {
            try
            {
                RSA rsa = new RSA(_rsa);   
                server = new TcpListenerX(int.Parse(txtPort.Text));
                server.Start();

                byte[] bytes = new byte[1024];
                string data = null;
                TcpClient client =null;
                while (true)
                {
                    if (started == false)
                        return;
                    log("waiting for a connection... ");

                    client = server.AcceptTcpClient();
                    log("connected!");

                    data = null;

                    try
                    {
                        using (NetworkStream stream = client.GetStream())
                        {
                            stream.Write(RSA.KeyToString(rsa.PublicKey).StringToBytes(), 0, 397);
                            if (stream.Read(bytes, 0, 397) != 0)
                            {
                                rsa.SetClientPublicKey(RSA.StringToKey(bytes.BytesToString()));
                                bytes = new Byte[4096];
                                stream.Read(bytes, 0, 128);
                                byte[] sign = bytes.Cute(0, 128);
                                int r = stream.Read(bytes, 0, bytes.Length);
                                data = bytes.Cute(0, r).BytesToString();
                                if(rsa.Varify(bytes.Cute(0, r), sign) == false)
                                    log("signture invalied");
                                else
                                    log("signture valied");

                                log("Received: " + data);

                                if (data.StartsWith("get cert for "))
                                {
                                    string username = data.Substring("get cert for ".Length);
                                    X509Certificate2 cert = CertificationFactory.GenerateCertificate();
                                    log("create certificate for user: " + username);
                                    byte[] msg = cert.ToBytes();
                                    stream.Write(rsa.Sign(msg), 0, 128);
                                    stream.Write(msg, 0, msg.Length);
                                    log("close connection with " + username);
                                }
                                else if(data.StartsWith("verify Cert for "))
                                {

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log("error: " + ex.Message);
                    }
                    finally
                    {
                        client.Close();
                    }
                }
            }
            catch 
            {
                log("server stoped"); 
            }
        }

        void log(string str)
        {
            try
            {
                listBox1.Items.Add(str);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            catch { }
        }

        private void CAForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
                server.Stop();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show((await Server.GetIPAddress()).ToSinglLine());
        }
    }
}
