namespace EFConsole3.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}