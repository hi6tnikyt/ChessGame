using System.ComponentModel.DataAnnotations.Schema;

namespace Chess.Models
{
    public class Game
    {
        public int Id { get; set; }

        public int WhitePlayerId { get; set; }
        public int BlackPlayerId { get; set; }

        [ForeignKey("WhitePlayerId")]
        public virtual Player WhitePlayer { get; set; } = null!;

        [ForeignKey("BlackPlayerId")]
        public virtual Player BlackPlayer { get; set; } = null!;

        public string Status { get; set; } = "Active";

        public string? Result { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Move> Moves { get; set; }
            = new List<Move>();
    }
}
