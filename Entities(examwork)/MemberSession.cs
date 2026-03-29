using ExamWorkDapper.Entities;

namespace BoardGamesManagement.Entities
{
    public class MemberSession
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int SessionId { get; set; }

        public Member Member { get; set; } = null!;
        public Sessions Session { get; set; } = null!;
    }
}