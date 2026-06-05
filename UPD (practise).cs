using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UPD__practise_
{
    public class Program
    {
        static int RemotePort; // порт куди відправляємо
        static IPAddress RemoteIPAddr = null!; // IP куди відправляємо
        static int LocalPort; // наш порт
        static string Username = ""; // ім'я користувача
        static ConsoleColor MessageColor; // колір повідомлення

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.Title = "UDP Чат";

            // Запитуємо IP адресу отримувача
            Console.Write("Введіть IP отримувача: ");
            RemoteIPAddr = IPAddress.Parse(Console.ReadLine() ?? "127.0.0.1");

            // Запитуємо порт отримувача
            Console.Write("Введіть порт отримувача: ");
            RemotePort = Convert.ToInt32(Console.ReadLine());

            // Запитуємо наш локальний порт
            Console.Write("Введіть локальний порт: ");
            LocalPort = Convert.ToInt32(Console.ReadLine());

            // Запитуємо ім'я користувача
            Console.Write("Введіть ваше ім'я: ");
            Username = Console.ReadLine() ?? "Анонімний";

            // Показуємо меню вибору кольору
            Console.WriteLine("\nОберіть колір повідомлення:");
            Console.WriteLine("1 - Червоний (Red)");
            Console.WriteLine("2 - Зелений (Green)");
            Console.WriteLine("3 - Жовтий (Yellow)");
            Console.WriteLine("4 - Голубий (Cyan)");
            Console.WriteLine("5 - Малиновий (Magenta)");
            Console.WriteLine("6 - Білий (White)");
            Console.Write("Ваш вибір (1-6): ");

            // Перетворюємо вибір користувача в колір
            int colorChoice = Convert.ToInt32(Console.ReadLine() ?? "1");
            MessageColor = GetColorFromChoice(colorChoice);

            // Створюємо потік для отримання повідомлень
            Thread receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Console.ForegroundColor = MessageColor;
            Console.WriteLine($"\nПривіт! Вводьте повідомлення (колір: {MessageColor}):\n");

            // Основний цикл для відправлення повідомлень
            while (true)
            {
                Console.ForegroundColor = MessageColor;
                string message = Console.ReadLine() ?? "";
                SendData(message);
            }
        }

        // Функція для перетворення вибору в колір
        private static ConsoleColor GetColorFromChoice(int choice)
        {
            return choice switch
            {
                1 => ConsoleColor.Red,
                2 => ConsoleColor.Green,
                3 => ConsoleColor.Yellow,
                4 => ConsoleColor.Cyan,
                5 => ConsoleColor.Magenta,
                6 => ConsoleColor.White,
                7 => ConsoleColor.White
            };
        }

        // Функція для отримання повідомлень
        private static void ReceiveData()
        {
            try
            {
                while (true)
                {
                    // Створюємо UDP клієнт на нашому локальному порту
                    UdpClient uClient = new UdpClient(LocalPort);
                    IPEndPoint ipEnd = null!;

                    // Отримуємо повідомлення від іншого клієнта
                    byte[] response = uClient.Receive(ref ipEnd);

                    // Розпаковуємо отримані дані з UTF8 кодуванням
                    string receivedData = Encoding.UTF8.GetString(response);

                    // Розділяємо дані на частини: ім'я|повідомлення|колір
                    string[] parts = receivedData.Split('|');

                    if (parts.Length == 3)
                    {
                        string receivedUsername = parts[0];
                        string message = parts[1];
                        int colorCode = Convert.ToInt32(parts[2]);
                        ConsoleColor receivedColor = (ConsoleColor)colorCode;

                        // Встановлюємо колір і виводимо повідомлення
                        Console.ForegroundColor = receivedColor;
                        Console.WriteLine($"[{receivedUsername}]: {message}");
                        Console.ForegroundColor = MessageColor;
                    }

                    // Закриваємо UDP клієнт
                    uClient.Close();
                }
            }
            catch (SocketException sockEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Помилка сокета: " + sockEx.Message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }

        // Функція для відправлення повідомлень
        private static void SendData(string data)
        {
            // Перевіряємо чи повідомлення не пусте
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            // Створюємо UDP клієнт для відправлення
            UdpClient uClient = new UdpClient();

            // Створюємо адресу адресата
            IPEndPoint ipEnd = new IPEndPoint(RemoteIPAddr, RemotePort);

            try
            {
                // Пакуємо дані у форматі: ім'я|повідомлення|колір
                string colorCode = ((int)MessageColor).ToString();
                string packedData = $"{Username}|{data}|{colorCode}";

                // Конвертуємо рядок у байти з UTF8 кодуванням
                byte[] bytes = Encoding.UTF8.GetBytes(packedData);

                // Відправляємо дані адресату
                uClient.Send(bytes, bytes.Length, ipEnd);

                // Виводимо своє повідомлення в консоль
                Console.ForegroundColor = MessageColor;
                Console.WriteLine($"[{Username}]: {data}");
            }
            catch (SocketException sockEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Помилка сокета: " + sockEx.Message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Помилка: " + ex.Message);
            }
            finally
            {
                // Закриваємо UDP клієнт
                uClient.Close();
            }
        }
    }
}