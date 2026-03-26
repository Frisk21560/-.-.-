using EFConsole5.Data;
using EFConsole5.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public void AddUser(string username, string email)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                CreatedDate = DateTime.Now
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            Console.WriteLine("Користувача успішно додано!");
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.Movies).ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Include(u => u.Movies).FirstOrDefault(u => u.Id == id);
        }

        public void UpdateUser(int id, string username, string email)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.Username = username;
                user.Email = email;
                _context.SaveChanges();
                Console.WriteLine("Користувача успішно оновлено!");
            }
            else
            {
                Console.WriteLine("Користувач не знайдено!");
            }
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                Console.WriteLine("Користувача успішно видалено!");
            }
            else
            {
                Console.WriteLine("Користувач не знайдено!");
            }
        }
    }
}