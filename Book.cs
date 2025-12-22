using System;

namespace Interfaces
{
    public class Book : IDisposable
    {
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int Year { get; set; }
        public int Pages { get; set; }

        public Book(string title, string author, int year, int pages)
        {
            Title = title;
            Author = author;
            Year = year;
            Pages = pages;
            Console.WriteLine($"Book created: {Title}");
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Title: {Title} | Author: {Author} | Year: {Year} | Pages: {Pages}");
        }

        // Finalizer (will be called by GC if Dispose not called)
        ~Book()
        {
            Console.WriteLine($"Book finalizer called for \"{Title}\"");
        }

        // Simple Dispose implementation 
        public void Dispose()
        {
            Console.WriteLine($"Book: Dispose called for \"{Title}\"");
            GC.SuppressFinalize(this); // prevent finalizer after dispose
        }
    }
}