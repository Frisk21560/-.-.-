using EFConsole3.Models;
using Microsoft.EntityFrameworkCore;

namespace EFConsole3.Data
{
    public class AcademyContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFConsole3;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Група
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name)
                    .HasMaxLength(10)
                    .IsRequired();
            });

            // Паспорт
            modelBuilder.Entity<Passport>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.PassportNumber)
                    .HasMaxLength(9)
                    .IsRequired();
            });

            // Студент
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(s => s.Email)
                    .IsRequired();
                entity.HasIndex(s => s.Email)
                    .IsUnique();
                entity.Property(s => s.Scholarship)
                    .HasColumnType("decimal(6,2)");
                entity.HasOne(s => s.Group)
                    .WithMany(g => g.Students)
                    .HasForeignKey(s => s.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(s => s.Passport)
                    .WithMany(p => p.Students)
                    .HasForeignKey(s => s.PassportId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Кафедра
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Name)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            // Предмет
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(s => s.Description)
                    .IsRequired(false);
                entity.HasOne(s => s.Department)
                    .WithMany(d => d.Subjects)
                    .HasForeignKey(s => s.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Викладач
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(t => t.Salary)
                    .HasColumnType("decimal(8,2)")
                    .HasDefaultValue(25000);
            });

            // Багато-до-багатьох: Teacher <-> Subject
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Subjects)
                .WithMany(s => s.Teachers)
                .UsingEntity(j => j.ToTable("TeacherSubjects"));
        }
    }
}