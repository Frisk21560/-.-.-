using System.Text;

namespace Legacy_Code_Exam_Work
{
    class Program
    {
        // Флаг для вбиття однієї копії програми
        private static Mutex mutex = new Mutex(false, "ForbiddenWordsApp_SingleInstance");

        static void Main(string[] args)
        {
            // Перевіряємо чи вже запущена копія програми
            if (!mutex.WaitOne(TimeSpan.Zero, false))
            {
                Console.WriteLine("Програма вже запущена");
                return;
            }

            try
            {
                // Перевіряємо аргументи командного рядка
                if (args.Length > 0 && args[0] == "-cli")
                {
                    // Режим командного рядка без інтерфейсу
                    RunConsoleMode();
                }
                else
                {
                    // Інтерактивний режим
                    RunInteractiveMode();
                }
            }
            finally
            {
                // Вивільняємо мютекс
                mutex.ReleaseMutex();
            }
        }

        private static void RunConsoleMode()
        {
            Console.WriteLine("Режим командного рядка\n");

            Console.Write("Ведіть шлях до файлу зі забороненними словами: ");
            string wordsFilePath = Console.ReadLine();

            if (!File.Exists(wordsFilePath))
            {
                Console.WriteLine("Файл не знайдено");
                return;
            }

            Console.Write("Введіть папку для збереження результатів: ");
            string outputFolder = Console.ReadLine();

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Читаємо заборонені слова
            var forbiddenWords = File.ReadAllLines(wordsFilePath).ToList();
            Console.WriteLine($"Завантажено {forbiddenWords.Count} заборонених слівв\n");

            // Запускаємо пошук
            var searcher = new ForbiddenWordsSearcher(forbiddenWords, outputFolder);
            searcher.StartSearch();

            Console.WriteLine("\nПошук завершено");
        }

        private static void RunInteractiveMode()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Пошук заборонених слів\n");

            List<string> forbiddenWords = new List<string>();

            // Меню вибору джерела слів
            Console.WriteLine("1. Ввести слова вручну");
            Console.WriteLine("2. Завантажити з файлу");
            Console.Write("Виберіть опцію: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("Введіть заборонені слова (кожне з нового рядка, порожній рядок для завершення):");
                while (true)
                {
                    string word = Console.ReadLine();
                    if (string.IsNullOrEmpty(word)) break;
                    forbiddenWords.Add(word.ToLower());
                }
            }
            else if (choice == "2")
            {
                Console.Write("Введіть шлях до файлу: ");
                string filePath = Console.ReadLine();
                if (File.Exists(filePath))
                {
                    forbiddenWords = File.ReadAllLines(filePath)
                        .Select(w => w.ToLower())
                        .ToList();
                    Console.WriteLine($"Завантажено {forbiddenWords.Count} слів");
                }
                else
                {
                    Console.WriteLine("Файл не знайдено");
                    return;
                }
            }

            if (forbiddenWords.Count == 0)
            {
                Console.WriteLine("Немає заборонених слів");
                return;
            }

            Console.Write("\nВведіть папку для результатів: ");
            string outputFolder = Console.ReadLine();

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Запускаємо пошук з інтерактивним управлінням
            var searcher = new ForbiddenWordsSearcher(forbiddenWords, outputFolder);
            RunSearchWithControls(searcher);
        }

        private static void RunSearchWithControls(ForbiddenWordsSearcher searcher)
        {
            // Запускаємо пошук у окремому потоці
            var searchTask = Task.Run(() => searcher.StartSearch());

            Console.WriteLine("\nПошук почався...");
            Console.WriteLine("Натисніть P для паузи, R для продовження, S для зупинки");

            while (!searchTask.IsCompleted)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    switch (char.ToUpper(key.KeyChar))
                    {
                        case 'P':
                            searcher.Pause();
                            Console.WriteLine("Пауза...");
                            break;
                        case 'R':
                            searcher.Resume();
                            Console.WriteLine("Продовження...");
                            break;
                        case 'S':
                            searcher.Stop();
                            Console.WriteLine("Зупинка...");
                            break;
                    }
                }

                // Виводимо прогрес
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Прогрес: {searcher.GetProgress()}% | Знайдено файлів: {searcher.GetFoundFilesCount()}  ");

