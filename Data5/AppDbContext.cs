using EFConsole5.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFConsole5;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department
            modelBuilder.Entity<Department>().HasKey(d => d.Id);
            modelBuilder.Entity<Department>().Property(d => d.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Department>().Property(d => d.Description).HasMaxLength(500);

            // Group
            modelBuilder.Entity<Group>().HasKey(g => g.Id);
            modelBuilder.Entity<Group>().Property(g => g.Name).IsRequired().HasMaxLength(100);

            // Student
            modelBuilder.Entity<Student>().HasKey(s => s.Id);
            modelBuilder.Entity<Student>().Property(s => s.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Student>().Property(s => s.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Student>().Property(s => s.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Student>().HasIndex(s => s.Email).IsUnique();
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId);

            // Subject
            modelBuilder.Entity<Subject>().HasKey(s => s.Id);
            modelBuilder.Entity<Subject>().Property(s => s.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Subject>().Property(s => s.Description).HasMaxLength(500);
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Subjects)
                .HasForeignKey(s => s.DepartmentId);

            // Teacher
            modelBuilder.Entity<Teacher>().HasKey(t => t.Id);
            modelBuilder.Entity<Teacher>().Property(t => t.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Teacher>().Property(t => t.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Teacher>().Property(t => t.Salary).HasColumnType("decimal(8,2)");
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.DepartmentId);

            // Many-to-Many Teacher-Subject
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Subjects)
                .WithMany(s => s.Teachers)
                .UsingEntity(j => j.ToTable("TeacherSubjects"));

            // Many-to-Many Teacher-Group
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Groups)
                .WithMany(g => g.Teachers)
                .UsingEntity(j => j.ToTable("TeacherGroups"));
        }
    }
}