namespace EFConsole5.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}