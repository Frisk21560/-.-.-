using BoardGamesManagement.Entities;
using ExamWorkDapper.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BoardGamesManagement.Data
{
    public class BoardGamesContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExamWorkDapper;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Game Configuration
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(g => g.Genre)
                    .IsRequired();
                entity.Property(g => g.MinPlayers)
                    .IsRequired();
                entity.Property(g => g.MaxPlayers)
                    .IsRequired();

                // Validation: MinPlayers > 0 and MaxPlayers > 0
                entity.HasCheckConstraint("CK_MinPlayers", "[MinPlayers] > 0");
                entity.HasCheckConstraint("CK_MaxPlayers", "[MaxPlayers] > 0");
                entity.HasCheckConstraint("CK_MinMaxPlayers", "[MinPlayers] <= [MaxPlayers]");

                // One-to-many: Game → Sessions
                entity.HasMany(g => g.Sessions)
                    .WithOne(s => s.Game)
                    .HasForeignKey(s => s.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Member Configuration
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.FullName)
                    .IsRequired();
                entity.Property(m => m.JoinDate)
                    .IsRequired()
                    .HasColumnType("datetime2");
            });

            // Session Configuration
            modelBuilder.Entity<Sessions>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Date)
                    .IsRequired()
                    .HasColumnType("datetime2");
                entity.Property(s => s.DurationMinutes)
                    .IsRequired();

                entity.HasOne(s => s.Game)
                    .WithMany(g => g.Sessions)
                    .HasForeignKey(s => s.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MemberSession Configuration (Many-to-many)
            modelBuilder.Entity<MemberSession>(entity =>
            {
                entity.HasKey(ms => ms.Id);

                entity.HasOne(ms => ms.Member)
                    .WithMany(m => m.MemberSessions)
                    .HasForeignKey(ms => ms.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ms => ms.Session)
                    .WithMany(s => s.MemberSessions)
                    .HasForeignKey(ms => ms.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(ms => new { ms.MemberId, ms.SessionId })
                    .IsUnique();
            });
        }
    }
}