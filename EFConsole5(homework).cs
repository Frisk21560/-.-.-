using EFConsole5.Data;
using EFConsole5.Entities;
using EFConsole5.Services;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5
{
    class Program
    {
        static AppDbContext context = new AppDbContext();
        static UserService userService = new UserService(context);
        static MovieService movieService = new MovieService(context);

        static void Main(string[] args)
        {
            context.Database.EnsureCreated();
            InitializeDatabase();
            MainMenu();
        }

        static void InitializeDatabase()
        {
            if (context.Users.Any())
            {
                return;
            }

            var user1 = new User
            {
                Username = "john_doe",
                Email = "john@gmail.com",
                CreatedDate = DateTime.Now
            };

            var user2 = new User
            {
                Username = "jane_smith",
                Email = "jane@gmail.com",
                CreatedDate = DateTime.Now
            };

            var movie1 = new Movie
            {
                Title = "Inception",
                Description = "A mind-bending thriller",
                ReleaseYear = 2010,
                User = user1,
                AddedDate = DateTime.Now
            };

            var movie2 = new Movie
            {
                Title = "The Matrix",
                Description = "A sci-fi masterpiece",
                ReleaseYear = 1999,
                User = user1,
                AddedDate = DateTime.Now
            };

            var movie3 = new Movie
            {
                Title = "Titanic",
                Description = "A romantic disaster film",
                ReleaseYear = 1997,
                User = user2,
                AddedDate = DateTime.Now
            };

            context.Users.AddRange(user1, user2);
            context.Movies.AddRange(movie1, movie2, movie3);
            context.SaveChanges();

            Console.WriteLine("База даних ініціалізована!");
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ ФІЛЬМАМИ\n");
                Console.WriteLine("1. Управління користувачами");
                Console.WriteLine("2. Управління фільмами");
                Console.WriteLine("3. Показати користувачів та їх фільми (VIEW)");
                Console.WriteLine("4. Вихід");
                Console.Write("\nВиберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UserMenu();
                        break;
                    case "2":
                        MovieMenu();
                        break;
                    case "3":
                        DisplayUsersMoviesView();
                        break;
                    case "4":
                        context.Dispose();
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void UserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("КОРИСТУВАЧІ\n");
                Console.WriteLine("1. Додати користувача");
                Console.WriteLine("2. Показати всіх користувачів");
                Console.WriteLine("3. Пошук користувача за ID");
                Console.WriteLine("4. Оновити користувача");
                Console.WriteLine("5. Видалити користувача");
                Console.WriteLine("6. Показати фільми користувача");
                Console.WriteLine("7. Повернутись");
                Console.Write("\nВиберіть: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddUser();
                        break;
                    case "2":
                        DisplayAllUsers();
                        break;
                    case "3":
                        SearchUser();
                        break;
                    case "4":
                        UpdateUser();
                        break;
                    case "5":
                        DeleteUser();
                        break;
                    case "6":
                        GetUserMovies();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddUser()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ КОРИСТУВАЧА\n");
            Console.Write("Ім'я користувача: ");
            string username = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();

            userService.AddUser(username, email);
            Console.ReadKey();
        }

        static void DisplayAllUsers()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК КОРИСТУВАЧІВ\n");
            var users = userService.GetAllUsers();
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id} | {user.Username} | {user.Email} | Фільмів: {user.Movies.Count}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void SearchUser()
        {
            Console.Clear();
            Console.WriteLine("ПОШУК КОРИСТУВАЧА\n");
            Console.Write("Введіть ID: ");
            int id = int.Parse(Console.ReadLine());
            var user = userService.GetUserById(id);
            if (user != null)
            {
                Console.WriteLine($"\nID: {user.Id}");
                Console.WriteLine($"Ім'я: {user.Username}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Дата створення: {user.CreatedDate:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"Фільмів додано: {user.Movies.Count}");
            }
            else
            {
                Console.WriteLine("Користувач не знайдено!");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void UpdateUser()
        {
            Console.Clear();
            Console.WriteLine("ОНОВЛЕННЯ КОРИСТУВАЧА\n");
            Console.Write("ID користувача: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Нове ім'я: ");
            string username = Console.ReadLine();
            Console.Write("Новий email: ");
            string email = Console.ReadLine();

            userService.UpdateUser(id, username, email);
            Console.ReadKey();
        }

        static void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ КОРИСТУВАЧА\n");
            Console.Write("ID користувача: ");
            int id = int.Parse(Console.ReadLine());

            userService.DeleteUser(id);
            Console.ReadKey();
        }

        static void GetUserMovies()
        {
            Console.Clear();
            Console.WriteLine("ФІЛЬМИ КОРИСТУВАЧА\n");
            Console.Write("ID користувача: ");
            int userId = int.Parse(Console.ReadLine());

            var movies = movieService.GetMoviesByUser(userId);
            if (movies.Count > 0)
            {
                foreach (var movie in movies)
                {
                    Console.WriteLine($"ID: {movie.Id} | {movie.Title} ({movie.ReleaseYear}) | Додано: {movie.AddedDate:dd.MM.yyyy}");
                }
            }
            else
            {
                Console.WriteLine("Користувач не додав жодного фільму!");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void MovieMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ФІЛЬМИ\n");
                Console.WriteLine("1. Додати фільм");
                Console.WriteLine("2. Показати всі фільми");
                Console.WriteLine("3. Пошук фільму за ID");
                Console.WriteLine("4. Оновити фільм");
                Console.WriteLine("5. Видалити фільм");
                Console.WriteLine("6. Повернутись");
                Console.Write("\nВиберіть: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddMovie();
                        break;
                    case "2":
                        DisplayAllMovies();
                        break;
                    case "3":
                        SearchMovie();
                        break;
                    case "4":
                        UpdateMovie();
                        break;
                    case "5":
                        DeleteMovie();
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

        static void AddMovie()
        {
            Console.Clear();
            Console.WriteLine(" ДОДАВАННЯ ФІЛЬМУ\n");
            Console.Write("Назва фільму: ");
            string title = Console.ReadLine();
            Console.Write("Опис: ");
            string description = Console.ReadLine();
            Console.Write("Рік випуску: ");
            int releaseYear = int.Parse(Console.ReadLine());
            Console.Write("ID користувача: ");
            int userId = int.Parse(Console.ReadLine());

            movieService.AddMovie(title, description, releaseYear, userId);
            Console.ReadKey();
        }

        static void DisplayAllMovies()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ФІЛЬМІВ\n");
            var movies = movieService.GetAllMovies();
            foreach (var movie in movies)
            {
                Console.WriteLine($"ID: {movie.Id} | {movie.Title} ({movie.ReleaseYear}) | Користувач: {movie.User?.Username} | Додано: {movie.AddedDate:dd.MM.yyyy}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void SearchMovie()
        {
            Console.Clear();
            Console.WriteLine("ПОШУК ФІЛЬМУ\n");
            Console.Write("Введіть ID: ");
            int id = int.Parse(Console.ReadLine());
            var movie = movieService.GetMovieById(id);
            if (movie != null)
            {
                Console.WriteLine($"\nID: {movie.Id}");
                Console.WriteLine($"Назва: {movie.Title}");
                Console.WriteLine($"Опис: {movie.Description}");
                Console.WriteLine($"Рік випуску: {movie.ReleaseYear}");
                Console.WriteLine($"Додано користувачем: {movie.User?.Username}");
                Console.WriteLine($"Дата додавання: {movie.AddedDate:dd.MM.yyyy HH:mm}");
            }
            else
            {
                Console.WriteLine("Фільм не знайдено!");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void UpdateMovie()
        {
            Console.Clear();
            Console.WriteLine("ОНОВЛЕННЯ ФІЛЬМУ\n");
            Console.Write("ID фільму: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Нова назва: ");
            string title = Console.ReadLine();
            Console.Write("Новий опис: ");
            string description = Console.ReadLine();
            Console.Write("Новий рік випуску: ");
            int releaseYear = int.Parse(Console.ReadLine());

            movieService.UpdateMovie(id, title, description, releaseYear);
            Console.ReadKey();
        }

        static void DeleteMovie()
        {
            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ ФІЛЬМУ\n");
            Console.Write("ID фільму: ");
            int id = int.Parse(Console.ReadLine());

            movieService.DeleteMovie(id);
            Console.ReadKey();
        }

        static void DisplayUsersMoviesView()
        {
            Console.Clear();
            Console.WriteLine(" КОРИСТУВАЧІ ТА ЇХ ФІЛЬМИ (VIEW)\n");

            var result = context.Database.SqlQueryRaw<UserMovieView>(
                @"SELECT UserId, Username, Email, MovieId, Title, Description, ReleaseYear, AddedDate 
                  FROM vw_UsersMovies"
            ).ToList();

            if (result.Count > 0)
            {
                string currentUser = "";
                foreach (var item in result)
                {
                    if (currentUser != item.Username)
                    {
                        currentUser = item.Username;
                        Console.WriteLine($"\n--- Користувач: {item.Username} ({item.Email}) ---");
                    }

                    if (item.MovieId.HasValue)
                    {
                        Console.WriteLine($"  • {item.Title} ({item.ReleaseYear}) - {item.Description}");
                    }
                    else
                    {
                        Console.WriteLine("  • Немає фільмів");
                    }
                }
            }
            else
            {
                Console.WriteLine("Немає даних!");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }

    // DTO для VIEW
    public class UserMovieView
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int? MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? ReleaseYear { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}