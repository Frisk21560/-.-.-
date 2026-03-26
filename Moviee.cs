using EFConsole3_Homework.Entities;

namespace EFConsole3_Homework.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}