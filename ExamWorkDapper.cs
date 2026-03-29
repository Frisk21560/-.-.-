using BoardGamesManagement.Data;
using BoardGamesManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BoardGamesContext())
            {
                // Створити БД
                context.Database.EnsureCreated();
                Console.WriteLine("[LOG] База даних готова.");

                // Сідування
                var seeder = new DataSeeder(context);
                seeder.Seed();

                // Меню
                MainMenu(context);
            }
        }

        static void MainMenu(BoardGamesContext context)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("КЛУБ НАСТІЛЬНИХ ІГОР\n");
                Console.WriteLine("1. Показати всі ігри");
                Console.WriteLine("2. Показати всіх учасників");
                Console.WriteLine("3. Показати всі сесії");
                Console.WriteLine("4. Показати сесії гри");
                Console.WriteLine("5. Показати учасників сесії");
                Console.WriteLine("6. Вихід");
                Console.Write("\nВиберіть: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllGames(context);
                        break;
                    case "2":
                        ViewAllMembers(context);
                        break;
                    case "3":
                        ViewAllSessions(context);
                        break;
                    case "4":
                        ViewSessionsByGame(context);
                        break;
                    case "5":
                        ViewMembersBySession(context);
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

        static void ViewAllGames(BoardGamesContext context)
        {
            Console.Clear();
            Console.WriteLine("ВСІ ІГ РИ\n");

            var games = context.Games.Include(g => g.Sessions).ToList();

            foreach (var game in games)
            {
                Console.WriteLine($"{game} | Сесій: {game.Sessions.Count}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ViewAllMembers(BoardGamesContext context)
        {
            Console.Clear();
            Console.WriteLine("ВСІ УЧАСНИКИ\n");

            var members = context.Members.Include(m => m.MemberSessions).ToList();

            foreach (var member in members)
            {
                Console.WriteLine($"{member} | Сесій: {member.MemberSessions.Count}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ViewAllSessions(BoardGamesContext context)
        {
            Console.Clear();
            Console.WriteLine("ВСІ СЕСІЇ\n");

            var sessions = context.Sessions
                .Include(s => s.Game)
                .Include(s => s.MemberSessions)
                .ThenInclude(ms => ms.Member)
                .OrderByDescending(s => s.Date)
                .ToList();

            foreach (var session in sessions)
            {
                var members = string.Join(", ", session.MemberSessions.Select(ms => ms.Member.FullName));
                Console.WriteLine($"{session}");
                Console.WriteLine($"   Учасники: {members}\n");
            }

            Console.WriteLine("Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ViewSessionsByGame(BoardGamesContext context)
        {
            Console.Clear();
            Console.WriteLine("СЕСІЇ ГРАВИЦІ\n");

            var games = context.Games.ToList();
            Console.WriteLine("Доступні ігри:");
            foreach (var game in games)
            {
                Console.WriteLine($"{game.Id}. {game.Title}");
            }

            Console.Write("\nВиберіть ID гри: ");
            if (int.TryParse(Console.ReadLine(), out int gameId))
            {
                var sessions = context.Sessions
                    .Where(s => s.GameId == gameId)
                    .Include(s => s.Game)
                    .Include(s => s.MemberSessions)
                    .ThenInclude(ms => ms.Member)
                    .ToList();

                if (sessions.Count > 0)
                {
                    Console.WriteLine($"\nСесії гри:\n");
                    foreach (var session in sessions)
                    {
                        var members = string.Join(", ", session.MemberSessions.Select(ms => ms.Member.FullName));
                        Console.WriteLine($"{session}");
                        Console.WriteLine($"   Учасники: {members}\n");
                    }
                }
                else
                {
                    Console.WriteLine("Сесій не знайдено!");
                }
            }

            Console.WriteLine("Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ViewMembersBySession(BoardGamesContext context)
        {
            Console.Clear();
            Console.WriteLine("УЧАСНИКИ СЕСІЇ\n");

            var sessions = context.Sessions
                .Include(s => s.Game)
                .OrderByDescending(s => s.Date)
                .ToList();

            Console.WriteLine("Доступні сесії:");
            foreach (var session in sessions)
            {
                Console.WriteLine($"{session.Id}. {session.Game.Title} - {session.Date:dd.MM.yyyy HH:mm}");
            }

            Console.Write("\nВиберіть ID сесії: ");
            if (int.TryParse(Console.ReadLine(), out int sessionId))
            {
                var members = context.MemberSessions
                    .Where(ms => ms.SessionId == sessionId)
                    .Include(ms => ms.Member)
                    .Select(ms => ms.Member)
                    .ToList();

                if (members.Count > 0)
                {
                    Console.WriteLine($"\nУчасники:\n");
                    foreach (var member in members)
                    {
                        Console.WriteLine($"• {member.FullName}");
                    }
                }
                else
                {
                    Console.WriteLine("Учасників не знайдено!");
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}