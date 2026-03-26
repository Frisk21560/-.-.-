using EFConsole5.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFConsole5(homework);Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // Movie
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
                entity.Property(m => m.Description).HasMaxLength(500);
                entity.Property(m => m.ReleaseYear).IsRequired();
                entity.Property(m => m.AddedDate).HasDefaultValueSql("GETDATE()");
                entity.HasOne(m => m.User)
                    .WithMany(u => u.Movies)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}