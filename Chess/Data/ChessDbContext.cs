using Chess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chess.Data
{
    public class ChessDbContext : IdentityDbContext
    {
        public ChessDbContext(DbContextOptions<ChessDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Move> Moves { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Game>()
            .HasOne(g => g.WhitePlayer)
            .WithMany(p => p.GamesAsWhite)
            .HasForeignKey(g => g.WhitePlayerId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(g => g.BlackPlayer)
                .WithMany(p => p.GamesAsBlack) 
                .HasForeignKey(g => g.BlackPlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
