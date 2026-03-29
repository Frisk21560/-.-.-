using BoardGamesManagement.Data;
using BoardGamesManagement.Entities;

namespace BoardGamesManagement.Services
{
    public class DataSeeder
    {
        private readonly BoardGamesContext _context;
        private Random _random = new Random();

        public DataSeeder(BoardGamesContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Games.Any())
            {
                Console.WriteLine("[LOG] База даних вже заповнена!");
                return;
            }

            // Додати ігри
            var games = new List<Game>
            {
                new Game { Title = "Catan", Genre = "Strategy", MinPlayers = 2, MaxPlayers = 4 },
                new Game { Title = "Ticket to Ride", Genre = "Strategy", MinPlayers = 2, MaxPlayers = 5 },
                new Game { Title = "Carcassonne", Genre = "Tile-laying", MinPlayers = 2, MaxPlayers = 6 },
                new Game { Title = "Splendor", Genre = "Engine Building", MinPlayers = 2, MaxPlayers = 4 },
                new Game { Title = "7 Wonders", Genre = "Civilization", MinPlayers = 2, MaxPlayers = 7 }
            };

            _context.Games.AddRange(games);
            _context.SaveChanges();
            Console.WriteLine("[LOG] 5 ігор додано.");

            // Додати учасників
            var members = new List<Member>
            {
                new Member { FullName = "Анна Коваленко", JoinDate = new DateTime(2023, 1, 15) },
                new Member { FullName = "Іван Петров", JoinDate = new DateTime(2023, 3, 20) },
                new Member { FullName = "Марія Захарченко", JoinDate = new DateTime(2023, 5, 10) },
                new Member { FullName = "Дмитро Сидоренко", JoinDate = new DateTime(2023, 6, 25) },
                new Member { FullName = "Олена Морозова", JoinDate = new DateTime(2023, 8, 5) }
            };

            _context.Members.AddRange(members);
            _context.SaveChanges();
            Console.WriteLine("[LOG] 5 учасників додано.");

            // Генерувати сесії
            GenerateSessions(games, members);
        }

        private void GenerateSessions(List<Game> games, List<Member> members)
        {
            var baseDate = new DateTime(2024, 1, 1);
            var sessionsCount = 0;

            for (int i = 0; i < 15; i++)
            {
                var randomGame = games[_random.Next(games.Count)];
                var randomDate = baseDate.AddDays(_random.Next(90));
                var randomDuration = _random.Next(30, 180); // 30-180 хвилин

                var session = new Sessions
                {
                    GameId = randomGame.Id,
                    Date = randomDate,
                    DurationMinutes = randomDuration
                };

                _context.Sessions.Add(session);
                _context.SaveChanges();

                // Додати випадкову кількість учасників до сесії
                var numberOfMembers = _random.Next(randomGame.MinPlayers, randomGame.MaxPlayers + 1);
                var selectedMembers = members.OrderBy(_ => _random.Next()).Take(numberOfMembers).ToList();

                foreach (var member in selectedMembers)
                {
                    _context.MemberSessions.Add(new MemberSession
                    {
                        MemberId = member.Id,
                        SessionId = session.Id
                    });
                }

                _context.SaveChanges();
                sessionsCount++;
            }

            Console.WriteLine($"[LOG] {sessionsCount} сесій генеровано.");
        }
    }
}