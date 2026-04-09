using System.ComponentModel.DataAnnotations;

namespace Chess.Models
{
    public class Move
    {
        [Key]
        public int Id { get; set; }

        public int GameId { get; set; }

        public string MoveText { get; set; } = null!;

        public int MoveNumber { get; set; }

        public DateTime MoveTime { get; set; } = DateTime.UtcNow;

        public Game Game { get; set; } = null!;
    }
}
