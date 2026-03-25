using Microsoft.Data.SqlClient;
using System;

namespace ADO_Lesson
{
    internal class Program
    {
        private const string _connectionString = @"
           Data Source=(localdb)\MSSQLLocalDB; 
           Initial Catalog= ADO_Lesson_DB;
           Integrated Security=True;TrustServerCertificate=True;
         ";

        static void Main(string[] args)
        {
            InitializeDatabase();

            while (true)
            {
                Console.WriteLine("\n УПРАВЛIННЯ КОРИСТУВАЧАМИ");
                Console.WriteLine("1. Зареєструвати користувача");
                Console.WriteLine("2. Показати всiх користувачiв");
                Console.WriteLine("3. Пошук за username");
                Console.WriteLine("4. Пошук за email");
                Console.WriteLine("5. Видалити користувача");
                Console.WriteLine("6. Вихiд");
                Console.Write("Виберіть операцiю: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        DisplayAllUsers();
                        break;
                    case "3":
                        SearchByUsername();
                        break;
                    case "4":
                        SearchByEmail();
                        break;
                    case "5":
                        DeleteUser();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Невiрна опцiя!");
                        break;
                }
            }
        }

        static void InitializeDatabase()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
                        CREATE TABLE Users (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Username NVARCHAR(50) NOT NULL UNIQUE,
                            Email NVARCHAR(100) NOT NULL UNIQUE,
                            Password NVARCHAR(255) NOT NULL,
                            BirthDate DATE NULL
                        )";

                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("✓ База даних готова!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при iнiцiалiзацiї БД: {ex.Message}");
                }
            }
        }

        static void RegisterUser()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            Console.Write("Дата народження (YYYY-MM-DD) [на пропуск натиснiть Enter]: ");
            string birthDateStr = Console.ReadLine();
            DateTime? birthDate = string.IsNullOrEmpty(birthDateStr) ? null : DateTime.Parse(birthDateStr);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO Users (Username, Email, Password, BirthDate) 
                                   VALUES (@username, @email, @password, @birthDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@birthDate", (object)birthDate ?? DBNull.Value);

                        command.ExecuteNonQuery();
                        Console.WriteLine("✓ Користувач успiшно зареєстрований!");
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"✗ Помилка: {ex.Message}");
                }
            }
        }

        static void DisplayAllUsers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Id, Username, Email, BirthDate FROM Users";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Немає користувачiв!");
                                return;
                            }

                            Console.WriteLine("\n=== СПИСОК КОРИСТУВАЧIВ ===");
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string username = reader.GetString(1);
                                string email = reader.GetString(2);
                                DateTime? birthDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3);

                                Console.WriteLine($"\nID: {id}");
                                Console.WriteLine($"Username: {username}");
                                Console.WriteLine($"Email: {email}");
                                Console.WriteLine($"Дата народження: {(birthDate?.ToString("dd.MM.yyyy") ?? "Не вказана")}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }

        static void SearchByUsername()
        {
            Console.Write("Введiть username: ");
            string username = Console.ReadLine();

            SearchUser("Username", username);
        }

        static void SearchByEmail()
        {
            Console.Write("Введiть email: ");
            string email = Console.ReadLine();

            SearchUser("Email", email);
        }

        static void SearchUser(string field, string value)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT Id, Username, Email, BirthDate FROM Users WHERE {field} = @value";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value", value);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("✗ Користувач не знайдений!");
                                return;
                            }

                            Console.WriteLine("\n=== РЕЗУЛЬТАТ ПОШУКУ ===");
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string username = reader.GetString(1);
                                string email = reader.GetString(2);
                                DateTime? birthDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3);

                                Console.WriteLine($"ID: {id}");
                                Console.WriteLine($"Username: {username}");
                                Console.WriteLine($"Email: {email}");
                                Console.WriteLine($"Дата народження: {(birthDate?.ToString("dd.MM.yyyy") ?? "Не вказана")}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }

        static void DeleteUser()
        {
            Console.Write("Введіть ID користувача для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("✗ Невірний ID!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Users WHERE Id = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", userId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("✓ Користувач успiшно видалений!");
                        }
                        else
                        {
                            Console.WriteLine("✗ Користувача з таким ID не знайдено!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }
    }
}