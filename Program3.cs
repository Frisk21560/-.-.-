namespace ConsoleApp3
{
    internal class WebSite
    {
        private char[] siteName = new char[50];         // Назва сайту
        private char[] siteUrl = new char[100];         // Шлях до сайту
        private char[] siteDescription = new char[200]; // Опис
        private char[] siteIp = new char[30];           // ip-адреса

        public void InputData()
        {
            Console.WriteLine("Введіть назву сайту:");
            ReadToArray(siteName);

            Console.WriteLine("Введіть шлях сайту:");
            ReadToArray(siteUrl);

            Console.WriteLine("Введіть опис сайту:");
            ReadToArray(siteDescription);

            Console.WriteLine("Введіть ip-адресу:");
            ReadToArray(siteIp);
        }

        public void PrintData()
        {
            Console.Write("Назва сайту: ");
            PrintArray(siteName);

            Console.Write("\nШлях: ");
            PrintArray(siteUrl);

            Console.Write("\nОпис: ");
            PrintArray(siteDescription);

            Console.Write("\nip-адреса: ");
            PrintArray(siteIp);
            Console.WriteLine();
        }

        // Метод для отримання назви сайту
        public void GetSiteName() { PrintArray(siteName); }
        // Метод для встановлення назви сайту
        public void SetSiteName() { ReadToArray(siteName); }

        // Метод для зчитування введення в масив символів
        private void ReadToArray(char[] arr)
        {
            int i = 0;
            int c = 0;
            while ((c = Console.Read()) != 10 && c != 13 && i < arr.Length)
            {
                arr[i++] = (char)c;
            }
            while (i < arr.Length) arr[i++] = '\0'; // Заповнення залишку нулями
            if (c != 10 && c != 13) Console.ReadLine(); // Якщо enter лишився у буфері
        }

        // Метод для виведення масиву символів
        private void PrintArray(char[] arr)
        {
            for (int i = 0; i < arr.Length && arr[i] != '\0'; i++)
            {
                Console.Write(arr[i]);
            }
        }
    }

    // Клас Журнал
    internal class Journal
    {
        private char[] journalName = new char[50];
        private int journalYear;
        private char[] journalDescription = new char[200];
        private char[] journalPhone = new char[30];
        private char[] journalEmail = new char[40];

        public void InputData()
        {
            Console.WriteLine("Введіть назву журналу:");
            ReadToArray(journalName);

            Console.WriteLine("Введіть рік заснування:");
            int year = 0;
            int.TryParse(Console.ReadLine(), out year);
            journalYear = year;

            Console.WriteLine("Введіть опис журналу:");
            ReadToArray(journalDescription);

            Console.WriteLine("Введіть телефон:");
            ReadToArray(journalPhone);

            Console.WriteLine("Введіть email:");
            ReadToArray(journalEmail);
        }

        public void PrintData()
        {
            Console.Write("Назва журналу: ");
            PrintArray(journalName);

            Console.Write("\nРік: ");
            Console.Write(journalYear);

            Console.Write("\nОпис: ");
            PrintArray(journalDescription);

            Console.Write("\nТелефон: ");
            PrintArray(journalPhone);

            Console.Write("\nEmail: ");
            PrintArray(journalEmail);
            Console.WriteLine();
        }

        // Геттери й сеттери для кожного поля через консоль
        public void GetJournalName() { PrintArray(journalName); }
        public void SetJournalName() { ReadToArray(journalName); }

        // Нижче приведено допоміжні методи для читання/друку масиву символів:
        private void ReadToArray(char[] arr)
        {
            int i = 0, c = 0;
            while ((c = Console.Read()) != 10 && c != 13 && i < arr.Length)
            {
                arr[i++] = (char)c;
            }
            while (i < arr.Length) arr[i++] = '\0';
            if (c != 10 && c != 13) Console.ReadLine();
        }
        private void PrintArray(char[] arr)
        {
            for (int i = 0; i < arr.Length && arr[i] != '\0'; i++)
            {
                Console.Write(arr[i]);
            }
        }
    }

    // Клас Магазин
    internal class Store
    {
        private char[] storeName = new char[50];
        private char[] storeAddress = new char[100];
        private char[] storeProfile = new char[150];
        private char[] storePhone = new char[30];
        private char[] storeEmail = new char[40];

        public void InputData()
        {
            Console.WriteLine("Введіть назву магазину:");
            ReadToArray(storeName);

            Console.WriteLine("Введіть адресу магазину:");
            ReadToArray(storeAddress);

            Console.WriteLine("Введіть опис профілю магазину:");
            ReadToArray(storeProfile);

            Console.WriteLine("Введіть телефон:");
            ReadToArray(storePhone);

            Console.WriteLine("Введіть email:");
            ReadToArray(storeEmail);
        }

        public void PrintData()
        {
            Console.Write("Назва магазину: ");
            PrintArray(storeName);

            Console.Write("\nАдреса: ");
            PrintArray(storeAddress);

            Console.Write("\nОпис: ");
            PrintArray(storeProfile);

            Console.Write("\nТелефон: ");
            PrintArray(storePhone);

            Console.Write("\nEmail: ");
            PrintArray(storeEmail);
            Console.WriteLine();
        }

        // Допоміжні методи зчитування/друку
        private void ReadToArray(char[] arr)
        {
            int i = 0, c = 0;
            while ((c = Console.Read()) != 10 && c != 13 && i < arr.Length)
            {
                arr[i++] = (char)c;
            }
            while (i < arr.Length) arr[i++] = '\0';
            if (c != 10 && c != 13) Console.ReadLine();
        }
        private void PrintArray(char[] arr)
        {
            for (int i = 0; i < arr.Length && arr[i] != '\0'; i++)
            {
                Console.Write(arr[i]);
            }
        }
    }

    // Головний клас для запуску программи
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1 - Веб-сайт");
            Console.WriteLine("2 - Журнал");
            Console.WriteLine("3 - Магазин");
            Console.WriteLine("Введіть номер завдання:");
            int task;
            int.TryParse(Console.ReadLine(), out task);

            if (task == 1)
            {
                // Створення і робота з об'єктом сайту
                WebSite siteA = new WebSite();
                siteA.InputData();
                Console.WriteLine();
                siteA.PrintData();
            }
            else if (task == 2)
            {
                // Створення і робота з об'єктом журналу
                Journal journalA = new Journal();
                journalA.InputData();
                Console.WriteLine();
                journalA.PrintData();
            }
            else if (task == 3)
            {
                // Створення і робота з об'єктом магазину
                Store storeA = new Store();
                storeA.InputData();
                Console.WriteLine();
                storeA.PrintData();
            }
            else
            {
                Console.WriteLine("Некоректний вибір");
            }
        }
    }
}