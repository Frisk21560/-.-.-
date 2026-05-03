namespace Legacy_Code_Homework_4
{
    public class Dani
    {
        public static List<int> chysla = new List<int>();
        public static object zamok = new object();
    }

    public class Program
    {
        // Подія для сигналізації про завершення генерування
        private static ManualResetEvent podia_gen_zaversheno = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            // Створюємо потоки для кожної операції
            Thread potik_generator = new Thread(() =>
            {
                GeneruvannyaChysel();
            });

            Thread potik_maksimum = new Thread(() =>
            {
                ZnahodzhennyaMaksymu();
            });

            Thread potik_minimum = new Thread(() =>
            {
                ZnahodzhennyaMinimu();
            });

            Thread potik_serednye = new Thread(() =>
            {
                ObchyslennyaSerednogho();
            });

            // Запускаємо всі потоки
            potik_generator.Start();
            potik_maksimum.Start();
            potik_minimum.Start();
            potik_serednye.Start();

            // Чекаємо поки всі потоки закінчаться
            potik_generator.Join();
            potik_maksimum.Join();
            potik_minimum.Join();
            potik_serednye.Join();

            Console.WriteLine("Всі операції завершені!");
        }

        // Перший потік генерує 1000 чисел від 0 до 5000
        static void GeneruvannyaChysel()
        {
            Console.WriteLine("Потік генерування почав роботу.");

            // Випадковий генератор для чисел
            Random rand = new Random();

            // Генеруємо 1000 чисел
            for (int i = 0; i < 1000; i++)
            {
                // Генеруємоо число від 0 до 5000
                int chyslo = rand.Next(0, 5001);

                // Додаємо число до списку, використовуючи замок щоб інші потоки не перешкоджали
                lock (Dani.zamok)
                {
                    Dani.chysla.Add(chyslo);
                }
            }

            Console.WriteLine("Генерування завершено. Всього чисел: " + Dani.chysla.Count);

            // Сигналізуємо що генерування завершилось, це дозволяє іншим потокам почати роботу
            podia_gen_zaversheno.Set();
        }

        // Другий потік знаходить максимум
        static void ZnahodzhennyaMaksymu()
        {
            // Чекаємо поки генерування завершиться
            podia_gen_zaversheno.WaitOne();

            Console.WriteLine("Потік максимуму почав роботу");

            // Беремо перше число як початкове максимальне значення
            int maksimum = Dani.chysla[0];

            // Проходимо по всіх ч��слах і шукаємо більше значення
            for (int i = 1; i < Dani.chysla.Count; i++)
            {
                // Якщо нове число більше за максимум, то воно стає новим максимумом
                if (Dani.chysla[i] > maksimum)
                {
                    maksimum = Dani.chysla[i];
                }
            }

            Console.WriteLine($"Максимальне число: {maksimum}");
        }

        // Третій потік знаходить мінімум
        static void ZnahodzhennyaMinimu()
        {
            // Чекаємо поки генерування завершиться
            podia_gen_zaversheno.WaitOne();

            Console.WriteLine("Потік мінімуму почав роботу");

            // Беремо перше число як початкове мінімальне значення
            int minimum = Dani.chysla[0];

            // Проходимо по всіх числах і шукаємо менше значення
            for (int i = 1; i < Dani.chysla.Count; i++)
            {
                // Якщо нове число менше за мінімум, то воно стає новим мінімумом
                if (Dani.chysla[i] < minimum)
                {
                    minimum = Dani.chysla[i];
                }
            }

            Console.WriteLine($"Мінімальне число: {minimum}");
        }

        // Четвертий потік обчислює середнє арифметичне
        static void ObchyslennyaSerednogho()
        {
            // Чекаємо поки генерування завершиться
            podia_gen_zaversheno.WaitOne();

            Console.WriteLine("Потік середнього почав роботу");

            // Сумуємо всі числа
            long suma = 0;
            for (int i = 0; i < Dani.chysla.Count; i++)
            {
                suma += Dani.chysla[i];
            }

            // Ділимо суму на кількість чисел щоб отримати середнє
            double serednye = (double)suma / Dani.chysla.Count;

            Console.WriteLine($"Середнє арифметичне: {serednye}");
        }
    }
}