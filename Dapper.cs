using Dapper;
using DapperDemo.Models;
using System.Data.SqlClient;

namespace DapperDemo
{
    class Program
    {
        private const string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DapperDemo;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            InitDogsTable(con);

            MainMenu(con);
        }

        // ІНІЦІАЛІЗАЦІЯ ТАБЛИЦІ
        private static void InitDogsTable(SqlConnection con)
        {
            string query = @"
            IF OBJECT_ID(N'dbo.Dogs', N'U') IS NULL
            BEGIN
                CREATE TABLE Dogs (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Age INT NOT NULL,
                    Breed NVARCHAR(100) NOT NULL,
                    IsAdopted BIT DEFAULT 0
                );
            END
            ";

            con.Execute(query);
            Console.WriteLine("[LOG] Таблиця Dogs ініціалізована.");
        }

        // ГОЛОВНЕ МЕНЮ
        static void MainMenu(SqlConnection con)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ПРИЛІТОК ДЛЯ СОБАК ===\n");
                Console.WriteLine("1. Додати собаку");
                Console.WriteLine("2. Показати всіх собак");
                Console.WriteLine("3. Показати собак у притулку");
                Console.WriteLine("4. Показати забраних собак");
                Console.WriteLine("5. Пошук собаки");
                Console.WriteLine("6. Вихід");
                Console.Write("\nВиберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddDogMenu(con);
                        break;
                    case "2":
                        ViewAllDogs(con);
                        break;
                    case "3":
                        ViewAvailableDogs(con);
                        break;
                    case "4":
                        ViewAdoptedDogs(con);
                        break;
                    case "5":
                        SearchDogMenu(con);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // ДОДАВАННЯ СОБАКИ
        static void AddDogMenu(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ДОДАВАННЯ СОБАКИ ===\n");

            Console.Write("Клічка: ");
            string name = Console.ReadLine();

            Console.Write("Вік: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Порода: ");
            string breed = Console.ReadLine();

            var dog = new Dog
            {
                Name = name,
                Age = age,
                Breed = breed,
                IsAdopted = false
            };

            AddDog(con, dog);

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static void AddDog(SqlConnection con, Dog dog)
        {
            string query = @"
                INSERT INTO Dogs (Name, Age, Breed, IsAdopted)
                VALUES (@Name, @Age, @Breed, @IsAdopted);
            ";

            con.Execute(query, dog);
            Console.WriteLine($"[LOG] Собака {dog.Name} успішно додана до притулку!");
        }

        // ПЕРЕГЛЯД ВСІХ СОБ��К
        static void ViewAllDogs(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("ВСІ СОБАКИ\n");

            var dogs = ReadAllDogs(con);

            if (dogs.Count > 0)
            {
                foreach (var dog in dogs)
                {
                    Console.WriteLine(dog);
                }
            }
            else
            {
                Console.WriteLine("У базі даних немає собак.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static List<Dog> ReadAllDogs(SqlConnection con)
        {
            string query = @"
                SELECT Id, Name, Age, Breed, IsAdopted FROM Dogs
                ORDER BY Id DESC
            ";

            return con.Query<Dog>(query).ToList();
        }

        // ПЕРЕГЛЯД СОБАК У ПРИТУЛКУ 
        static void ViewAvailableDogs(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("СОБАКИ У ПРИТУЛКУ\n");

            var dogs = ReadAvailableDogs(con);

            if (dogs.Count > 0)
            {
                foreach (var dog in dogs)
                {
                    Console.WriteLine(dog);
                }
                Console.WriteLine($"\nВсього: {dogs.Count} собак(и)");
            }
            else
            {
                Console.WriteLine("Всі собаки знайшли своїх господарів!");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static List<Dog> ReadAvailableDogs(SqlConnection con)
        {
            string query = @"
                SELECT Id, Name, Age, Breed, IsAdopted FROM Dogs
                WHERE IsAdopted = 0
                ORDER BY Name
            ";

            return con.Query<Dog>(query).ToList();
        }

        //ПЕРЕГЛЯД ЗАБРАНИХ СОБАК
        static void ViewAdoptedDogs(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine(" ЗАБРАНІ СОБАКИ \n");

            var dogs = ReadAdoptedDogs(con);

            if (dogs.Count > 0)
            {
                foreach (var dog in dogs)
                {
                    Console.WriteLine(dog);
                }
                Console.WriteLine($"\nВсього: {dogs.Count} собак(и) знайшло дім");
            }
            else
            {
                Console.WriteLine("Поки що ніхто не взяв собаку!");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static List<Dog> ReadAdoptedDogs(SqlConnection con)
        {
            string query = @"
                SELECT Id, Name, Age, Breed, IsAdopted FROM Dogs
                WHERE IsAdopted = 1
                ORDER BY Name
            ";

            return con.Query<Dog>(query).ToList();
        }

        // ПОШУК СОБАКИ
        static void SearchDogMenu(SqlConnection con)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ПОШУК СОБАКИ ===\n");
                Console.WriteLine("1. Пошук за клічкою");
                Console.WriteLine("2. Пошук за ID");
                Console.WriteLine("3. Пошук за породою");
                Console.WriteLine("4. Повернутись");
                Console.Write("\nВиберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SearchDogByName(con);
                        break;
                    case "2":
                        SearchDogById(con);
                        break;
                    case "3":
                        SearchDogByBreed(con);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void SearchDogByName(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ПОШУК ЗА КЛІЧКОЮ ===\n");
            Console.Write("Введіть клічку (або частину): ");
            string name = Console.ReadLine();

            var dogs = FindDogsByName(con, name);

            if (dogs.Count > 0)
            {
                foreach (var dog in dogs)
                {
                    Console.WriteLine(dog);
                }
            }
            else
            {
                Console.WriteLine("Собак з такою клічкою не знайдено.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static List<Dog> FindDogsByName(SqlConnection con, string name)
        {
            string query = @"
                SELECT Id, Name, Age, Breed, IsAdopted FROM Dogs
                WHERE Name LIKE '%' + @Name + '%'
                ORDER BY Name
            ";

            return con.Query<Dog>(query, new { Name = name }).ToList();
        }

        private static void SearchDogById(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ПОШУК ЗА ID ===\n");
            Console.Write("Введіть ID: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var dog = FindDogById(con, id);

                if (dog != null)
                {
                    Console.WriteLine($"\nЗнайдено:\n{dog}");
                }
                else
                {
                    Console.WriteLine("Собаку з таким ID не знайдено.");
                }
            }
            else
            {
                Console.WriteLine("Невірний формат ID.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static Dog FindDogById(SqlConnection con, int id)
        {
            string query = @"
                SELECT Id, Name, Age, Breed, IsAdopted FROM Dogs
                WHERE Id = @Id
            ";

            return con.QueryFirstOrDefault<Dog>(query, new { Id = id });
        }

        private static void SearchDogByBreed(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ПОШУК ЗА ПОРОДОЮ ===\n");
            Console.Write("Введіть породу (або частину): ");
            string breed = Console.ReadLine();

            var dogs = FindDogsByBreed(con, breed);

            if (dogs.Count > 0)
            {
                foreach (var dog in dogs)
                {
                    Console.WriteLine(dog);
                }
            }
            else
            {
                Console.WriteLine("Собак з такою породою не знайдено.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static List<Dog> FindDogsByBreed(SqlConnection con, string breed)
        {
            string query = @"
                SELECT Id, Name, Age, Breed, IsAdopted FROM Dogs
                WHERE Breed LIKE '%' + @Breed + '%'
                ORDER BY Breed
            ";

            return con.Query<Dog>(query, new { Breed = breed }).ToList();
        }
    }
}