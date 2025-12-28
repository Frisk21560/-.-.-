using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StudentPoemsApp
{
    public class Poem
    {
        public string Title { get; set; }
        public string AuthorFullName { get; set; }
        public int Year { get; set; }
        public string Text { get; set; }
        public string Theme { get; set; }

        public Poem() { }

        public Poem(string t, string a, int y, string tx, string th)
        {
            Title = t;
            AuthorFullName = a;
            Year = y;
            Text = tx;
            Theme = th;
        }

        // length measured in words
        public int WordCount()
        {
            if (string.IsNullOrWhiteSpace(Text)) return 0;
            return Text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public override string ToString()
        {
            return $"\"{Title}\" by {AuthorFullName} ({Year}) [{Theme}] lenWords={WordCount()}";
        }
    }

    // Manager for poems: add, remove, edit, search, save/load, reports
    public class PoetryManager
    {
        private List<Poem> _list = new List<Poem>();

        // add poem
        public void AddPoem(Poem p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            _list.Add(p);
        }

        // remove by title
        public bool RemoveByTitle(string title)
        {
            var it = _list.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (it != null)
            {
                _list.Remove(it);
                return true;
            }
            return false;
        }

        // edit first poem matched by title - update fields if non-null/valid
        public bool EditPoem(string title, string newTitle = null, string newAuthor = null, int? newYear = null, string newText = null, string newTheme = null)
        {
            var it = _list.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (it == null) return false;

            if (!string.IsNullOrWhiteSpace(newTitle)) it.Title = newTitle;
            if (!string.IsNullOrWhiteSpace(newAuthor)) it.AuthorFullName = newAuthor;
            if (newYear.HasValue) it.Year = newYear.Value;
            if (!string.IsNullOrWhiteSpace(newText)) it.Text = newText;
            if (!string.IsNullOrWhiteSpace(newTheme)) it.Theme = newTheme;
            return true;
        }

        // search by title (contains)
        public List<Poem> SearchByTitle(string titlePart)
        {
            return _list.Where(x => x.Title.IndexOf(titlePart ?? "", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // search by author full name (contains)
        public List<Poem> SearchByAuthor(string authorPart)
        {
            return _list.Where(x => x.AuthorFullName.IndexOf(authorPart ?? "", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // search by theme (exact or contains)
        public List<Poem> SearchByTheme(string themePart)
        {
            return _list.Where(x => x.Theme.IndexOf(themePart ?? "", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // search by word inside text (word boundary)
        public List<Poem> SearchByWordInText(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return new List<Poem>();
            string pattern = $@"\b{Regex.Escape(word)}\b";
            return _list.Where(p => Regex.IsMatch(p.Text ?? "", pattern, RegexOptions.IgnoreCase)).ToList();
        }

        // search by year exact
        public List<Poem> SearchByYear(int year)
        {
            return _list.Where(x => x.Year == year).ToList();
        }

        // search by length (word count) - returns poems with wordcount equal or in range
        public List<Poem> SearchByLength(int minWords, int maxWords)
        {
            return _list.Where(x => x.WordCount() >= minWords && x.WordCount() <= maxWords).ToList();
        }

        // Save collection to a text file 
        // Each poem on multiple lines, fields separated by "|||"
        // We escape newlines in Text with \n so read back easier
        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                foreach (var p in _list)
                {
                    // Title|||Author|||Year|||Theme|||Text(with \n)
                    string txtEsc = (p.Text ?? "").Replace("\r", "").Replace("\n", "\\n");
                    string line = $"{EscapeField(p.Title)}|||{EscapeField(p.AuthorFullName)}|||{p.Year}|||{EscapeField(p.Theme)}|||{EscapeField(txtEsc)}";
                    sw.WriteLine(line);
                }
            }
        }

        // Load from file (overwrite current list)
        public void LoadFromFile(string path)
        {
            var newList = new List<Poem>();
            if (!File.Exists(path)) throw new FileNotFoundException("file not found", path);

            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var parts = line.Split(new[] { "|||" }, StringSplitOptions.None);
                    if (parts.Length >= 5)
                    {
                        string title = UnescapeField(parts[0]);
                        string author = UnescapeField(parts[1]);
                        int year = 0;
                        int.TryParse(parts[2], out year);
                        string theme = UnescapeField(parts[3]);
                        string textEsc = UnescapeField(parts[4]);
                        string text = textEsc.Replace("\\n", Environment.NewLine);
                        newList.Add(new Poem(title, author, year, text, theme));
                    }
                }
            }
            _list = newList;
        }

        private string EscapeField(string s)
        {
            if (s == null) return "";
            return s.Replace("|||", "||pipe||"); // simple avoid separator collisions
        }

        private string UnescapeField(string s)
        {
            if (s == null) return "";
            return s.Replace("||pipe||", "|||");
        }

        // Reports - can print to console or save to file
        public string ReportByTitle(string titlePart)
        {
            var res = SearchByTitle(titlePart);
            return BuildReport(res, $"Report by title contains \"{titlePart}\"");
        }

        public string ReportByAuthor(string authorPart)
        {
            var res = SearchByAuthor(authorPart);
            return BuildReport(res, $"Report by author contains \"{authorPart}\"");
        }

        public string ReportByTheme(string themePart)
        {
            var res = SearchByTheme(themePart);
            return BuildReport(res, $"Report by theme contains \"{themePart}\"");
        }

        public string ReportByWord(string word)
        {
            var res = SearchByWordInText(word);
            return BuildReport(res, $"Report by word in text \"{word}\"");
        }

        public string ReportByYear(int year)
        {
            var res = SearchByYear(year);
            return BuildReport(res, $"Report by year {year}");
        }

        public string ReportByLength(int minWords, int maxWords)
        {
            var res = SearchByLength(minWords, maxWords);
            return BuildReport(res, $"Report by length words between {minWords} and {maxWords}");
        }

        private string BuildReport(List<Poem> arr, string header)
        {
            var sb = new StringBuilder();
            sb.AppendLine("------- " + header + " -------");
            foreach (var p in arr)
            {
                sb.AppendLine(p.ToString());
            }
            sb.AppendLine($"Total: {arr.Count}");
            return sb.ToString();
        }

        // allow user to get internal list copy for other use
        public List<Poem> GetAll() => _list.ToList();
    }

    public static class FileHelpers
    {
        // Task 2
        public static List<string> FindFiles(string folderPath, string pattern)
        {
            if (!Directory.Exists(folderPath)) return new List<string>();
            // use SearchOption.AllDirectories
            try
            {
                var files = Directory.GetFiles(folderPath, pattern, SearchOption.AllDirectories).ToList();
                return files;
            }
            catch (Exception)
            {
                // maybe access denied - return empty for student simplicity
                return new List<string>();
            }
        }

        // Task 3: delete files by mask and delete empty subfolders
        public static void DeleteFilesByMask(string folderPath, string pattern)
        {
            if (!Directory.Exists(folderPath)) return;
            try
            {
                var files = Directory.GetFiles(folderPath, pattern, SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    try
                    {
                        File.Delete(f);
                        Console.WriteLine("Deleted file: " + f);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Can't delete file: " + f + " reason: " + ex.Message);
                    }
                }

                // delete empty directories inside folderPath
                DeleteEmptyDirectories(folderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteFilesByMask: " + ex.Message);
            }
        }

        private static void DeleteEmptyDirectories(string path)
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                DeleteEmptyDirectories(dir); // recursive
                try
                {
                    if (!Directory.EnumerateFileSystemEntries(dir).Any())
                    {
                        Directory.Delete(dir);
                        Console.WriteLine("Deleted empty dir: " + dir);
                    }
                }
                catch { /* ignore errors for student code */ }
            }
        }
    }

    public static class RegexValidators
    {
        // Task4: restaurant name - cannot contain %, &, ), (
        public static readonly string RestaurantNamePattern = @"^[^%&()]+$";

        // Task5: address - only english letters and digits (no spaces per task)
        public static readonly string AddressPattern = @"^[A-Za-z0-9]+$";

        // Task6: cuisine name - only letters english any case
        public static readonly string CuisinePattern = @"^[A-Za-z]+$";

        // Task7: rating 1..12
        public static readonly string RatingPattern = @"^([1-9]|1[0-2])$";

        // small helpers
        public static bool IsValid(string pattern, string input)
        {
            if (input == null) return false;
            return Regex.IsMatch(input, pattern);
        }
    }

    // Task8: simple restaurant review form (console) - validate with regex and save to file
    public class RestaurantReview
    {
        public string Nick { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public string Cuisine { get; set; }
        public string ContactPhone { get; set; }
        public string Rating { get; set; } // keep as string to validate with regex
        public string ReviewText { get; set; }

        // simple email regex (lowercase letters/digits/ - simplified)
        private static string EmailPattern = @"^([a-z0-9_\-\.]+)@[a-z0-9_\-]+(\.[a-z0-9_\-]+)*\.[a-z]{2,6}$";

        public List<string> Validate()
        {
            var errs = new List<string>();

            if (string.IsNullOrWhiteSpace(Nick)) errs.Add("Nick is empty");
            if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, EmailPattern, RegexOptions.IgnoreCase)) errs.Add("Email invalid");
            if (string.IsNullOrWhiteSpace(Phone)) errs.Add("Phone empty");
            if (string.IsNullOrWhiteSpace(RestaurantName) || !RegexValidators.IsValid(RegexValidators.RestaurantNamePattern, RestaurantName)) errs.Add("Restaurant name invalid (no % & ( ) allowed)");
            if (string.IsNullOrWhiteSpace(RestaurantAddress) || !RegexValidators.IsValid(RegexValidators.AddressPattern, RestaurantAddress)) errs.Add("Address invalid (only english letters and digits allowed)");
            if (string.IsNullOrWhiteSpace(Cuisine) || !RegexValidators.IsValid(RegexValidators.CuisinePattern, Cuisine)) errs.Add("Cuisine invalid (only english letters allowed)");
            if (string.IsNullOrWhiteSpace(ContactPhone)) errs.Add("Contact phone empty");
            if (string.IsNullOrWhiteSpace(Rating) || !RegexValidators.IsValid(RegexValidators.RatingPattern, Rating)) errs.Add("Rating invalid (must be 1..12)");
            if (string.IsNullOrWhiteSpace(ReviewText)) errs.Add("Review text empty");

            return errs;
        }

        // Save review to file 
        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                string line = $"{Escape(Nick)};{Escape(Email)};{Escape(Phone)};{Escape(RestaurantName)};{Escape(RestaurantAddress)};{Escape(Cuisine)};{Escape(ContactPhone)};{Escape(Rating)};{Escape(ReviewText)}";
                sw.WriteLine(line);
            }
        }

        private string Escape(string s)
        {
            if (s == null) return "";
            return s.Replace(";", "\\;");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Student project: Poems collection, file search, delete and regex validators\n");

            // Task1 - poems
            var mgr = new PoetryManager();

            // add some poems 
            mgr.AddPoem(new Poem("Morning", "Ivan Petrov", 2010, "Sun is up\nBirds sing", "nature"));
            mgr.AddPoem(new Poem("Night", "Maria Ivanova", 2005, "Moon is high\nStars are many", "nature"));
            mgr.AddPoem(new Poem("Love Song", "John Doe", 1999, "I love you so much", "love"));
            mgr.AddPoem(new Poem("Short", "Anna New", 2020, "One line", "short"));

            Console.WriteLine("All poems:");
            foreach (var ppp in mgr.GetAll()) Console.WriteLine(ppp);

            // search examples
            Console.WriteLine("\nReport by theme 'nature':");
            string rep = mgr.ReportByTheme("nature");
            Console.WriteLine(rep);

            Console.WriteLine("Report by word 'love':");
            Console.WriteLine(mgr.ReportByWord("love"));

            // save to file and load back
            string poemsFile = "poems.txt";
            mgr.SaveToFile(poemsFile);
            Console.WriteLine($"Saved poems to {poemsFile}");

            var newMgr = new PoetryManager();
            newMgr.LoadFromFile(poemsFile);
            Console.WriteLine("Loaded poems from file:");
            foreach (var p in newMgr.GetAll()) Console.WriteLine(p);

            // Task2 - find files by mask
            Console.WriteLine("\nTask2: find files demo (search current directory for *.txt):");
            var found = FileHelpers.FindFiles(".", "*.txt");
            foreach (var f in found) Console.WriteLine(" found: " + f);

            // Task3 - delete files by mask (CAUTION: this will delete if you run) - here I just show method call but I won't call destructive delete
            Console.WriteLine("\nTask3: delete files by mask demo - method is FileHelpers.DeleteFilesByMask(path, mask)");
            Console.WriteLine(" (not calling delete here to avoid accidental file loss).");

            // Task4-7 regex patterns - show them
            Console.WriteLine("\nRegex examples:");
            Console.WriteLine("RestaurantNamePattern: " + RegexValidators.RestaurantNamePattern);
            Console.WriteLine("AddressPattern: " + RegexValidators.AddressPattern);
            Console.WriteLine("CuisinePattern: " + RegexValidators.CuisinePattern);
            Console.WriteLine("RatingPattern: " + RegexValidators.RatingPattern);

            // Task8 - simple interactive review
            Console.WriteLine("\nTask8 demo - filling simple review and validating (example):");

            var review = new RestaurantReview()
            {
                Nick = "coolguy",
                Email = "test@example.com",
                Phone = "0123456789",
                RestaurantName = "GoodFood", // ok - no %&()
                RestaurantAddress = "MainStreet1", // only letters and digits
                Cuisine = "italian",
                ContactPhone = "0987654321",
                Rating = "10",
                ReviewText = "Nice place and good food!"
            };

            var errors = review.Validate();
            if (errors.Any())
            {
                Console.WriteLine("Validation failed:");
                foreach (var e in errors) Console.WriteLine(" - " + e);
            }
            else
            {
                Console.WriteLine("Validation ok, saving to reviews.txt");
                review.SaveToFile("reviews.txt");
            }

            Console.WriteLine("\nDemo finished. Press any key to exit...");
            Console.ReadKey();
        }
    }
}