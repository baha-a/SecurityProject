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

namespace BankServer
{
    public partial class ServerForm: Form
    {
        Server server;

        public ServerForm()
        {
            InitializeComponent();
            server = new Server(DataSet.CreateDataSet());
            server.StateMonitor += TxtState;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            txtServerIp.Text = (await Server.GetIPAddress()).First();
            if (helper._DEBUG_)
            {
                btnStart_Click(sender, e);
                btnCA_Click(sender, e);
                btnCreateClient_Click(sender, e);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (server.IsActive)
            {
                btnStart.Text = "start";
                server.Stop();
            }
            else
            {
                btnStart.Text = "stop";
                server.StartAsync(int.Parse(txtPort.Text));
            }
        }

        public void TxtState(string txt)
        {
            listServerState.Items.Add(txt);
            listServerState.SelectedIndex = listServerState.Items.Count - 1;
        }

        private void chkAES_CheckedChanged(object sender, EventArgs e)
        {
            server.EnableAES(chkAES.Checked);
        }
        
        private void chkRSA_CheckedChanged(object sender, EventArgs e)
        {
            server.EnableRSA(chkRSA.Checked);
        }

        private void btnShowAllUser_Click(object sender, EventArgs e)
        {
            MessageBox.Show(server.showAllUserInfo().ToSinglLine() + "\r\n\r\n Total Money: " + server.getTotalBalance());
        }

        private void btnShowTransactions_Click(object sender, EventArgs e)
        {
            MessageBox.Show(server.showAllTransactions().ToSinglLine());
        }

        [STAThread]
        private void btnCreateClient_Click(object sender, EventArgs e)
        {
            new BankClient.ClientForm().Show();
        }

        private void btnCA_Click(object sender, EventArgs e)
        {
            new CertificateAuthority.CAForm().Show();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show((await Server.GetIPAddress()).ToSinglLine());
        }
    }
}

