namespace BankServer
{
    partial class ServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listServerState = new System.Windows.Forms.ListBox();
            this.chkAES = new System.Windows.Forms.CheckBox();
            this.chkRSA = new System.Windows.Forms.CheckBox();
            this.btnShowAllUser = new System.Windows.Forms.Button();
            this.btnShowTransactions = new System.Windows.Forms.Button();
            this.btnCreateClient = new System.Windows.Forms.Button();
            this.btnCA = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtServerIp
            // 
            this.txtServerIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerIp.Location = new System.Drawing.Point(12, 31);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.ReadOnly = true;
            this.txtServerIp.Size = new System.Drawing.Size(159, 20);
            this.txtServerIp.TabIndex = 0;
            this.txtServerIp.Text = "127.0.0.1";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(235, 29);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(177, 31);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(49, 20);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "2222";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server IP:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Server State:";
            // 
            // listServerState
            // 
            this.listServerState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listServerState.FormattingEnabled = true;
            this.listServerState.Location = new System.Drawing.Point(12, 164);
            this.listServerState.Name = "listServerState";
            this.listServerState.Size = new System.Drawing.Size(292, 316);
            this.listServerState.TabIndex = 7;
            // 
            // chkAES
            // 
            this.chkAES.AutoSize = true;
            this.chkAES.Checked = true;
            this.chkAES.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAES.Location = new System.Drawing.Point(15, 60);
            this.chkAES.Name = "chkAES";
            this.chkAES.Size = new System.Drawing.Size(80, 17);
            this.chkAES.TabIndex = 8;
            this.chkAES.Text = "Enable AES";
            this.chkAES.UseVisualStyleBackColor = true;
            this.chkAES.CheckedChanged += new System.EventHandler(this.chkAES_CheckedChanged);
            // 
            // chkRSA
            // 
            this.chkRSA.AutoSize = true;
            this.chkRSA.Checked = true;
            this.chkRSA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRSA.Location = new System.Drawing.Point(15, 83);
            this.chkRSA.Name = "chkRSA";
            this.chkRSA.Size = new System.Drawing.Size(81, 17);
            this.chkRSA.TabIndex = 9;
            this.chkRSA.Text = "Enable RSA";
            this.chkRSA.UseVisualStyleBackColor = true;
            this.chkRSA.CheckedChanged += new System.EventHandler(this.chkRSA_CheckedChanged);
            // 
            // btnShowAllUser
            // 
            this.btnShowAllUser.Location = new System.Drawing.Point(177, 114);
            this.btnShowAllUser.Name = "btnShowAllUser";
            this.btnShowAllUser.Size = new System.Drawing.Size(127, 23);
            this.btnShowAllUser.TabIndex = 10;
            this.btnShowAllUser.Text = "show all Users";
            this.btnShowAllUser.UseVisualStyleBackColor = true;
            this.btnShowAllUser.Click += new System.EventHandler(this.btnShowAllUser_Click);
            // 
            // btnShowTransactions
            // 
            this.btnShowTransactions.Location = new System.Drawing.Point(177, 137);
            this.btnShowTransactions.Name = "btnShowTransactions";
            this.btnShowTransactions.Size = new System.Drawing.Size(127, 23);
            this.btnShowTransactions.TabIndex = 11;
            this.btnShowTransactions.Text = "show Transactions";
            this.btnShowTransactions.UseVisualStyleBackColor = true;
            this.btnShowTransactions.Click += new System.EventHandler(this.btnShowTransactions_Click);
            // 
            // btnCreateClient
            // 
            this.btnCreateClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateClient.Location = new System.Drawing.Point(177, 64);
            this.btnCreateClient.Name = "btnCreateClient";
            this.btnCreateClient.Size = new System.Drawing.Size(127, 23);
            this.btnCreateClient.TabIndex = 12;
            this.btnCreateClient.Text = "Create Client";
            this.btnCreateClient.UseVisualStyleBackColor = true;
            this.btnCreateClient.Click += new System.EventHandler(this.btnCreateClient_Click);
            // 
            // btnCA
            // 
            this.btnCA.Location = new System.Drawing.Point(177, 87);
            this.btnCA.Name = "btnCA";
            this.btnCA.Size = new System.Drawing.Size(127, 23);
            this.btnCA.TabIndex = 13;
            this.btnCA.Text = "Certificate Authority";
            this.btnCA.UseVisualStyleBackColor = true;
            this.btnCA.Click += new System.EventHandler(this.btnCA_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 7F);
            this.button1.Location = new System.Drawing.Point(133, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 20);
            this.button1.TabIndex = 14;
            this.button1.Text = "more";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 488);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCA);
            this.Controls.Add(this.btnCreateClient);
            this.Controls.Add(this.btnShowTransactions);
            this.Controls.Add(this.btnShowAllUser);
            this.Controls.Add(this.chkRSA);
            this.Controls.Add(this.chkAES);
            this.Controls.Add(this.listServerState);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtServerIp);
            this.Location = new System.Drawing.Point(10, 50);
            this.Name = "ServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServerIp;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listServerState;
        private System.Windows.Forms.CheckBox chkAES;
        private System.Windows.Forms.CheckBox chkRSA;
        private System.Windows.Forms.Button btnShowAllUser;
        private System.Windows.Forms.Button btnShowTransactions;
        private System.Windows.Forms.Button btnCreateClient;
        private System.Windows.Forms.Button btnCA;
        private System.Windows.Forms.Button button1;
    }
}

