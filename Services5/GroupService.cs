using EFConsole5.Data;
using EFConsole5.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5.Services
{
    public class GroupService
    {
        private readonly AppDbContext _context;

        public GroupService(AppDbContext context)
        {
            _context = context;
        }

        public void AddGroup(string name)
        {
            var group = new Group { Name = name };
            _context.Groups.Add(group);
            _context.SaveChanges();
            Console.WriteLine("Групу успішно додано!");
        }

        public List<Group> GetAllGroups()
        {
            return _context.Groups.Include(g => g.Students).Include(g => g.Teachers).ToList();
        }

        public Group GetGroupById(int id)
        {
            return _context.Groups.Include(g => g.Students).Include(g => g.Teachers).FirstOrDefault(g => g.Id == id);
        }

        public void UpdateGroup(int id, string name)
        {
            var group = _context.Groups.Find(id);
            if (group != null)
            {
                group.Name = name;
                _context.SaveChanges();
                Console.WriteLine("Групу успішно оновлено!");
            }
            else
            {
                Console.WriteLine("Група не знайдена!");
            }
        }

        public void DeleteGroup(int id)
        {
            var group = _context.Groups.Find(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                _context.SaveChanges();
                Console.WriteLine("Групу успішно видалено!");
            }
            else
            {
                Console.WriteLine("Група не знайдена!");
            }
        }
    }
}