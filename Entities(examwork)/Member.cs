namespace BoardGamesManagement.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime JoinDate { get; set; }

        public ICollection<Sessions> Sessions { get; set; } = new List<Sessions>();
        public ICollection<MemberSession> MemberSessions { get; set; } = new List<MemberSession>();

        public override string ToString()
        {
            return $"ID: {Id} | Ім'я: {FullName} | Дата вступу: {JoinDate:dd.MM.yyyy}";
        }
    }
}