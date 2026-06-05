namespace UPD__homework_
{
    partial class ChatClient
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
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.lblCurrentChat = new System.Windows.Forms.Label();
            this.cbChatRooms = new System.Windows.Forms.ComboBox();
            this.btnJoinChat = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(12, 15);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(51, 13);
            this.lblUsername.Text = "Vashe imya:";

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(69, 12);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(120, 20);
            this.txtUsername.TabIndex = 0;

            // btnConnect
            this.btnConnect.Location = new System.Drawing.Point(195, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(90, 23);
            this.btnConnect.Text = "Pidklyuchytysya";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // btnDisconnect
            this.btnDisconnect.Location = new System.Drawing.Point(291, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(90, 23);
            this.btnDisconnect.Text = "Vidklyuchytysya";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);

            // lblCurrentChat
            this.lblCurrentChat.AutoSize = true;
            this.lblCurrentChat.Location = new System.Drawing.Point(12, 45);
            this.lblCurrentChat.Name = "lblCurrentChat";
            this.lblCurrentChat.Size = new System.Drawing.Size(140, 13);
            this.lblCurrentChat.Text = "Potochnyy chat: General";

            // cbChatRooms
            this.cbChatRooms.Location = new System.Drawing.Point(12, 65);
            this.cbChatRooms.Name = "cbChatRooms";
            this.cbChatRooms.Size = new System.Drawing.Size(150, 21);
            this.cbChatRooms.TabIndex = 1;

            // btnJoinChat
            this.btnJoinChat.Location = new System.Drawing.Point(168, 65);
            this.btnJoinChat.Name = "btnJoinChat";
            this.btnJoinChat.Size = new System.Drawing.Size(90, 23);
            this.btnJoinChat.Text = "Perejty";
            this.btnJoinChat.Click += new System.EventHandler(this.btnJoinChat_Click);

            // txtChat
            this.txtChat.Location = new System.Drawing.Point(12, 95);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(369, 220);
            this.txtChat.TabIndex = 2;

            // lblMessage
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 325);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(115, 13);
            this.lblMessage.Text = "Vashe povidomlennya:";

            // txtMessage
            this.txtMessage.Location = new System.Drawing.Point(12, 345);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(280, 60);
            this.txtMessage.TabIndex = 3;

            // btnSend
            this.btnSend.Location = new System.Drawing.Point(298, 345);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(83, 60);
            this.btnSend.Text = "Vyslaty";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 417);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.btnJoinChat);
            this.Controls.Add(this.cbChatRooms);
            this.Controls.Add(this.lblCurrentChat);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Name = "Form1";
            this.Text = "Chat z chaty";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Label lblCurrentChat;
        private System.Windows.Forms.ComboBox cbChatRooms;
        private System.Windows.Forms.Button btnJoinChat;
    }
}