namespace Broadcast_ClientUI__homework_
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbNews = new System.Windows.Forms.CheckBox();
            this.chbReminder = new System.Windows.Forms.CheckBox();
            this.chbEntertainment = new System.Windows.Forms.CheckBox();
            this.btnSubscribe = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();

            // lblServerIp
            this.lblServerIp.AutoSize = true;
            this.lblServerIp.Location = new System.Drawing.Point(12, 15);
            this.lblServerIp.Name = "lblServerIp";
            this.lblServerIp.Size = new System.Drawing.Size(80, 13);
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
            this.btnConnect.Text = "Pidklyuchytysya";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // btnDisconnect
            this.btnDisconnect.Location = new System.Drawing.Point(360, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 23);
            this.btnDisconnect.Text = "Vidklyuchytysya";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);

            // groupBox1 - Pidpyska
            this.groupBox1.Location = new System.Drawing.Point(12, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 100);
            this.groupBox1.Text = "Oberyit typy povidomlen'";

            // chbNews
            this.chbNews.AutoSize = true;
            this.chbNews.Location = new System.Drawing.Point(10, 20);
            this.chbNews.Name = "chbNews";
            this.chbNews.Size = new System.Drawing.Size(80, 17);
            this.chbNews.Text = "Novyny";

            // chbReminder
            this.chbReminder.AutoSize = true;
            this.chbReminder.Location = new System.Drawing.Point(10, 45);
            this.chbReminder.Name = "chbReminder";
            this.chbReminder.Size = new System.Drawing.Size(100, 17);
            this.chbReminder.Text = "Nagaduyvannya";

            // chbEntertainment
            this.chbEntertainment.AutoSize = true;
            this.chbEntertainment.Location = new System.Drawing.Point(10, 70);
            this.chbEntertainment.Name = "chbEntertainment";
            this.chbEntertainment.Size = new System.Drawing.Size(90, 17);
            this.chbEntertainment.Text = "Rozvazhalne";

            // btnSubscribe
            this.btnSubscribe.Location = new System.Drawing.Point(170, 60);
            this.btnSubscribe.Name = "btnSubscribe";
            this.btnSubscribe.Size = new System.Drawing.Size(120, 30);
            this.btnSubscribe.Text = "Pryntyty pidpysku";
            this.btnSubscribe.Click += new System.EventHandler(this.btnSubscribe_Click);

            this.groupBox1.Controls.Add(this.chbNews);
            this.groupBox1.Controls.Add(this.chbReminder);
            this.groupBox1.Controls.Add(this.chbEntertainment);
            this.groupBox1.Controls.Add(this.btnSubscribe);

            // txtMessages
            this.txtMessages.Location = new System.Drawing.Point(12, 155);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessages.Size = new System.Drawing.Size(448, 250);
            this.txtMessages.TabIndex = 1;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 417);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtServerIp);
            this.Controls.Add(this.lblServerIp);
            this.Name = "Form1";
            this.Text = "Broadcast Klient z pidpyskamy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblServerIp;
        private System.Windows.Forms.TextBox txtServerIp;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbNews;
        private System.Windows.Forms.CheckBox chbReminder;
        private System.Windows.Forms.CheckBox chbEntertainment;
        private System.Windows.Forms.Button btnSubscribe;
    }
}
