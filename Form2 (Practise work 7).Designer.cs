namespace Legacy_Code_Practies_work_7
{
    partial class Form2
    {
        // Kontejner dlya komponentiv formy
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
            // Stvoryuemo knopku sho bude vtikaty
            this.knopka_evade = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // Nastavlyayem parametry knopky
            // Location
            this.knopka_evade.Location = new System.Drawing.Point(200, 150);
            this.knopka_evade.Name = "knopka_evade";
            // Size
            this.knopka_evade.Size = new System.Drawing.Size(100, 40);
            this.knopka_evade.TabIndex = 0;
            // Text
            this.knopka_evade.Text = "Try Click Me!";
            this.knopka_evade.UseVisualStyleBackColor = true;
            // BackColor
            this.knopka_evade.BackColor = System.Drawing.Color.LightBlue;

            // Nastavlyayem parametry formy
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            // ClientSize
            this.ClientSize = new System.Drawing.Size(500, 400);
            // Dodayemo knopku na formu
            this.Controls.Add(this.knopka_evade);
            this.Name = "Form2";
            this.Text = "Evading Button";
            // Load
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
        }

        // Zminna dlya knopky
        private System.Windows.Forms.Button knopka_evade;
    }
}