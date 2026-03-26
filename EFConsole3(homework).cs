using EFConsole3_Homework.Data;
using EFConsole3_Homework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFConsole3_Homework
{
    internal class Program
    {
        static PlatformContext context = new PlatformContext();
        static User currentUser = null;

        static void Main(string[] args)
        {
            context.Database.EnsureCreated();
            MainMenu();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ПЛАТФОРМА УПРАВЛІННЯ ФІЛЬМАМИ");
                Console.WriteLine();

                if (currentUser != null)
                {
                    Console.WriteLine($"Ви авторизовані як: {currentUser.Username}");
                    Console.WriteLine();
                    Console.WriteLine("1. Управління моїми фільмами");
                    Console.WriteLine("2. Переглянути всі фільми");
                    Console.WriteLine("3. Переглянути користувачів");
                    Console.WriteLine("4. Вихід з профілю");
                    Console.WriteLine("5. Завершити програму");
                }
                else
                {
                    Console.WriteLine("1. Реєстрація користувача");
                    Console.WriteLine("2. Vход до системи");
                    Console.WriteLine("3. Переглянути всі фільми");
                    Console.WriteLine("4. Переглянути користувачів");
                    Console.WriteLine("5. Завершити програму");
                }

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                if (currentUser != null)
                {
                    switch (choice)
                    {
                        case "1":
                            MyMoviesMenu();
                            break;
                        case "2":
                            ViewAllMovies();
                            break;
                        case "3":
                            ViewAllUsers();
                            break;
                        case "4":
                            Logout();
                            break;
                        case "5":
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
                            RegisterUser();
                            break;
                        case "2":
                            LoginUser();
                            break;
                        case "3":
                            ViewAllMovies();
                            break;
                        case "4":
                            ViewAllUsers();
                            break;
                        case "5":
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

        // РЕЄСТРАЦІЯ
        static void RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("РЕЄСТРАЦІЯ НОВОГО КОРИСТУВАЧА");
            Console.WriteLine();

            Console.Write("Введіть username: ");
            string username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username не може бути пустим!");
                Console.ReadKey();
                return;
            }

            if (context.Users.Any(u => u.Username == username))
            {
                Console.WriteLine("Такий username вже існує!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть email: ");
            string email = Console.ReadLine();

            if (!IsValidEmail(email))
            {
                Console.WriteLine("Невірний формат email!");
                Console.ReadKey();
                return;
            }

            if (context.Users.Any(u => u.Email == email))
            {
                Console.WriteLine("Цей email вже зареєстрований!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Пароль не може бути пустим!");
                Console.ReadKey();
                return;
            }

            var user = new User
            {
                Username = username,
                Email = email,
                Password = password
            };

            context.Users.Add(user);
            context.SaveChanges();

            Console.WriteLine("Користувач успішно зареєстрований!");
            Console.ReadKey();
        }

        // ВХОД 
        static void LoginUser()
        {
            Console.Clear();
            Console.WriteLine("VХОД ДО СИСТЕМИ");
            Console.WriteLine();

            Console.Write("Введіть username: ");
            string username = Console.ReadLine();

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                currentUser = user;
                Console.WriteLine($"Вітаємо, {user.Username}!");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Невірний username або пароль!");
                Console.ReadKey();
            }
        }

        // УПРАВЛІННЯ МОЇМИ ФІЛЬМАМИ
        static void MyMoviesMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ МОЇМИ ФІЛЬМАМИ");
                Console.WriteLine();
                Console.WriteLine("1. Додати новий фільм");
                Console.WriteLine("2. Показати мої фільми");
                Console.WriteLine("3. Видалити фільм");
                Console.WriteLine("4. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddMovie();
                        break;
                    case "2":
                        DisplayMyMovies();
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

        static void AddMovie()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ НОВОГО ФІЛЬМУ");
            Console.WriteLine();

            Console.Write("Введіть назву фільму (max 50 символів): ");
            string title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title) || title.Length > 50)
            {
                Console.WriteLine("Невірна назва фільму!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть рік виходу (більше 0): ");
            if (!int.TryParse(Console.ReadLine(), out int releaseYear) || releaseYear <= 0)
            {
                Console.WriteLine("Невірний рік виходу!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть опис (можна залишити пустим): ");
            string description = Console.ReadLine();

            var movie = new Movie
            {
                Title = title,
                ReleaseYear = releaseYear,
                Description = string.IsNullOrWhiteSpace(description) ? null : description,
                DateAdded = DateTime.Now,
                UserId = currentUser.Id
            };

            context.Movies.Add(movie);
            context.SaveChanges();

            Console.WriteLine("Фільм успішно додан!");
            Console.ReadKey();
        }

        static void DisplayMyMovies()
        {
            Console.Clear();
            Console.WriteLine("МОЇ ФІЛЬМИ");
            Console.WriteLine();

            var movies = context.Movies
                .Where(m => m.UserId == currentUser.Id)
                .OrderByDescending(m => m.DateAdded)
                .ToList();

            if (movies.Count == 0)
            {
                Console.WriteLine("Ви не додали жодного фільму!");
            }
            else
            {
                foreach (var movie in movies)
                {
                    Console.WriteLine($"ID: {movie.Id}");
                    Console.WriteLine($"Назва: {movie.Title}");
                    Console.WriteLine($"Рік виходу: {movie.ReleaseYear}");
                    Console.WriteLine($"Опис: {movie.Description ?? "Не вказаний"}");
                    Console.WriteLine($"Дата додавання: {movie.DateAdded:dd.MM.yyyy HH:mm}");
                    Console.WriteLine();
                }
                Console.WriteLine($"Всього фільмів: {movies.Count}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        static void DeleteMovie()
        {
            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ ФІЛЬМУ");
            Console.WriteLine();

            Console.Write("Введіть ID фільму для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int movieId))
            {
                Console.WriteLine("Невірний ID!");
                Console.ReadKey();
                return;
            }

            var movie = context.Movies.FirstOrDefault(m => m.Id == movieId && m.UserId == currentUser.Id);
            if (movie == null)
            {
                Console.WriteLine("Фільм не знайдено або він не належить вам!");
                Console.ReadKey();
                return;
            }

            context.Movies.Remove(movie);
            context.SaveChanges();

            Console.WriteLine("Фільм успішно видалений!");
            Console.ReadKey();
        }

        // ПЕРЕГЛЯД ВСІХ ФІЛЬМІВ
        static void ViewAllMovies()
        {
            Console.Clear();
            Console.WriteLine("ВСІ ФІЛЬМИ НА ПЛАТФОРМІ");
            Console.WriteLine();

            var movies = context.Movies
                .Include(m => m.User)
                .OrderByDescending(m => m.DateAdded)
                .ToList();

            if (movies.Count == 0)
            {
                Console.WriteLine("На платформі немає фільмів!");
            }
            else
            {
                foreach (var movie in movies)
                {
                    Console.WriteLine($"ID: {movie.Id}");
                    Console.WriteLine($"Назва: {movie.Title}");
                    Console.WriteLine($"Рік виходу: {movie.ReleaseYear}");
                    Console.WriteLine($"Опис: {movie.Description ?? "Не вказаний"}");
                    Console.WriteLine($"Додав користувач: {movie.User?.Username}");
                    Console.WriteLine($"Дата додавання: {movie.DateAdded:dd.MM.yyyy HH:mm}");
                    Console.WriteLine();
                }
                Console.WriteLine($"Всього фільмів: {movies.Count}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        // ПЕРЕГЛЯД КОРИСТУВАЧІВ
        static void ViewAllUsers()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК КОРИСТУВАЧІВ");
            Console.WriteLine();

            var users = context.Users.Include(u => u.Movies).ToList();

            if (users.Count == 0)
            {
                Console.WriteLine("Немає користувачів в системі!");
            }
            else
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.Id}");
                    Console.WriteLine($"Username: {user.Username}");
                    Console.WriteLine($"Email: {user.Email}");
                    Console.WriteLine($"Додав фільмів: {user.Movies.Count}");
                    Console.WriteLine();
                }
                Console.WriteLine($"Всього користувачів: {users.Count}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        // ДОПОМІЖНІ ФУНКЦІЇ
        static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

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
            Console.WriteLine("Дякуємо за використання платформи!");
            context.Dispose();
        }
    }
}