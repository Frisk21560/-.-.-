namespace EFConsole5.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public DateTime AddedDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}