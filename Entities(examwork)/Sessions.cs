using ExamWorkDapper.Entities;

namespace BoardGamesManagement.Entities
{
    public class Sessions
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public DateTime Date { get; set; }
        public int DurationMinutes { get; set; }

        public Game Game { get; set; } = null!;
        public ICollection<Member> Members { get; set; } = new List<Member>();
        public ICollection<MemberSession> MemberSessions { get; set; } = new List<MemberSession>();

        public override string ToString()
        {
            return $"ID: {Id} | Гра: {Game?.Title} | Дата: {Date:dd.MM.yyyy HH:mm} | Тривалість: {DurationMinutes} хв";
        }
    }
}