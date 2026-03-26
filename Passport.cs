namespace EFConsole3.Models
{
    public class Passport
    {
        public int Id { get; set; }
        public string PassportNumber { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}