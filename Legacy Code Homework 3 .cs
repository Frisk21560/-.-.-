using System;
using System.Threading;

namespace MultithreadingApp
{
    // Завдання 1 - Консольний годинник з Timer
    class ConsoleClockApp
    {
        // Змінна для Timer'а
        private static Timer zminna_timer = null;

        static void Main()
        {
            Console.WriteLine("Завдання 1: Консольний Годинник");
            Console.WriteLine("Годинник працює. Натисни Enter для виходу.\n");

            // Створюємо Timer
            // Timer виконує метода кожну секунду (1000 мс)
            zminna_timer = new Timer(TimerCallback, null, 0, 1000);

            // Чекаємо на Enter від користувача
            Console.ReadLine();

            // Зупиняємо Timer
            zminna_timer.Dispose();

            Console.WriteLine("\nГодинник зупинен.");
        }

        // Метода що виконується кожну секунду
        private static void TimerCallback(object zminna_state)
        {
            // Очищаємо консоль від попереднього виведу
            Console.Clear();

            // Отримуємо поточний час
            DateTime zminna_chas_teper = DateTime.Now;

            // Виводимо годинник у форматі HH:mm:ss
            Console.WriteLine("КОНСОЛЬНИЙ ГОДИННИК\n");
            // Встановлюємо блакитний колір тексту
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Поточний час: {zminna_chas_teper:HH:mm:ss}");
            // Повертаємо білий колір
            Console.ForegroundColor = ConsoleColor.White;

            // Додаткова інформація
            Console.WriteLine($"День тижня: {zminna_chas_teper:dddd}");
            Console.WriteLine($"Дата: {zminna_chas_teper:dd.MM.yyyy}");
            Console.WriteLine($"\nНатисни Enter для виходу...");
        }
    }
}