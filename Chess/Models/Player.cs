namespace Chess.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public int Rating { get; set; } = 1200;

        public int Wins { get; set; } = 0;

        public int Losses { get; set; } = 0;

        public int Draws { get; set; } = 0;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Game> GamesAsWhite { get; set; }
            = new List<Game>();

        public virtual ICollection<Game> GamesAsBlack { get; set; }
            = new List<Game>();
    }
}
