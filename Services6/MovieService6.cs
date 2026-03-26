using EFConsole5.Data;
using EFConsole5.Entities;

using EFConsole5.Data;
using EFConsole5.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFConsole5.Services
{
    public class MovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public void AddMovie(string title, string description, int releaseYear, int userId)
        {
            var movie = new Movie
            {
                Title = title,
                Description = description,
                ReleaseYear = releaseYear,
                UserId = userId,
                AddedDate = DateTime.Now
            };
            _context.Movies.Add(movie);
            _context.SaveChanges();
            Console.WriteLine("Фільм успішно додано!");
        }

        public List<Movie> GetAllMovies()
        {
            return _context.Movies.Include(m => m.User).ToList();
        }

        public Movie GetMovieById(int id)
        {
            return _context.Movies.Include(m => m.User).FirstOrDefault(m => m.Id == id);
        }

        public void UpdateMovie(int id, string title, string description, int releaseYear)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                movie.Title = title;
                movie.Description = description;
                movie.ReleaseYear = releaseYear;
                _context.SaveChanges();
                Console.WriteLine("Фільм успішно оновлено!");
            }
            else
            {
                Console.WriteLine("Фільм не знайдено!");
            }
        }

        public void DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                Console.WriteLine("Фільм успішно видалено!");
            }
            else
            {
                Console.WriteLine("Фільм не знайдено!");
            }
        }

        public List<Movie> GetMoviesByUser(int userId)
        {
            return _context.Movies.Where(m => m.UserId == userId).ToList();
        }
    }
}