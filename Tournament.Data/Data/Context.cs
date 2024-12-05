using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<TournamentDetails> TournamentDetails { get; set; } = default!;
        public DbSet<Game> Games { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TournamentDetails entity
            modelBuilder.Entity<TournamentDetails>()
                .HasMany(t => t.Games)
                .WithOne(g => g.TournamentDetails)
                .HasForeignKey(g => g.TournamentId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete ensures Games are deleted if TournamentDetails is removed

            modelBuilder.Entity<TournamentDetails>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            //// Optional: Configure Game entity if needed
            //modelBuilder.Entity<Game>()
            //    .Property(g => g.Title)
            //    .IsRequired()
            //    .HasMaxLength(100);
        }
    }
}
