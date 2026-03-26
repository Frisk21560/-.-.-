using EFConsole3_Homework.Models;
using Microsoft.EntityFrameworkCore;

namespace EFConsole3_Homework.Data
{
    public class PlatformContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFConsole3_Homework;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Користувач
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username)
                    .IsRequired();
                entity.HasIndex(u => u.Username)
                    .IsUnique();
                entity.Property(u => u.Email)
                    .IsRequired();
                entity.HasIndex(u => u.Email)
                    .IsUnique();
                entity.Property(u => u.Password)
                    .IsRequired();
                entity.HasMany(u => u.Movies)
                    .WithOne(m => m.User)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Фільм
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Title)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(m => m.ReleaseYear)
                    .IsRequired();
                entity.HasCheckConstraint("CK_Movie_ReleaseYear", "[ReleaseYear] > 0");
                entity.Property(m => m.Description)
                    .IsRequired(false);
                entity.Property(m => m.DateAdded)
                    .HasDefaultValueSql("GETDATE()");
                entity.HasOne(m => m.User)
                    .WithMany(u => u.Movies)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}