namespace EFConsole3.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int PassportId { get; set; }
        public Passport Passport { get; set; }
        public decimal Scholarship { get; set; }
    }
}