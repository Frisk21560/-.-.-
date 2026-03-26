namespace EFConsole5.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}