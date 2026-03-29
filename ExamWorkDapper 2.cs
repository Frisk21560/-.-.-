using System;
using BoardGamesAnalytics.DataAccess;
using BoardGamesAnalytics.Models;

namespace BoardGamesAnalytics
{
    class Program
    {
        private const string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ExamWorkDapper;Integrated Security=True;";
        private static AnalyticsRepository _repository;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            _repository = new AnalyticsRepository(_connectionString);
            MainMenu();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== АНАЛІТИКА КЛУБУ НАСТІЛЬНИХ ІГОР ===\n");
                Console.WriteLine("1. Показати всі сесії");
                Console.WriteLine("2. Топ-3 ігри за годинами");
                Console.WriteLine("3. Рейтинг учасників");
                Console.WriteLine("4. Загальна статистика");
                Console.WriteLine("5. Статистика за період");
                Console.WriteLine("6. Вихід");
                Console.Write("\nВиберіть: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllSessions();
                        break;
                    case "2":
                        ShowTop3Games();
                        break;
                    case "3":
                        ShowMembersRating();
                        break;
                    case "4":
                        ShowGeneralStats();
                        break;
                    case "5":
                        ShowStatsByPeriod();
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

        static void ShowAllSessions()
        {
            Console.Clear();
            Console.WriteLine("=== ВСІ СЕСІЇ ===\n");

            var sessions = _repository.GetAllSessions();

            if (sessions.Count == 0)
            {
                Console.WriteLine("Сесій не знайдено.");
            }
            else
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-15} {4,-30}", "ID", "Гра", "Дата", "Тривалість (хв)", "Учасники");
                Console.WriteLine(new string('-', 120));

                foreach (var session in sessions)
                {
                    Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-15} {4,-30}",
                        session.Id,
                        session.GameTitle,
                        ((DateTime)session.Date).ToString("dd.MM.yyyy HH:mm"),
                        session.DurationMinutes,
                        session.Members ?? "немає"
                    );
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ShowTop3Games()
        {
            Console.Clear();
            Console.WriteLine("=== ТОП-3 ІГРИ ЗА КІЛЬКІСТЮ ГОДИН ===\n");

            var games = _repository.GetTop3Games();

            if (games.Count == 0)
            {
                Console.WriteLine("Ігор не знайдено.");
            }
            else
            {
                Console.WriteLine("{0,-3} {1,-30} {2,-15}", "№", "Назва гри", "Годин");
                Console.WriteLine(new string('-', 50));

                int position = 1;
                foreach (var game in games)
                {
                    Console.WriteLine("{0,-3} {1,-30} {2,-15}",
                        position++,
                        game.Title,
                        game.TotalHours
                    );
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ShowMembersRating()
        {
            Console.Clear();
            Console.WriteLine("=== РЕЙТИНГ УЧАСНИКІВ ===\n");

            var members = _repository.GetMembersRating();

            if (members.Count == 0)
            {
                Console.WriteLine("Учасників не знайдено.");
            }
            else
            {
                Console.WriteLine("{0,-3} {1,-30} {2,-20}", "№", "Ім'я учасника", "Хвилин");
                Console.WriteLine(new string('-', 55));

                int position = 1;
                foreach (var member in members)
                {
                    Console.WriteLine("{0,-3} {1,-30} {2,-20}",
                        position++,
                        member.FullName,
                        member.TotalMinutes
                    );
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ShowGeneralStats()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАГАЛЬНА СТАТИСТИКА ===\n");

            var stats = _repository.GetGeneralStats();

            if (stats != null)
            {
                Console.WriteLine($"Всього сесій: {stats.TotalSessions}");
                Console.WriteLine($"Загальна тривалість: {stats.TotalMinutes} хвилин ({stats.TotalMinutes / 60} годин {stats.TotalMinutes % 60} хвилин)");
            }
            else
            {
                Console.WriteLine("Статистика не доступна.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ShowStatsByPeriod()
        {
            Console.Clear();
            Console.WriteLine("=== СТАТИСТИКА ЗА ПЕРІОД ===\n");

            Console.Write("Введіть дату початку (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("Невірний формат дати.");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть дату завершення (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("Невірний формат дати.");
                Console.ReadKey();
                return;
            }

            var stats = _repository.GetGeneralStatsByPeriod(startDate, endDate);

            if (stats != null && stats.TotalSessions > 0)
            {
                Console.WriteLine($"\nПеріод: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
                Console.WriteLine($"Сесій у цьому періоді: {stats.TotalSessions}");
                Console.WriteLine($"Загальна тривалість: {stats.TotalMinutes} хвилин ({stats.TotalMinutes / 60} годин {stats.TotalMinutes % 60} хвилин)");
            }
            else
            {
                Console.WriteLine($"У період {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy} сесій не знайдено.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}