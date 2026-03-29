using System.Reflection.Emit;

namespace Dapper3
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private ListBox listBoxDogs;
        private TextBox txtDogName;
        private TextBox txtAge;
        private TextBox txtBreed;
        private TextBox txtAdopterName;
        private TextBox txtPhoneNumber;
        private TextBox txtSearchDog;
        private Button btnAddDog;
        private Button btnViewAvailable;
        private Button btnViewAdopted;
        private Button btnAdopt;
        private Button btnAddAdopter;
        private Button btnSearchDog;
        private Button btnViewAll;
        private Label lblStatus;
        private Label lblDogName;
        private Label lblAge;
        private Label lblBreed;
        private Label lblAdopterName;
        private Label lblPhoneNumber;
        private Label lblSearch;

        private void InitializeComponent()
        {
            this.Text = "Приліток для собак";
            this.Size = new System.Drawing.Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // ListBox для собак
            listBoxDogs = new ListBox();
            listBoxDogs.Location = new System.Drawing.Point(20, 20);
            listBoxDogs.Size = new System.Drawing.Size(600, 300);
            this.Controls.Add(listBoxDogs);

            // Група "Додавання собаки"
            lblDogName = new Label() { Text = "Клічка:", Location = new System.Drawing.Point(650, 20), AutoSize = true };
            txtDogName = new TextBox() { Location = new System.Drawing.Point(650, 40), Width = 200 };
            lblAge = new Label() { Text = "Вік:", Location = new System.Drawing.Point(650, 70), AutoSize = true };
            txtAge = new TextBox() { Location = new System.Drawing.Point(650, 90), Width = 200 };
            lblBreed = new Label() { Text = "Порода:", Location = new System.Drawing.Point(650, 120), AutoSize = true };
            txtBreed = new TextBox() { Location = new System.Drawing.Point(650, 140), Width = 200 };
            btnAddDog = new Button() { Text = "Додати собаку", Location = new System.Drawing.Point(650, 170), Width = 200 };
            btnAddDog.Click += btnAddDog_Click;

            this.Controls.Add(lblDogName);
            this.Controls.Add(txtDogName);
            this.Controls.Add(lblAge);
            this.Controls.Add(txtAge);
            this.Controls.Add(lblBreed);
            this.Controls.Add(txtBreed);
            this.Controls.Add(btnAddDog);

            // Група "Перегляд"
            btnViewAll = new Button() { Text = "Всі собаки", Location = new System.Drawing.Point(20, 330), Width = 140 };
            btnViewAll.Click += btnViewAll_Click;
            btnViewAvailable = new Button() { Text = "У притулку", Location = new System.Drawing.Point(170, 330), Width = 140 };
            btnViewAvailable.Click += btnViewAvailable_Click;
            btnViewAdopted = new Button() { Text = "Забрані", Location = new System.Drawing.Point(320, 330), Width = 140 };
            btnViewAdopted.Click += btnViewAdopted_Click;

            this.Controls.Add(btnViewAll);
            this.Controls.Add(btnViewAvailable);
            this.Controls.Add(btnViewAdopted);

            // Пошук
            lblSearch = new Label() { Text = "Пошук за клічкою:", Location = new System.Drawing.Point(20, 370), AutoSize = true };
            txtSearchDog = new TextBox() { Location = new System.Drawing.Point(20, 390), Width = 300 };
            btnSearchDog = new Button() { Text = "Пошук", Location = new System.Drawing.Point(330, 390), Width = 80 };
            btnSearchDog.Click += btnSearchDog_Click;

            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearchDog);
            this.Controls.Add(btnSearchDog);

            // Адопція
            btnAdopt = new Button() { Text = "Забрати собаку", Location = new System.Drawing.Point(20, 430), Width = 140, Height = 40 };
            btnAdopt.Click += btnAdopt_Click;
            this.Controls.Add(btnAdopt);

            // Група "Додавання опікуна"
            lblAdopterName = new Label() { Text = "Ім'я опікуна:", Location = new System.Drawing.Point(650, 320), AutoSize = true };
            txtAdopterName = new TextBox() { Location = new System.Drawing.Point(650, 340), Width = 200 };
            lblPhoneNumber = new Label() { Text = "Номер телефону:", Location = new System.Drawing.Point(650, 370), AutoSize = true };
            txtPhoneNumber = new TextBox() { Location = new System.Drawing.Point(650, 390), Width = 200 };
            btnAddAdopter = new Button() { Text = "Додати опікуна", Location = new System.Drawing.Point(650, 420), Width = 200 };
            btnAddAdopter.Click += btnAddAdopter_Click;

            this.Controls.Add(lblAdopterName);
            this.Controls.Add(txtAdopterName);
            this.Controls.Add(lblPhoneNumber);
            this.Controls.Add(txtPhoneNumber);
            this.Controls.Add(btnAddAdopter);

            // Статус
            lblStatus = new Label() { Text = "Готово", Location = new System.Drawing.Point(20, 600), AutoSize = true };
            this.Controls.Add(lblStatus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}