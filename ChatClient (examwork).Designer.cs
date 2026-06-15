namespace Network_Programing_Exam_work
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpAuth = new System.Windows.Forms.GroupBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnReg = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPriv = new System.Windows.Forms.TabPage();
            this.lblPrivUser = new System.Windows.Forms.Label();
            this.txtPrivUser = new System.Windows.Forms.TextBox();
            this.lblPrivMsg = new System.Windows.Forms.Label();
            this.txtPrivMsg = new System.Windows.Forms.TextBox();
            this.btnSendPriv = new System.Windows.Forms.Button();
            this.txtPrivChat = new System.Windows.Forms.TextBox();
            this.tabGr = new System.Windows.Forms.TabPage();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.btnMkGroup = new System.Windows.Forms.Button();
            this.lblGroups = new System.Windows.Forms.Label();
            this.cbGroups = new System.Windows.Forms.ComboBox();
            this.btnJoinGr = new System.Windows.Forms.Button();
            this.btnRefGr = new System.Windows.Forms.Button();
            this.lblGroupMsg = new System.Windows.Forms.Label();
            this.txtGroupMsg = new System.Windows.Forms.TextBox();
            this.btnSendGr = new System.Windows.Forms.Button();
            this.txtGrChat = new System.Windows.Forms.TextBox();
            this.tabUsrs = new System.Windows.Forms.TabPage();
            this.btnRefUsr = new System.Windows.Forms.Button();
            this.txtUsers = new System.Windows.Forms.TextBox();
            this.grpAuth.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPriv.SuspendLayout();
            this.tabGr.SuspendLayout();
            this.tabUsrs.SuspendLayout();
            this.SuspendLayout();

            // grpAuth
            this.grpAuth.Controls.Add(this.btnLog);
            this.grpAuth.Controls.Add(this.btnReg);
            this.grpAuth.Controls.Add(this.txtPass);
            this.grpAuth.Controls.Add(this.lblPass);
            this.grpAuth.Controls.Add(this.txtUser);
            this.grpAuth.Controls.Add(this.lblUser);
            this.grpAuth.Location = new System.Drawing.Point(12, 12);
            this.grpAuth.Name = "grpAuth";
            this.grpAuth.Size = new System.Drawing.Size(400, 100);
            this.grpAuth.Text = "Auth";

            // lblUser
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(10, 20);
            this.lblUser.Text = "User:";

            // txtUser
            this.txtUser.Location = new System.Drawing.Point(100, 20);
            this.txtUser.Size = new System.Drawing.Size(150, 20);
            this.txtUser.TabIndex = 0;

            // lblPass
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(10, 50);
            this.lblPass.Text = "Pass:";

            // txtPass
            this.txtPass.Location = new System.Drawing.Point(100, 50);
            this.txtPass.Size = new System.Drawing.Size(150, 20);
            this.txtPass.PasswordChar = '*';
            this.txtPass.TabIndex = 1;

            // btnReg
            this.btnReg.Location = new System.Drawing.Point(260, 20);
            this.btnReg.Size = new System.Drawing.Size(80, 23);
            this.btnReg.Text = "Reg";
            this.btnReg.Click += new System.EventHandler(this.btnReg_Click);

            // btnLog
            this.btnLog.Location = new System.Drawing.Point(260, 50);
            this.btnLog.Size = new System.Drawing.Size(80, 23);
            this.btnLog.Text = "Log";
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);

            // tabControl1
            this.tabControl1.Controls.Add(this.tabPriv);
            this.tabControl1.Controls.Add(this.tabGr);
            this.tabControl1.Controls.Add(this.tabUsrs);
            this.tabControl1.Location = new System.Drawing.Point(12, 120);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(500, 300);
            this.tabControl1.TabIndex = 0;

            // tabPriv
            this.tabPriv.Controls.Add(this.txtPrivChat);
            this.tabPriv.Controls.Add(this.btnSendPriv);
            this.tabPriv.Controls.Add(this.txtPrivMsg);
            this.tabPriv.Controls.Add(this.lblPrivMsg);
            this.tabPriv.Controls.Add(this.txtPrivUser);
            this.tabPriv.Controls.Add(this.lblPrivUser);
            this.tabPriv.Name = "tabPriv";
            this.tabPriv.Text = "Priv";

            // lblPrivUser
            this.lblPrivUser.AutoSize = true;
            this.lblPrivUser.Location = new System.Drawing.Point(10, 10);
            this.lblPrivUser.Text = "To:";

            // txtPrivUser
            this.txtPrivUser.Location = new System.Drawing.Point(100, 10);
            this.txtPrivUser.Size = new System.Drawing.Size(150, 20);
            this.txtPrivUser.TabIndex = 0;

            // lblPrivMsg
            this.lblPrivMsg.AutoSize = true;
            this.lblPrivMsg.Location = new System.Drawing.Point(10, 40);
            this.lblPrivMsg.Text = "Msg:";

            // txtPrivMsg
            this.txtPrivMsg.Location = new System.Drawing.Point(100, 40);
            this.txtPrivMsg.Size = new System.Drawing.Size(250, 20);
            this.txtPrivMsg.TabIndex = 1;

            // btnSendPriv
            this.btnSendPriv.Location = new System.Drawing.Point(360, 40);
            this.btnSendPriv.Size = new System.Drawing.Size(80, 23);
            this.btnSendPriv.Text = "Send";
            this.btnSendPriv.Click += new System.EventHandler(this.btnSendPriv_Click);

            // txtPrivChat
            this.txtPrivChat.Location = new System.Drawing.Point(10, 70);
            this.txtPrivChat.Multiline = true;
            this.txtPrivChat.ReadOnly = true;
            this.txtPrivChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrivChat.Size = new System.Drawing.Size(470, 190);
            this.txtPrivChat.TabIndex = 2;

            // tabGr
            this.tabGr.Controls.Add(this.txtGrChat);
            this.tabGr.Controls.Add(this.btnSendGr);
            this.tabGr.Controls.Add(this.txtGroupMsg);
            this.tabGr.Controls.Add(this.lblGroupMsg);
            this.tabGr.Controls.Add(this.btnRefGr);
            this.tabGr.Controls.Add(this.btnJoinGr);
            this.tabGr.Controls.Add(this.cbGroups);
            this.tabGr.Controls.Add(this.lblGroups);
            this.tabGr.Controls.Add(this.btnMkGroup);
            this.tabGr.Controls.Add(this.txtGroupName);
            this.tabGr.Controls.Add(this.lblGroupName);
            this.tabGr.Name = "tabGr";
            this.tabGr.Text = "Groups";

            // lblGroupName
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.Location = new System.Drawing.Point(10, 10);
            this.lblGroupName.Text = "New Gr:";

            // txtGroupName
            this.txtGroupName.Location = new System.Drawing.Point(100, 10);
            this.txtGroupName.Size = new System.Drawing.Size(150, 20);
            this.txtGroupName.TabIndex = 0;

            // btnMkGroup
            this.btnMkGroup.Location = new System.Drawing.Point(260, 10);
            this.btnMkGroup.Size = new System.Drawing.Size(80, 23);
            this.btnMkGroup.Text = "Mk";
            this.btnMkGroup.Click += new System.EventHandler(this.btnMkGroup_Click);

            // lblGroups
            this.lblGroups.AutoSize = true;
            this.lblGroups.Location = new System.Drawing.Point(10, 40);
            this.lblGroups.Text = "Grs:";

            // cbGroups
            this.cbGroups.Location = new System.Drawing.Point(100, 40);
            this.cbGroups.Size = new System.Drawing.Size(150, 21);
            this.cbGroups.TabIndex = 1;

            // btnJoinGr
            this.btnJoinGr.Location = new System.Drawing.Point(260, 40);
            this.btnJoinGr.Size = new System.Drawing.Size(80, 23);
            this.btnJoinGr.Text = "Join";
            this.btnJoinGr.Click += new System.EventHandler(this.btnJoinGr_Click);

            // btnRefGr
            this.btnRefGr.Location = new System.Drawing.Point(350, 40);
            this.btnRefGr.Size = new System.Drawing.Size(80, 23);
            this.btnRefGr.Text = "Ref";
            this.btnRefGr.Click += new System.EventHandler(this.btnRefGr_Click);

            // lblGroupMsg
            this.lblGroupMsg.AutoSize = true;
            this.lblGroupMsg.Location = new System.Drawing.Point(10, 70);
            this.lblGroupMsg.Text = "Msg:";

            // txtGroupMsg
            this.txtGroupMsg.Location = new System.Drawing.Point(100, 70);
            this.txtGroupMsg.Size = new System.Drawing.Size(250, 20);
            this.txtGroupMsg.TabIndex = 2;

            // btnSendGr
            this.btnSendGr.Location = new System.Drawing.Point(360, 70);
            this.btnSendGr.Size = new System.Drawing.Size(80, 23);
            this.btnSendGr.Text = "Send";
            this.btnSendGr.Click += new System.EventHandler(this.btnSendGr_Click);

            // txtGrChat
            this.txtGrChat.Location = new System.Drawing.Point(10, 100);
            this.txtGrChat.Multiline = true;
            this.txtGrChat.ReadOnly = true;
            this.txtGrChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGrChat.Size = new System.Drawing.Size(470, 160);
            this.txtGrChat.TabIndex = 3;

            // tabUsrs
            this.tabUsrs.Controls.Add(this.txtUsers);
            this.tabUsrs.Controls.Add(this.btnRefUsr);
            this.tabUsrs.Name = "tabUsrs";
            this.tabUsrs.Text = "Users";

            // btnRefUsr
            this.btnRefUsr.Location = new System.Drawing.Point(10, 10);
            this.btnRefUsr.Size = new System.Drawing.Size(80, 23);
            this.btnRefUsr.Text = "Ref";
            this.btnRefUsr.Click += new System.EventHandler(this.btnRefUsr_Click);

            // txtUsers
            this.txtUsers.Location = new System.Drawing.Point(10, 40);
            this.txtUsers.Multiline = true;
            this.txtUsers.ReadOnly = true;
            this.txtUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUsers.Size = new System.Drawing.Size(470, 220);
            this.txtUsers.TabIndex = 0;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 431);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.grpAuth);
            this.Name = "Form1";
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.grpAuth.ResumeLayout(false);
            this.grpAuth.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPriv.ResumeLayout(false);
            this.tabPriv.PerformLayout();
            this.tabGr.ResumeLayout(false);
            this.tabGr.PerformLayout();
            this.tabUsrs.ResumeLayout(false);
            this.tabUsrs.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox grpAuth;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btnReg;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPriv;
        private System.Windows.Forms.Label lblPrivUser;
        private System.Windows.Forms.TextBox txtPrivUser;
        private System.Windows.Forms.Label lblPrivMsg;
        private System.Windows.Forms.TextBox txtPrivMsg;
        private System.Windows.Forms.Button btnSendPriv;
        private System.Windows.Forms.TextBox txtPrivChat;
        private System.Windows.Forms.TabPage tabGr;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Button btnMkGroup;
        private System.Windows.Forms.Label lblGroups;
        private System.Windows.Forms.ComboBox cbGroups;
        private System.Windows.Forms.Button btnJoinGr;
        private System.Windows.Forms.Button btnRefGr;
        private System.Windows.Forms.Label lblGroupMsg;
        private System.Windows.Forms.TextBox txtGroupMsg;
        private System.Windows.Forms.Button btnSendGr;
        private System.Windows.Forms.TextBox txtGrChat;
        private System.Windows.Forms.TabPage tabUsrs;
        private System.Windows.Forms.Button btnRefUsr;
        private System.Windows.Forms.TextBox txtUsers;
    }
}