namespace Broadcast_ClientUII
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
            this.lblServerIp = new System.Windows.Forms.Label();
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            // lblServerIp
            this.lblServerIp.AutoSize = true;
            this.lblServerIp.Location = new System.Drawing.Point(12, 15);
            this.lblServerIp.Name = "lblServerIp";
            this.lblServerIp.Size = new System.Drawing.Size(80, 13);
            // IP servera
            this.lblServerIp.Text = "IP servera:";

            // txtServerIp
            this.txtServerIp.Location = new System.Drawing.Point(98, 12);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(150, 20);
            this.txtServerIp.Text = "127.0.0.1";
            this.txtServerIp.TabIndex = 0;

            // btnConnect
            this.btnConnect.Location = new System.Drawing.Point(254, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 23);
            // Pidklyuchytysya
            this.btnConnect.Text = "Pidklyuchytysya";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // btnDisconnect
            this.btnDisconnect.Location = new System.Drawing.Point(360, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 23);
            // Vidklyuchytysya
            this.btnDisconnect.Text = "Vidklyuchytysya";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);

            // txtMessages
            this.txtMessages.Location = new System.Drawing.Point(12, 45);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessages.Size = new System.Drawing.Size(448, 300);
            this.txtMessages.TabIndex = 1;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 357);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtServerIp);
            this.Controls.Add(this.lblServerIp);
            this.Name = "Form1";
            // Broadcast Klient
            this.Text = "Broadcast Klient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblServerIp;
        private System.Windows.Forms.TextBox txtServerIp;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TextBox txtMessages;
    }
}