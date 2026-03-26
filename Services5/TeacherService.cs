using EFConsole5.Data;
using EFConsole5.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5.Services
{
    public class TeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        public void AddTeacher(string firstName, string lastName, DateTime birthDate, decimal salary, int departmentId)
        {
            var teacher = new Teacher
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                Salary = salary,
                DepartmentId = departmentId
            };
            _context.Teachers.Add(teacher);
            _context.SaveChanges();
            Console.WriteLine("Викладача успішно додано!");
        }

        public List<Teacher> GetAllTeachers()
        {
            return _context.Teachers.Include(t => t.Department).Include(t => t.Subjects).ToList();
        }

        public Teacher GetTeacherById(int id)
        {
            return _context.Teachers.Include(t => t.Department).Include(t => t.Subjects).FirstOrDefault(t => t.Id == id);
        }

        public void UpdateTeacher(int id, string firstName, string lastName, decimal salary)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                teacher.FirstName = firstName;
                teacher.LastName = lastName;
                teacher.Salary = salary;
                _context.SaveChanges();
                Console.WriteLine("Викладача успішно оновлено!");
            }
            else
            {
                Console.WriteLine("Викладач не знайдено!");
            }
        }

        public void DeleteTeacher(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
                Console.WriteLine("Викладача успішно видалено!");
            }
            else
            {
                Console.WriteLine("Викладач не знайдено!");
            }
        }

        public List<Teacher> GetTeachersByDepartment(int departmentId)
        {
            return _context.Teachers.Where(t => t.DepartmentId == departmentId).Include(t => t.Subjects).ToList();
        }

        public void AssignSubjectToTeacher(int teacherId, int subjectId)
        {
            var teacher = _context.Teachers.Include(t => t.Subjects).FirstOrDefault(t => t.Id == teacherId);
            var subject = _context.Subjects.Find(subjectId);

            if (teacher != null && subject != null)
            {
                if (!teacher.Subjects.Contains(subject))
                {
                    teacher.Subjects.Add(subject);
                    _context.SaveChanges();
                    Console.WriteLine("Предмет успішно призначено!");
                }
                else
                {
                    Console.WriteLine("Цей предмет вже призначено викладачу!");
                }
            }
            else
            {
                Console.WriteLine("Викладач або предмет не знайдено!");
            }
        }
    }
}