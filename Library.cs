
using System;

namespace Interfaces
{
    public class Library : IDisposable
    {
        private Book[] _books = new Book[0];

        public Library()
        {
            Console.WriteLine("Library created");
        }

        public void AddBook(Book b)
        {
            Array.Resize(ref _books, _books.Length + 1);
            _books[_books.Length - 1] = b;
            Console.WriteLine($"Library: book added - \"{b.Title}\"");
        }

        public void ShowAllBooks()
        {
            Console.WriteLine("Library: show all books:");
            if (_books == null || _books.Length == 0)
            {
                Console.WriteLine(" Library empty");
                return;
            }

            for (int i = 0; i < _books.Length; i++)
            {
                var bk = _books[i];
                if (bk != null)
                    bk.DisplayInfo();
            }
        }

        // Dispose for library: free resources, dispose inner books
        public void Dispose()
        {
            Console.WriteLine("Library: Dispose called, freeing books...");
            if (_books != null)
            {
                for (int i = 0; i < _books.Length; i++)
                {
                    try
                    {
                        _books[i]?.Dispose();
                    }
                    catch
                    {
                        // just ignore small errors like a student would
                    }
                }
                _books = null!;
            }
            GC.SuppressFinalize(this);
        }
    }
}