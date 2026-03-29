namespace BoardGamesManagement.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }

        public ICollection<Sessions> Sessions { get; set; } = new List<Sessions>();

        public override string ToString()
        {
            return $"ID: {Id} | Гра: {Title} | Жанр: {Genre} | Гравців: {MinPlayers}-{MaxPlayers}";
        }
    }
}