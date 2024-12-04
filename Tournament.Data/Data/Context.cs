﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<TournamentDetails> TournamentDetails { get; set; } = default!;
        public DbSet<Game> Games { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentDetails>()
                .HasMany(t => t.Games)
                .WithOne()
                .HasForeignKey(g => g.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TournamentDetails>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
