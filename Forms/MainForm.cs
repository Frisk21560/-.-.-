using Dapper3.DataAccess;
using Dapper3.Forms;
using Dapper3.Models;
using System;
using System.Windows.Forms;

namespace Dapper3
{
    public partial class MainForm : Form
    {
        private const string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Dapper3;Integrated Security=True;TrustServerCertificate=True;";
        private DogRepository _repository;

        public MainForm()
        {
            InitializeComponent();
            _repository = new DogRepository(_connectionString);
            _repository.InitTables();
            LoadDogs();
        }

        private void LoadDogs()
        {
            var dogs = _repository.GetAllDogs();
            listBoxDogs.Items.Clear();
            foreach (var dog in dogs)
            {
                listBoxDogs.Items.Add(dog);
            }
        }

        private void btnAddDog_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDogName.Text) || string.IsNullOrWhiteSpace(txtBreed.Text))
            {
                MessageBox.Show("Заповніть усі поля!", "Помилка");
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Введіть коректний вік!", "Помилка");
                return;
            }

            var dog = new Dog
            {
                Name = txtDogName.Text,
                Age = age,
                Breed = txtBreed.Text,
                IsAdopted = false
            };

            _repository.AddDog(dog);
            MessageBox.Show("Собаку успішно додано!", "Успіх");
            ClearDogFields();
            LoadDogs();
        }

        private void btnViewAvailable_Click(object sender, EventArgs e)
        {
            var dogs = _repository.GetAvailableDogs();
            listBoxDogs.Items.Clear();
            foreach (var dog in dogs)
            {
                listBoxDogs.Items.Add(dog);
            }
            lblStatus.Text = $"Собак у притулку: {dogs.Count}";
        }

        private void btnViewAdopted_Click(object sender, EventArgs e)
        {
            var dogs = _repository.GetAdoptedDogs();
            listBoxDogs.Items.Clear();
            foreach (var dog in dogs)
            {
                listBoxDogs.Items.Add(dog);
            }
            lblStatus.Text = $"Забраних собак: {dogs.Count}";
        }

        private void btnAdopt_Click(object sender, EventArgs e)
        {
            if (listBoxDogs.SelectedItem is not Dog selectedDog)
            {
                MessageBox.Show("Виберіть собаку!", "Помилка");
                return;
            }

            if (selectedDog.IsAdopted)
            {
                MessageBox.Show("Цю собаку вже забрали!", "Помилка");
                return;
            }

            var adopters = _repository.GetAllAdopters();
            if (adopters.Count == 0)
            {
                MessageBox.Show("Немає опікунів у базі! Спочатку додайте опікуна.", "Помилка");
                return;
            }

            var adoptForm = new AdoptionForm(adopters, _repository, selectedDog);
            adoptForm.ShowDialog();
            LoadDogs();
        }

        private void btnAddAdopter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAdopterName.Text) || string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                MessageBox.Show("Заповніть усі поля!", "Помилка");
                return;
            }

            var adopter = new Adopter
            {
                FullName = txtAdopterName.Text,
                PhoneNumber = txtPhoneNumber.Text
            };

            _repository.AddAdopter(adopter);
            MessageBox.Show("Опікуна успішно додано!", "Успіх");
            ClearAdopterFields();
        }

        private void btnSearchDog_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchDog.Text))
            {
                LoadDogs();
                return;
            }

            var dogs = _repository.SearchDogsByName(txtSearchDog.Text);
            listBoxDogs.Items.Clear();
            foreach (var dog in dogs)
            {
                listBoxDogs.Items.Add(dog);
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadDogs();
            lblStatus.Text = $"Всього собак: {listBoxDogs.Items.Count}";
        }

        private void ClearDogFields()
        {
            txtDogName.Clear();
            txtAge.Clear();
            txtBreed.Clear();
        }

        private void ClearAdopterFields()
        {
            txtAdopterName.Clear();
            txtPhoneNumber.Clear();
        }
    }
}