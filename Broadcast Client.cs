using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Broadcast_Client
{
    public class Program
    {
        const int serverPort = 9050;

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Broadcast Client";

            // Запитуємо IP сервера
            Console.Write("Введіть IP сервера: ");
            IPAddress serverIp = IPAddress.Parse(Console.ReadLine() ?? "127.0.0.1");

            try
            {
                // Реєструємо клієнта на сервері
                RegisterWithServer(serverIp);

                Console.WriteLine("Клієнт зареєстровано. Очікування повідомлень...\n");

                // Слухаємо повідомлення від сервера
                ReceiveMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }

        // Функція для реєстрації клієнта на сервері
        private static void RegisterWithServer(IPAddress serverIp)
        {
            try
            {
                using UdpClient client = new UdpClient();
                IPEndPoint serverEp = new IPEndPoint(serverIp, serverPort);

                // Відправляємо повідомлення реєстрації
                string registerMessage = "REGISTER";
                byte[] data = Encoding.UTF8.GetBytes(registerMessage);
                client.Send(data, serverEp);

                Console.WriteLine("Реєстрація на сервері...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка реєстрації: " + ex.Message);
            }
        }

        // Функція для отримання повідомлень від сервера
        private static void ReceiveMessages()
        {
            try
            {
                using UdpClient receiver = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    // Отримуємо повідомлення
                    byte[] buffer = receiver.Receive(ref remoteEp);
                    string message = Encoding.UTF8.GetString(buffer);

                    // Виводимо отримане повідомлення
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Повідомлення: {message}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні: " + ex.Message);
            }
        }
    }
}