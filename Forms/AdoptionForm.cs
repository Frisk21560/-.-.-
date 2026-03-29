using Dapper3.DataAccess;
using Dapper3.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Dapper3
{
    public partial class AdoptionForm : Form
    {
        private List<Adopter> _adopters;
        private DogRepository _repository;
        private Dog _selectedDog;

        public AdoptionForm(List<Adopter> adopters, DogRepository repository, Dog dog)
        {
            InitializeComponent();
            _adopters = adopters;
            _repository = repository;
            _selectedDog = dog;
            LoadAdopters();
        }

        private void LoadAdopters()
        {
            comboBoxAdopters.Items.Clear();
            foreach (var adopter in _adopters)
            {
                comboBoxAdopters.Items.Add(adopter);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (comboBoxAdopters.SelectedItem is not Adopter selectedAdopter)
            {
                MessageBox.Show("Виберіть опікуна!", "Помилка");
                return;
            }

            _repository.AdoptDog(_selectedDog.Id, selectedAdopter.Id);
            MessageBox.Show($"Собака {_selectedDog.Name} успішно передана {selectedAdopter.FullName}!", "Успіх");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = "Адопція собаки";
            this.Size = new System.Drawing.Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;

            var lblDog = new Label() { Text = $"Собака: {_selectedDog.Name}", Location = new System.Drawing.Point(20, 20), AutoSize = true };
            var lblAdopter = new Label() { Text = "Виберіть опікуна:", Location = new System.Drawing.Point(20, 60), AutoSize = true };

            comboBoxAdopters = new ComboBox() { Location = new System.Drawing.Point(20, 80), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };

            btnConfirm = new Button() { Text = "Підтвердити", Location = new System.Drawing.Point(100, 120), Width = 100 };
            btnConfirm.Click += btnConfirm_Click;

            btnCancel = new Button() { Text = "Скасувати", Location = new System.Drawing.Point(210, 120), Width = 100 };
            btnCancel.Click += btnCancel_Click;

            this.Controls.Add(lblDog);
            this.Controls.Add(lblAdopter);
            this.Controls.Add(comboBoxAdopters);
            this.Controls.Add(btnConfirm);
            this.Controls.Add(btnCancel);
        }

        private ComboBox comboBoxAdopters;
        private Button btnConfirm;
        private Button btnCancel;
    }
}