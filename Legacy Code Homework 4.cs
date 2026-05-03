using System.Diagnostics;

namespace Legacy_Code_Homework_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Запраховуємо час коли починається програма щоб пізніше розрахувати скільки всього тривало
            Stopwatch chasy = Stopwatch.StartNew();

            // Питаємо користувача якої папку шукати
            Console.WriteLine("Введіть шлях до папки:");
            string papka = Console.ReadLine();

            // Перевіряємо чи існує папка, якщо не існує то завершуємо програм
            if (!Directory.Exists(papka))
            {
                Console.WriteLine("Папка не знайдено!");
                return;
            }

            // Знаходимо усі .txt файли у папці, SearchOption.AllDirectories означає що шукати і у підпапках
            string[] fajly = Directory.GetFiles(papka, "*.txt", SearchOption.AllDirectories);

            // Якщо файлів не знайшли, то виводимо повідомлення і завершуємо
            if (fajly.Length == 0)
            {
                Console.WriteLine("Txt файлів не знайдено!");
                return;
            }

            Console.WriteLine($"Знайдено {fajly.Length} файлів. Починається шифрування...\r\n");

            // Для кожного файлу запускаємо шифрування у threadi
            foreach (string fajl in fajly)
            {
                // ThreadPool.QueueUserWorkItem запускає метод у фоновому threadi, не блокує основний thread
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    try
                    {
                        // Виводимо у консол що почали шифрувати цей файл
                        Console.WriteLine($"Початий шифрування: {Path.GetFileName(fajl)}");

                        // Читаємо зміст файлу
                        string zmist = File.ReadAllText(fajl);

                        // Шифруватися, в цьому випадку просто замінюємо букви на інші (проста шифр)
                        string zshyfrovanyy = ShyfrUvannyaBazhko(zmist);

                        // Ім'я нового файлу, додаємо до його назви "_зашифрований"
                        string noviyfajl = fajl.Replace(".txt", "_zashyfrovanyy.txt");

                        // Пишемо зашифрований зміст у новий файл, File.WriteAllText робить це
                        File.WriteAllText(noviyfajl, zshyfrovanyy);

                        // Виводимо у консол що шифрування закінчилось
                        Console.WriteLine($"Закінчене шифрування: {Path.GetFileName(fajl)}");
                    }
                    catch (Exception osybka)
                    {
                        Console.WriteLine($"Помилка при шифруванніі {fajl}: {osybka.Message}");
                    }
                });
            }

            // Чекаємо трохи, щоб дати часу на виконання усім задач у threadi
            System.Threading.Thread.Sleep(5000);

            // Зупиняємо відліки часу
            chasy.Stop();

            // Виводимо статистику
            Console.WriteLine($"\r\nСтатистика:");
            Console.WriteLine($"Всього файлів обраховано: {fajly.Length}");
            Console.WriteLine($"Часу витрачено: {chasy.ElapsedMilliseconds} ms");
        }

        // Метод для шифрування, в цьому випадку просто замінюємо букви
        static string ShyfrUvannyaBazhko(string tekst)
        {
            string shifrovanyy = "";

            // Проходимо по кожному симвОлу в тексті
            foreach (char simvol in tekst)
            {
                // ЯКщо це букви, замінюємо її на іншу (в цьому випадку змішуємо на 3 позиції)
                if (char.IsLetter(simvol))
                {
                    shifrovanyy += (char)(simvol + 3);
                }
                else
                {
                    // Якщо не букви то залишаємо як є
                    shifrovanyy += simvol;
                }
            }

            return shifrovanyy;
        }
    }
}