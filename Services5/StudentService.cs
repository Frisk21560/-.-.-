using EFConsole5.Data;
using EFConsole5.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5.Services
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public void AddStudent(string firstName, string lastName, string email, DateTime birthDate, int groupId)
        {
            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                BirthDate = birthDate,
                GroupId = groupId
            };
            _context.Students.Add(student);
            _context.SaveChanges();
            Console.WriteLine("Студента успішно додано!");
        }

        public List<Student> GetAllStudents()
        {
            return _context.Students.Include(s => s.Group).ToList();
        }

        public Student GetStudentById(int id)
        {
            return _context.Students.Include(s => s.Group).FirstOrDefault(s => s.Id == id);
        }

        public void UpdateStudent(int id, string firstName, string lastName, string email, DateTime birthDate)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                student.FirstName = firstName;
                student.LastName = lastName;
                student.Email = email;
                student.BirthDate = birthDate;
                _context.SaveChanges();
                Console.WriteLine("Студента успішно оновлено!");
            }
            else
            {
                Console.WriteLine("Студент не знайдено!");
            }
        }

        public void DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
                Console.WriteLine("Студента успішно видалено!");
            }
            else
            {
                Console.WriteLine("Студент не знайдено!");
            }
        }

        public List<Student> GetStudentsByGroup(int groupId)
        {
            return _context.Students.Where(s => s.GroupId == groupId).ToList();
        }
    }
}