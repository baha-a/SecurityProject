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

namespace BankClient
{
    public partial class ClientForm: Form
    {
        Client client = new Client();
        public ClientForm()
        {
            InitializeComponent();
        }

        public void log(string x)
        {
            listRecieve.Items.Add(x);
            listRecieve.SelectedIndex = listRecieve.Items.Count - 1;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!client.Active)
                    log("error, not connected");
                else
                    log(await client.SendCommand(txtSend.Text));
            }
            catch (Exception ex) { log(ex.Message); }
        }
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (client.Active)
                {
                    log("Disconnecting...");
                    client.Stop();
                    log("Disconnected");
                    btnLogin.Text = "login";
                }
                else
                {
                    log("Connecting...");

                    if (await client.StartAsyc(txtIPaddress.Text, int.Parse(txtPort.Text)))
                    {
                        if (await client.LoginAsyc(txtUsername.Text, txtPassword.Text))
                        {
                            log("Connected and login successed");
                            btnLogin.Text = "log out";
                        }
                        else
                        {
                            log("Connected but login faild");
                            client.Stop();
                        }
                    }
                    else
                        log("failed");
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }
        private void chkAES_CheckedChanged(object sender, EventArgs e)
        {
            client.EnableAES(chkAES.Checked);
        }
        private void chkRSA_CheckedChanged(object sender, EventArgs e)
        {
            client.EnableRSA(chkRSA.Checked);
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                log(await client.SendCommand("get " + (txtGetBalance.Text)));
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                log(await client.SendCommand("transfer " + int.Parse(txtAmount.Text) + " to " + txtToUser.Text));
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client.Active)
                client.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(client.showKeys());
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (await client.StartAsyc(txtIPaddress.Text, int.Parse(txtPort.Text)))
                {
                    string res = await client.RegisterAsyc(txtUsername.Text, txtPassword.Text);
                    if (res == "ok")
                    {
                        log("ok, you are reqisted, go and login");
                    }
                    else
                    {
                        log("faild :" + res);
                    }
                    client.Stop();
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (await client.GetCertificate(txtUsername.Text))
                    log("you have certificate now");
                else
                    log("faild to get certificate");
            }
            catch (Exception ex)
            {
                log(ex.Message); 
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(client.showCertificate());
        }
    }
}