using System.Diagnostics;

namespace Legacy_Code_Homework_5._2
{
    public class Korystuvach
    {
        public string pib { get; set; }
        public string username { get; set; }

        public Korystuvach(string pib, string username)
        {
            this.pib = pib;
            this.username = username;
        }

        public override string ToString()
        {
            return $"{pib} - {username}";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Питаємо користувача де знаходиться файл зі списком користувачів
            Console.Write("Введіть шлях до файлу зі списком користувачів: ");
            string shlyakh_do_fajlu = Console.ReadLine();

            // Перевіряємо чи файл існує
            if (!File.Exists(shlyakh_do_fajlu))
            {
                Console.WriteLine("Файл не знайдено!");
                return;
            }

            // Список де будемо зберігати всіх користувачів
            List<Korystuvach> korystuvachi = new List<Korystuvach>();

            try
            {
                // Читаємо всі рядки з файлу
                string[] ryadky = File.ReadAllLines(shlyakh_do_fajlu);

                // Для кожного рядка у файлі розбираємо дані
                foreach (string ryadok in ryadky)
                {
                    // Перевіряємо чи рядок містить прочерк
                    if (ryadok.Contains("-"))
                    {
                        // Розділяємо рядок на дві частини за прочерком
                        string[] chastyny = ryadok.Split('-');

                        // Беремо першу частину (ПІБ) і другу (юзернейм)
                        // Trim() видаляє пропуски на початку і в кінці
                        string pib = chastyny[0].Trim();
                        string username = chastyny[1].Trim();

                        // Додаємо нового користувача до списку
                        korystuvachi.Add(new Korystuvach(pib, username));
                    }
                }

                Console.WriteLine($"Завантажено {korystuvachi.Count} користувачів\n");
            }
            catch (Exception osybka)
            {
                Console.WriteLine($"Помилка при читанні файлу: {osybka.Message}");
                return;
            }

            // Питаємо користувача що шукати
            Console.Write("Введіть ключове слово для пошуку (ім'я або юзернейм): ");
            string klyuchove_slovo = Console.ReadLine();

            // Якщо ключове слово порожне, то не робимо нічого
            if (string.IsNullOrEmpty(klyuchove_slovo))
            {
                Console.WriteLine("Ключове слово не введено!");
                return;
            }

            Console.WriteLine("\n ПОШУК ПОСЛІДОВНО\n");

            // ПОСЛІДОВНИЙ ПОШУК
            // Вимірюємо час виконання послідовного пошуку
            Stopwatch chasy_poslidovno = Stopwatch.StartNew();

            // LINQ запит без паралельності
            // Where перевіряє кожного користувача
            // Ми шукаємо або в імені або в юзернеймі
            // Contains шукає текст всередину рядка
            var rezultaty_poslidovno = korystuvachi
                .Where(k => k.pib.ToLower().Contains(klyuchove_slovo.ToLower()) ||
                           k.username.ToLower().Contains(klyuchove_slovo.ToLower()))
                .ToList();

            chasy_poslidovno.Stop();

            // Виводимо результати послідовного пошуку
            Console.WriteLine($"Знайдено {rezultaty_poslidovno.Count} користувачів");
            Console.WriteLine($"Час виконання: {chasy_poslidovno.ElapsedMilliseconds} ms\n");

            // Показуємо перших 10 результатів
            Console.WriteLine("Результати:");
            for (int i = 0; i < Math.Min(10, rezultaty_poslidovno.Count); i++)
            {
                Console.WriteLine($"{rezultaty_poslidovno[i]}");
            }

            if (rezultaty_poslidovno.Count > 10)
            {
                Console.WriteLine($"...і ще {rezultaty_poslidovno.Count - 10} користувачів");
            }

            Console.WriteLine("\nПОШУК ПАРАЛЕЛЬНО\n");

            // ПАРАЛЕЛЬНИЙ ПОШУК
            // Вимірюємо час виконання паралельного пошуку
            Stopwatch chasy_paralel = Stopwatch.StartNew();

            // AsParallel() перетворює LINQ запит в паралельний
            // Це означає що фільтрація буде розподілена між декількома потоками
            // Кожен потік буде обробляти свою частину списку одночасно
            var rezultaty_paralel = korystuvachi
                .AsParallel()
                .Where(k => k.pib.ToLower().Contains(klyuchove_slovo.ToLower()) ||
                           k.username.ToLower().Contains(klyuchove_slovo.ToLower()))
                .ToList();

            chasy_paralel.Stop();

            // Виводимо результати паралел��ного пошуку
            Console.WriteLine($"Знайдено {rezultaty_paralel.Count} користувачів");
            Console.WriteLine($"Час виконання: {chasy_paralel.ElapsedMilliseconds} ms\n");

            // Показуємо перших 10 результатів
            Console.WriteLine("Результати:");
            for (int i = 0; i < Math.Min(10, rezultaty_paralel.Count); i++)
            {
                Console.WriteLine($"{rezultaty_paralel[i]}");
            }

            if (rezultaty_paralel.Count > 10)
            {
                Console.WriteLine($"...і ще {rezultaty_paralel.Count - 10} користувачів");
            }

            // ПОРІВНЯННЯ
            Console.WriteLine("\nПОРІВНЯННЯ\n");

            // Вираховуємо на скільки відсотків один метод швидше за другий
            // Формула: (послідовно - паралель) / послідовно * 100
            double riznycya = ((double)(chasy_poslidovno.ElapsedMilliseconds - chasy_paralel.ElapsedMilliseconds) /
                              chasy_poslidovno.ElapsedMilliseconds) * 100;

            Console.WriteLine($"Послідовний пошук: {chasy_poslidovno.ElapsedMilliseconds} ms");
            Console.WriteLine($"Паралельний пошук: {chasy_paralel.ElapsedMilliseconds} ms");

            // Якщо паралельний пошук швидше
            if (chasy_paralel.ElapsedMilliseconds < chasy_poslidovno.ElapsedMilliseconds)
            {
                Console.WriteLine($"Паралельний пошук швидше на {(int)riznycya}%");
            }
            // Якщо послідовний пошук швидше
            else
            {
                // Math.Abs() робить число позитивним
                Console.WriteLine($"Послідовний пошук швидше на {(int)Math.Abs(riznycya)}%");
            }

            // Обидва методи повинні знайти одну й ту ж кількість користувачів
            Console.WriteLine($"\nКількість результатів однакова: {rezultaty_poslidovno.Count == rezultaty_paralel.Count}");
        }
    }
}