using EF_Core_Lesson.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Lesson.Data
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=School_EF_Core_DB;Integrated Security=True;");
        }
    }
}