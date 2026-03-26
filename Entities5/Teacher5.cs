namespace EFConsole5.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}