namespace Legacy_Code_practise_work_4
{
    public class BankAccount
    {
        // Баланс рахунку
        private decimal balans = 1000;

        // Замок для синхронізації доступу до балансу
        private object zamok = new object();

        // Метод для зняття грошей
        public void Znyattya(decimal suma)
        {
            // Використовуємо lock щоб тільки один потік міг змінювати баланс одночасно
            lock (zamok)
            {
                // Перевіряємо чи достатньо грошей на рахунку
                if (balans >= suma)
                {
                    Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} починає зняття {suma}");

                    // Імітуємо час обробки операції
                    Thread.Sleep(100);

                    // Зменшуємо балаанс
                    balans -= suma;

                    Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} зняв {suma}. Новий баланс: {balans}");
                }
                else
                {
                    Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} не може зняти {suma}. Баланс: {balans}");
                }
            }
        }

        // Метод для поповнення рахунку
        public void Popovnennya(decimal suma)
        {
            // Використовуємо lock щоб тільки один потік міг змінювати баланс одночасно
            lock (zamok)
            {
                Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} починає поповнення на {suma}");

                // Імітуємо час обробки операції
                Thread.Sleep(100);

                // Збільшуємо баланс
                balans += suma;

                Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} поповнив на {suma}. Новий баланс: {balans}");
            }
        }

        // Метод для отримання поточного балансу
        public decimal GetBalans()
        {
            lock (zamok)
            {
                return balans;
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ЗАВДАННЯ 1\n");

            // Створюємо один банківський рахунок
            BankAccount rakhunok = new BankAccount();

            // Створюємо 5 потоків які будуть робити операції з рахунком
            Thread[] potiiky = new Thread[5];

            for (int i = 0; i < potiiky.Length; i++)
            {
                potiiky[i] = new Thread(() =>
                {
                    // Кожен потік робить 3 операції
                    for (int j = 0; j < 3; j++)
                    {
                        Random rand = new Random();

                        // Випадково вибираємо операцію (зняття або поповнення)
                        if (rand.Next(0, 2) == 0)
                        {
                            // Знімаємо випадкову суму від 50 до 200
                            decimal suma = rand.Next(50, 201);
                            rakhunok.Znyattya(suma);
                        }
                        else
                        {
                            // Поповнюємо випадкову суму від 50 до 200
                            decimal suma = rand.Next(50, 201);
                            rakhunok.Popovnennya(suma);
                        }

                        // Чекаємо трохи перед наступною операцією
                        Thread.Sleep(200);
                    }
                });

                potiiky[i].Start();
            }

            // Чекаємо поки всі потоки закінчаться
            for (int i = 0; i < potiiky.Length; i++)
            {
                potiiky[i].Join();
            }

            Console.WriteLine($"\nФінальний баланс: {rakhunok.GetBalans()}\n");

            Console.WriteLine("ЗАВДАННЯ 2\n");

            // Semaphore(3, 3) означає що одночасно можуть виконуватися тільки 3 потоки
            Semaphore semaphor = new Semaphore(3, 3);

            // Створюємо 10 потоків
            Thread[] desyat_potikiv = new Thread[10];

            for (int i = 0; i < desyat_potikiv.Length; i++)
            {
                desyat_potikiv[i] = new Thread(() =>
                {
                    // WaitOne() чекає поки буде вільний слот для виконання потоку
                    semaphor.WaitOne();

                    try
                    {
                        // Виводимо ідентифікатор потоку
                        Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} починає роботу");

                        // Генеруємо і виводимо 5 випадкових чисел
                        Random rand = new Random();
                        for (int j = 0; j < 5; j++)
                        {
                            int chyslo = rand.Next(1, 101);
                            Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} вивів число: {chyslo}");

                            // Трохи чекаємо між числами
                            Thread.Sleep(300);
                        }

                        Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} завершив роботу\n");
                    }
                    finally
                    {
                        // Release() звільняє "слот" щоб наступний потік міг почати роботу
                        semaphor.Release();
                    }
                });

                desyat_potikiv[i].Start();
            }

            // Чекаємо поки всі потоки закінчаться
            for (int i = 0; i < desyat_potikiv.Length; i++)
            {
                desyat_potikiv[i].Join();
            }

            Console.WriteLine("Всі потоки завершили роботу!");
        }
    }
}