                Thread.Sleep(100);
            }

            searchTask.Wait();

            Console.WriteLine("\n\nРезультати:");
            Console.WriteLine($"Всього знайдено файлів: {searcher.GetFoundFilesCount()}");
            Console.WriteLine($"Загалом замін: {searcher.GetTotalReplacements()}");
            Console.WriteLine($"Файл звіту: {searcher.GetReportPath()}");

            // Виводимо топ-10 слів
            var topWords = searcher.GetTop10Words();
            Console.WriteLine("\nТоп-10 заборонених слів:");
            int i = 1;
            foreach (var word in topWords)
            {
                Console.WriteLine($"{i}. {word.Key} - {word.Value} разів");
                i++;
            }
        }
    }

    // Клас для виконання пошуку заборонених слів
    public class ForbiddenWordsSearcher
    {
        private List<string> forbiddenWords;
        private string outputFolder;

        // Для синхронізації потоків
        private object lockObj = new object();
        private ManualResetEvent pauseEvent = new ManualResetEvent(true);
        private CancellationTokenSource cancellationTokenSource;

        // Статистика
        private int foundFilesCount = 0;
        private int totalReplacements = 0;
        private Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
        private List<FileSearchResult> results = new List<FileSearchResult>();

        // Прогрес
        private int totalFilesScanned = 0;
        private int totalDrivesCount = 0;

        public ForbiddenWordsSearcher(List<string> words, string output)
        {
            forbiddenWords = words;
            outputFolder = output;
            cancellationTokenSource = new CancellationTokenSource();

            // Ініціалізуємо словник частоти
            foreach (var word in forbiddenWords)
            {
                wordFrequency[word] = 0;
            }
        }

        public void StartSearch()
        {
            try
            {
                // Отримуємо всі диски
                var drives = DriveInfo.GetDrives().Where(d => d.IsReady).ToList();
                totalDrivesCount = drives.Count;

                var searchTasks = new List<Task>();

                // Запускаємо пошук на кожному диску у окремому потоці
                foreach (var drive in drives)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    var task = Task.Run(() => SearchInDrive(drive), cancellationTokenSource.Token);
                    searchTasks.Add(task);
                }

                // Чекаємо завершення всіх задач
                Task.WaitAll(searchTasks.ToArray(), cancellationTokenSource.Token);

                // Генеруємо звіт
                GenerateReport();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Пошук скасовано");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        private void SearchInDrive(DriveInfo drive)
        {
            try
            {
                SearchInDirectory(drive.RootDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                // Ігноруємо папки без доступу
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при скануванні диска {drive.Name}: {ex.Message}");
            }
        }

        private void SearchInDirectory(DirectoryInfo directory)
        {
            try
            {
                // Перевіряємо токен для скасування
                pauseEvent.WaitOne();
                if (cancellationTokenSource.Token.IsCancellationRequested)
                    return;

                // Сканимо файли в папці
                var files = directory.GetFiles();
                foreach (var file in files)
                {
                    pauseEvent.WaitOne();
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                        return;

                    SearchInFile(file);

                    lock (lockObj)
                    {
                        totalFilesScanned++;
                    }
                }

                // Рекурсивно сканимо підпапки
                var subDirectories = directory.GetDirectories();
                foreach (var subDir in subDirectories)
                {
                    SearchInDirectory(subDir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ігноруємо папки без доступу
            }
            catch (Exception ex)
            {
                // Просто пропускаємо помилки при скануванні
            }
        }

        private void SearchInFile(FileInfo file)
        {
            try
            {
                // Пропускаємо системні файли та великі файли
                if (file.Length > 100 * 1024 * 1024) // Більше ніж 100 МБ
                    return;

                // Спробуємо прочитати файл як текст
                string content = File.ReadAllText(file.FullName, Encoding.UTF8);
                bool foundForbiddenWord = false;
                var replacements = new Dictionary<string, int>();

                // Шукаємо заборонені слова
                foreach (var forbiddenWord in forbiddenWords)
                {
                    // Чутливо до регістру для вирішення
                    if (content.IndexOf(forbiddenWord, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        foundForbiddenWord = true;

                        lock (lockObj)
                        {
                            // Підраховуємо кількість знайдених слів
                            int count = CountOccurrences(content, forbiddenWord);
                            wordFrequency[forbiddenWord] += count;

                            if (replacements.ContainsKey(forbiddenWord))
                                replacements[forbiddenWord] += count;
                            else
                                replacements[forbiddenWord] = count;
                        }
                    }
                }

                // Якщо знайдені заборонені слова
                if (foundForbiddenWord)
                {
                    lock (lockObj)
                    {
                        foundFilesCount++;

                        // Підраховуємо всі замін
                        foreach (var count in replacements.Values)
                        {
                            totalReplacements += count;
                        }

                        // Зберігаємо результат
                        results.Add(new FileSearchResult
                        {
                            FilePath = file.FullName,
                            FileSize = file.Length,
                            FoundWords = replacements
                        });
                    }

                    // Копіюємо оригінальний файл
                    CopyOriginalFile(file);

                    // Створюємо файл із заміненими словами
                    CreateCensoredFile(file, content);
                }
            }
            catch (Exception ex)
            {
                // Якщо не можемо прочитати файл - пропускаємо
            }
        }

        private int CountOccurrences(string text, string word)
        {
            int count = 0;
            int index = 0;
            while ((index = text.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index += word.Length;
            }
            return count;
        }

        private void CopyOriginalFile(FileInfo file)
        {
            try
            {
                string destinationPath = Path.Combine(outputFolder, $"original_{Path.GetFileName(file.FullName)}");
                File.Copy(file.FullName, destinationPath, true);
            }
            catch
            {
                // Ігноруємо помилки копіювання
            }
        }

        private void CreateCensoredFile(FileInfo file, string content)
        {
            try
            {
                string censoredContent = content;

                // Замінюємо всі заборонені слова на зірочки
                foreach (var forbiddenWord in forbiddenWords)
                {
                    var regex = new System.Text.RegularExpressions.Regex(
                        System.Text.RegularExpressions.Regex.Escape(forbiddenWord),
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    );
                    censoredContent = regex.Replace(censoredContent, "*******");
                }

                string censoredPath = Path.Combine(outputFolder, $"censored_{Path.GetFileName(file.FullName)}");
                File.WriteAllText(censoredPath, censoredContent, Encoding.UTF8);
            }
            catch
            {
                // Ігноруємо помилки
            }
        }

        private void GenerateReport()
        {
            try
            {
                string reportPath = Path.Combine(outputFolder, "report.txt");

                using (var writer = new StreamWriter(reportPath, false, Encoding.UTF8))
                {
                    writer.WriteLine("ЗВІТ ПРО ЗНАЙДЕНІ ЗАБОРОНЕНІ СЛОВА\n");
                    writer.WriteLine($"Дата звіту: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"Всього знайдено файлів: {foundFilesCount}");
                    writer.WriteLine($"Всього замін: {totalReplacements}");
                    writer.WriteLine($"Всього сканованих файлів: {totalFilesScanned}\n");

                    writer.WriteLine("ЗНАЙДЕНІ ФАЙЛИ\n");
                    foreach (var result in results)
                    {
                        writer.WriteLine($"Файл: {result.FilePath}");
                        writer.WriteLine($"Розмір: {result.FileSize} байт");
                        writer.WriteLine("Знайдені слова:");
                        foreach (var word in result.FoundWords)
                        {
                            writer.WriteLine($"  - {word.Key}: {word.Value} разів");
                        }
                        writer.WriteLine();
                    }

                    writer.WriteLine("\nТОП-10 ЗАБОРОНЕНИХ СЛІВ\n");
                    var topWords = wordFrequency
                        .Where(w => w.Value > 0)
                        .OrderByDescending(w => w.Value)
                        .Take(10)
                        .ToList();

                    int i = 1;
                    foreach (var word in topWords)
                    {
                        writer.WriteLine($"{i}. {word.Key} - {word.Value} разів");
                        i++;
                    }
                }

                Console.WriteLine($"Звіт збережено: {reportPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при генеруванні звіту: {ex.Message}");
            }
        }

        public void Pause()
        {
            pauseEvent.Reset();
        }

        public void Resume()
        {
            pauseEvent.Set();
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        public int GetProgress()
        {
            if (totalDrivesCount == 0) return 0;
            return (int)((totalFilesScanned / (float)(totalFilesScanned + 1000)) * 100);
        }

        public int GetFoundFilesCount()
        {
            lock (lockObj)
            {
                return foundFilesCount;
            }
        }

        public int GetTotalReplacements()
        {
            lock (lockObj)
            {
                return totalReplacements;
            }
        }

        public string GetReportPath()
        {
            return Path.Combine(outputFolder, "report.txt");
        }

        public List<KeyValuePair<string, int>> GetTop10Words()
        {
            lock (lockObj)
            {
                return wordFrequency
                    .Where(w => w.Value > 0)
                    .OrderByDescending(w => w.Value)
                    .Take(10)
                    .ToList();
            }
        }
    }

    // Клас для зберігання результатів пошуку в файлі
    public class FileSearchResult
    {
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public Dictionary<string, int> FoundWords { get; set; }
    }
}