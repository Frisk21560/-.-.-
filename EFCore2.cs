using EFConsole2.Data2;
using EFConsole2.Models2;
using System;
using System.Linq;

namespace EFConsole2
{
    internal class Program
    {
        static MoviesContext context = new MoviesContext();
        static User currentUser = null;

        static void Main(string[] args)
        {
            // Створення БД при першому запуску
            context.Database.EnsureCreated();

            MainMenu();
        }

        // ГОЛОВНЕ МЕНЮ
        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("СИСТЕМА УПРАВЛІННЯ ФІЛЬМАМИ");
                Console.WriteLine();

                if (currentUser != null)
                {
                    Console.WriteLine($"Ви авторизовані як: {currentUser.Name}");
                    Console.WriteLine();
                    Console.WriteLine("1. Перегляд фільмів");
                    Console.WriteLine("2. Перегляд користувачів");
                    Console.WriteLine("3. Vихid з профілю");
                    Console.WriteLine("4. Завершити програму");
                }
                else
                {
                    Console.WriteLine("1. Реєстрація користувача");
                    Console.WriteLine("2. Vхid до системи");
                    Console.WriteLine("3. Перегляд користувачів");
                    Console.WriteLine("4. Завершити програму");
                }

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                if (currentUser != null)
                {
                    switch (choice)
                    {
                        case "1":
                            ViewMoviesMenu();
                            break;
                        case "2":
                            ViewUsersMenu();
                            break;
                        case "3":
                            Logout();
                            break;
                        case "4":
                            ExitApplication();
                            return;
                        default:
                            Console.WriteLine("Невірна опція!");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case "1":
                            RegisterUserMenu();
                            break;
                        case "2":
                            LoginMenu();
                            break;
                        case "3":
                            ViewUsersMenu();
                            break;
                        case "4":
                            ExitApplication();
                            return;
                        default:
                            Console.WriteLine("Невірна опція!");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        //МЕНЮ РЕЄСТРАЦІЇ
        static void RegisterUserMenu()
        {
            Console.Clear();
            Console.WriteLine("РЕЄСТРАЦІЯ НОВОГО КОРИСТУВАЧА");
            Console.WriteLine();

            Console.Write("Введіть імя: ");
            string name = Console.ReadLine();

            Console.Write("Введіть логін: ");
            string login = Console.ReadLine();

            // Перевірка чи логін вже існує
            if (context.Users.Any(u => u.Login == login))
            {
                Console.WriteLine("\nТакий логін вже існує!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            var newUser = new User
            {
                Name = name,
                Login = login,
                Password = password
            };

            context.Users.Add(newUser);
            context.SaveChanges();

            Console.WriteLine("\nКористувач успішно зареєстрований!");
            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey();
        }

        //МЕНЮ ВХОДУ
        static void LoginMenu()
        {
            Console.Clear();
            Console.WriteLine("VХID ДО СИСТЕМИ");
            Console.WriteLine();

            Console.Write("Введіть логін: ");
            string login = Console.ReadLine();

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            var user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                currentUser = user;
                Console.WriteLine($"\nВітаємо, {user.Name}!");
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\nНевірний логін або пароль!");
                Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню...");
                Console.ReadKey();
            }
        }

        // МЕНЮ ПЕРЕГЛЯДУ ФІЛЬМІВ
        static void ViewMoviesMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("МЕНЮ ФІЛЬМІВ");
                Console.WriteLine();

                Console.WriteLine("1. Показати всі фільми");
                Console.WriteLine("2. Додати новий фільм");
                Console.WriteLine("3. Видалити фільм");
                Console.WriteLine("4. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllMovies();
                        break;
                    case "2":
                        AddMovie();
                        break;
                    case "3":
                        DeleteMovie();
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

        static void DisplayAllMovies()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ВСІХ ФІЛЬМІВ");
            Console.WriteLine();

            var movies = context.Titles.ToList();

            if (movies.Count == 0)
            {
                Console.WriteLine("Немає фільмів в базі даних!");
            }
            else
            {
                foreach (var movie in movies)
                {
                    Console.WriteLine($"ID: {movie.Id}");
                    Console.WriteLine($"Назва: {movie.Name}");
                    Console.WriteLine($"Тривалість: {movie.Duration} хв");
                    Console.WriteLine();
                }
                Console.WriteLine($"Всього фільмів: {movies.Count}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        static void AddMovie()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ НОВОГО ФІЛЬМУ");
            Console.WriteLine();

            Console.Write("Введіть назву фільму: ");
            string name = Console.ReadLine();

            Console.Write("Введіть тривалість (в хвилинах): ");
            if (!int.TryParse(Console.ReadLine(), out int duration))
            {
                Console.WriteLine("Невірна тривалість!");
                Console.ReadKey();
                return;
            }

            var movie = new Title
            {
                Name = name,
                Duration = duration
            };

            context.Titles.Add(movie);
            context.SaveChanges();

            Console.WriteLine("\nФільм успішно додан!");
            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        static void DeleteMovie()
        {
            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ ФІЛЬМУ");
            Console.WriteLine();

            Console.Write("Введіть ID фільму для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Невірний ID!");
                Console.ReadKey();
                return;
            }

            var movie = context.Titles.Find(id);
            if (movie == null)
            {
                Console.WriteLine("Фільм не знайдено!");
                Console.ReadKey();
                return;
            }

            context.Titles.Remove(movie);
            context.SaveChanges();

            Console.WriteLine("Фільм успішно видалений!");
            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        // МЕНЮ ПЕРЕГЛЯДУ КОРИСТУВАЧІВ
        static void ViewUsersMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("МЕНЮ КОРИСТУВАЧІВ");
                Console.WriteLine();

                Console.WriteLine("1. Показати всіх користувачів");
                Console.WriteLine("2. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllUsers();
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DisplayAllUsers()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ВСІХ КОРИСТУВАЧІВ");
            Console.WriteLine();

            var users = context.Users.ToList();

            if (users.Count == 0)
            {
                Console.WriteLine("Немає користувачів в базі даних!");
            }
            else
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.Id}");
                    Console.WriteLine($"Імя: {user.Name}");
                    Console.WriteLine($"Логін: {user.Login}");
                    Console.WriteLine();
                }
                Console.WriteLine($"Всього користувачів: {users.Count}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        //ВИХІД
        static void Logout()
        {
            currentUser = null;
            Console.WriteLine("Ви вийшли з системи!");
            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey();
        }

        static void ExitApplication()
        {
            Console.Clear();
            Console.WriteLine("Дякуємо за використання системи!");
            context.Dispose();
        }
    }
}