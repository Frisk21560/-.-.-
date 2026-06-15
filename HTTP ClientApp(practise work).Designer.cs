namespace HTTP__practise_work_
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
            this.lblUri = new System.Windows.Forms.Label();
            this.txtUri = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblUri
            this.lblUri.AutoSize = true;
            this.lblUri.Location = new System.Drawing.Point(12, 15);
            this.lblUri.Name = "lblUri";
            this.lblUri.Size = new System.Drawing.Size(80, 13);
            this.lblUri.Text = "Vvedit URI:";

            // txtUri
            this.txtUri.Location = new System.Drawing.Point(12, 35);
            this.txtUri.Name = "txtUri";
            this.txtUri.Size = new System.Drawing.Size(400, 20);
            this.txtUri.TabIndex = 0;
            this.txtUri.Text = "http://localhost:8080/";

            // btnSend
            this.btnSend.Location = new System.Drawing.Point(418, 35);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Vyslaty zapyt";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // btnClear
            this.btnClear.Location = new System.Drawing.Point(524, 35);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Ochystyty";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 65);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(90, 13);
            this.lblStatus.Text = "Status: Hotovyy";

            // lblResult
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 90);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(80, 13);
            this.lblResult.Text = "Rezultat:";

            // txtResult
            this.txtResult.Location = new System.Drawing.Point(12, 110);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(592, 300);
            this.txtResult.TabIndex = 3;
            this.txtResult.Font = new System.Drawing.Font("Courier New", 9);

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 425);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtUri);
            this.Controls.Add(this.lblUri);
            this.Name = "Form1";
            this.Text = "HTTP Zaptyty";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblUri;
        private System.Windows.Forms.TextBox txtUri;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblStatus;
    }
}