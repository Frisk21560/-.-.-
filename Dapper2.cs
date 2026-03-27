using Dapper;
using Dapper2.Models;
using System.Data.SqlClient;

namespace Dapper2
{
    class Program
    {
        private const string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Dapper2;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            InitBooksTable(con);

            MainMenu(con);
        }

        // ========== ІНІЦІАЛІЗАЦІЯ ТАБЛИЦІ ==========
        private static void InitBooksTable(SqlConnection con)
        {
            string query = @"
            IF OBJECT_ID(N'dbo.Books', N'U') IS NULL
            BEGIN
                CREATE TABLE Books (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Title NVARCHAR(MAX) NOT NULL,
                    Author NVARCHAR(MAX) NOT NULL
                );
            END
            ";

            con.Execute(query);
            Console.WriteLine("[LOG] Таблиця Books ініціалізована.\n");
        }

        // ========== ГОЛОВНЕ МЕНЮ ==========
        static void MainMenu(SqlConnection con)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== БІБЛІОТЕКА ===\n");
                Console.WriteLine("1. Додати книгу");
                Console.WriteLine("2. Показати всі книги");
                Console.WriteLine("3. Пошук книги");
                Console.WriteLine("4. Оновити книгу");
                Console.WriteLine("5. Видалити книгу");
                Console.WriteLine("6. Вихід");
                Console.Write("\nВиберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBookMenu(con);
                        break;
                    case "2":
                        ViewAllBooks(con);
                        break;
                    case "3":
                        SearchBookMenu(con);
                        break;
                    case "4":
                        UpdateBookMenu(con);
                        break;
                    case "5":
                        DeleteBookMenu(con);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // ========== ДОДАВАННЯ КНИГИ ==========
        static void AddBookMenu(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ДОДАВАННЯ КНИГИ ===\n");

            Console.Write("Назва книги: ");
            string title = Console.ReadLine();

            Console.Write("Автор: ");
            string author = Console.ReadLine();

            var book = new Book
            {
                Title = title,
                Author = author
            };

            AddBook(con, book);

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static void AddBook(SqlConnection con, Book book)
        {
            string query = @"
                INSERT INTO Books (Title, Author)
                VALUES (@Title, @Author);
            ";

            con.Execute(query, book);
            Console.WriteLine($"[LOG] Книга '{book.Title}' успішно додана до бібліотеки!");
        }

        // ========== ПЕРЕГЛЯД ВСІХ КНИГ ==========
        static void ViewAllBooks(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ВСІ КНИГИ ===\n");

            var books = ReadAllBooks(con);

            if (books.Count > 0)
            {
                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
                Console.WriteLine($"\nВсього книг: {books.Count}");
            }
            else
            {
                Console.WriteLine("Бібліотека порожня.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static List<Book> ReadAllBooks(SqlConnection con)
        {
            string query = @"
                SELECT Id, Title, Author FROM Books
                ORDER BY Title
            ";

            return con.Query<Book>(query).ToList();
        }

        // ========== ПОШУК КНИГИ ==========
        static void SearchBookMenu(SqlConnection con)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ПОШУК КНИГИ ===\n");
                Console.WriteLine("1. Пошук за назвою");
                Console.WriteLine("2. Пошук за автором");
                Console.WriteLine("3. Пошук за ID");
                Console.WriteLine("4. Повернутись");
                Console.Write("\nВиберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SearchBookByTitle(con);
                        break;
                    case "2":
                        SearchBookByAuthor(con);
                        break;
                    case "3":
                        SearchBookById(con);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void SearchBookByTitle(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ПОШУК ЗА НАЗВОЮ ===\n");
            Console.Write("Введіть назву (або частину): ");
            string title = Console.ReadLine();

            var books = FindBooksByTitle(con, title);

            if (books.Count > 0)
            {
                Console.WriteLine($"\nЗнайдено {books.Count} книг(и):\n");
                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("Книг з такою назвою не знайдено.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static List<Book> FindBooksByTitle(SqlConnection con, string title)
        {
            string query = @"
                SELECT Id, Title, Author FROM Books
                WHERE Title LIKE '%' + @Title + '%'
                ORDER BY Title
            ";

            return con.Query<Book>(query, new { Title = title }).ToList();
        }

        private static void SearchBookByAuthor(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ПОШУК ЗА АВТОРОМ ===\n");
            Console.Write("Введіть ім'я автора (або частину): ");
            string author = Console.ReadLine();

            var books = FindBooksByAuthor(con, author);

            if (books.Count > 0)
            {
                Console.WriteLine($"\nЗнайдено {books.Count} книг(и):\n");
                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("Книг від такого автора не знайдено.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static List<Book> FindBooksByAuthor(SqlConnection con, string author)
        {
            string query = @"
                SELECT Id, Title, Author FROM Books
                WHERE Author LIKE '%' + @Author + '%'
                ORDER BY Author
            ";

            return con.Query<Book>(query, new { Author = author }).ToList();
        }

        private static void SearchBookById(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ПОШУК ЗА ID ===\n");
            Console.Write("Введіть ID: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var book = FindBookById(con, id);

                if (book != null)
                {
                    Console.WriteLine($"\nЗнайдено:\n{book}");
                }
                else
                {
                    Console.WriteLine("Книгу з таким ID не знайдено.");
                }
            }
            else
            {
                Console.WriteLine("Невірний формат ID.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static Book FindBookById(SqlConnection con, int id)
        {
            string query = @"
                SELECT Id, Title, Author FROM Books
                WHERE Id = @Id
            ";

            return con.QueryFirstOrDefault<Book>(query, new { Id = id });
        }

        // ========== ОНОВЛЕННЯ КНИГИ ==========
        static void UpdateBookMenu(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ОНОВЛЕННЯ КНИГИ ===\n");
            Console.Write("Введіть ID книги: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var book = FindBookById(con, id);

                if (book != null)
                {
                    Console.WriteLine($"\nТекуча книга: {book}\n");

                    Console.Write("Нова назва (залишіть порожнім, щоб не змінювати): ");
                    string newTitle = Console.ReadLine();

                    Console.Write("Новий автор (залишіть порожнім, щоб не змінювати): ");
                    string newAuthor = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newTitle))
                        book.Title = newTitle;

                    if (!string.IsNullOrWhiteSpace(newAuthor))
                        book.Author = newAuthor;

                    UpdateBook(con, book);
                }
                else
                {
                    Console.WriteLine("Книгу з таким ID не знайдено.");
                }
            }
            else
            {
                Console.WriteLine("Невірний формат ID.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static void UpdateBook(SqlConnection con, Book book)
        {
            string query = @"
                UPDATE Books
                SET Title = @Title, Author = @Author
                WHERE Id = @Id
            ";

            con.Execute(query, book);
            Console.WriteLine($"[LOG] Книга успішно оновлена!");
        }

        // ========== ВИДАЛЕННЯ КНИГИ ==========
        static void DeleteBookMenu(SqlConnection con)
        {
            Console.Clear();
            Console.WriteLine("=== ВИДАЛЕННЯ КНИГИ ===\n");
            Console.Write("Введіть ID книги для видалення: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var book = FindBookById(con, id);

                if (book != null)
                {
                    Console.WriteLine($"\nКнига: {book}");
                    Console.Write("Ви впевнені? (Y/N): ");

                    if (Console.ReadLine().ToUpper() == "Y")
                    {
                        DeleteBook(con, id);
                    }
                    else
                    {
                        Console.WriteLine("Видалення скасовано.");
                    }
                }
                else
                {
                    Console.WriteLine("Книгу з таким ID не знайдено.");
                }
            }
            else
            {
                Console.WriteLine("Невірний формат ID.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        private static void DeleteBook(SqlConnection con, int id)
        {
            string query = @"
                DELETE FROM Books
                WHERE Id = @Id
            ";

            con.Execute(query, new { Id = id });
            Console.WriteLine("[LOG] Книга успішно видалена!");
        }
    }
}