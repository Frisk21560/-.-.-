using EFConsole2.Models2;
using Microsoft.EntityFrameworkCore;

namespace EFConsole2.Data2
{
    public class MoviesContext : DbContext
    {
        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Movies;Integrated Security=True;");
        }
    }
}