namespace Legacy_Code_Practies_work_7
{
    partial class Form1
    {
        // Kontejner dlya komponentiv
        private System.ComponentModel.IContainer components = null;

        // Dijstruktor - zalyshaye resursy
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Inicijalizacija komponentiv formy
        private void InitializeComponent()
        {
            // Stvoryuemo textBox dlya vyvodu logu
            this.textBox_vuvid = new System.Windows.Forms.TextBox();
            // Stvoryuemo knopku dlya start/stop
            this.knopka_start = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // Nastavlyayem parametry textBox
            // Location
            this.textBox_vuvid.Location = new System.Drawing.Point(12, 12);
            // Multiline
            this.textBox_vuvid.Multiline = true;
            this.textBox_vuvid.Name = "textBox_vuvid";
            // ReadOnly
            this.textBox_vuvid.ReadOnly = true;
            // ScrollBars
            this.textBox_vuvid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            // Size
            this.textBox_vuvid.Size = new System.Drawing.Size(360, 200);
            this.textBox_vuvid.TabIndex = 0;

            // Nastavlyayem parametry knopky
            // Location
            this.knopka_start.Location = new System.Drawing.Point(12, 218);
            this.knopka_start.Name = "knopka_start";
            // Size
            this.knopka_start.Size = new System.Drawing.Size(360, 30);
            this.knopka_start.TabIndex = 1;
            this.knopka_start.Text = "Start Logging";
            this.knopka_start.UseVisualStyleBackColor = true;
            // Click
            this.knopka_start.Click += new System.EventHandler(this.knopka_start_Click);

            // Nastavlyayem parametry formy
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            // ClientSize - rozmir formy (384 szerokist, 261 vysota)
            this.ClientSize = new System.Drawing.Size(384, 261);
            // Dodayemo kontrol na formu
            this.Controls.Add(this.knopka_start);
            this.Controls.Add(this.textBox_vuvid);
            this.Name = "Form1";
            this.Text = "Keyboard Hook Logger";
            // Load - pidpisuemo na podiyu zavantuvannya formy
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Zminnye dlya kontroliv
        private System.Windows.Forms.TextBox textBox_vuvid;
        private System.Windows.Forms.Button knopka_start;
    }
}