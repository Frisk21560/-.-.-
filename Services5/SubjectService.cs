using EFConsole5.Data;
using EFConsole5.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5.Services
{
    public class SubjectService
    {
        private readonly AppDbContext _context;

        public SubjectService(AppDbContext context)
        {
            _context = context;
        }

        public void AddSubject(string name, string description, int departmentId)
        {
            var subject = new Subject
            {
                Name = name,
                Description = description,
                DepartmentId = departmentId
            };
            _context.Subjects.Add(subject);
            _context.SaveChanges();
            Console.WriteLine("Предмет успішно додано!");
        }

        public List<Subject> GetAllSubjects()
        {
            return _context.Subjects.Include(s => s.Department).Include(s => s.Teachers).ToList();
        }

        public Subject GetSubjectById(int id)
        {
            return _context.Subjects.Include(s => s.Department).Include(s => s.Teachers).FirstOrDefault(s => s.Id == id);
        }

        public void UpdateSubject(int id, string name, string description)
        {
            var subject = _context.Subjects.Find(id);
            if (subject != null)
            {
                subject.Name = name;
                subject.Description = description;
                _context.SaveChanges();
                Console.WriteLine("Предмет успішно оновлено!");
            }
            else
            {
                Console.WriteLine("Предмет не знайдено!");
            }
        }

        public void DeleteSubject(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                _context.SaveChanges();
                Console.WriteLine("Предмет успішно видалено!");
            }
            else
            {
                Console.WriteLine("Предмет не знайдено!");
            }
        }

        public List<Subject> GetSubjectsByDepartment(int departmentId)
        {
            return _context.Subjects.Where(s => s.DepartmentId == departmentId).ToList();
        }
    }
}