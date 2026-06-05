using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Broadcast_Server
{

    public class Program
    {
        // Список всіх підключених клієнтів
        static List<IPEndPoint> connectedClients = new List<IPEndPoint>();
        const int port = 9050;

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Broadcast Server";

            // Запускаємо сервер для отримання реєстрацій клієнтів
            Thread serverThread = new Thread(ServerListening);
            serverThread.IsBackground = true;
            serverThread.Start();

            Console.WriteLine("Сервер запущено. Адміністратор, вводьте повідомлення:\n");

            // Основний цикл для введення повідомлень
            while (true)
            {
                Console.Write("Введіть повідомлення: ");
                string message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message))
                    continue;

                // Відправляємо повідомлення всім клієнтам
                SendBroadcastMessage(message);
            }
        }

        // Функція для прослуховування реєстрацій клієнтів
        private static void ServerListening()
        {
            try
            {
                using UdpClient receiver = new UdpClient(port);
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, port);

                Console.WriteLine($"Сервер слухає на порту {port}\n");

                while (true)
                {
                    // Отримуємо повідомлення від клієнта (реєстрація)
                    byte[] buffer = receiver.Receive(ref remoteEp);
                    string message = Encoding.UTF8.GetString(buffer);

                    // Перевіряємо чи це повідомлення реєстрації
                    if (message == "REGISTER")
                    {
                        // Додаємо клієнта до списку
                        if (!connectedClients.Contains(remoteEp))
                        {
                            connectedClients.Add(remoteEp);
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Новий клієнт підключився: {remoteEp}");
                            Console.WriteLine($"Всього підключених клієнтів: {connectedClients.Count}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка сервера: " + ex.Message);
            }
        }

        // Функція для розсилання повідомлення всім клієнтам
        private static void SendBroadcastMessage(string messageText)
        {
            try
            {
                using UdpClient sender = new UdpClient();
                byte[] data = Encoding.UTF8.GetBytes(messageText);

                // Відправляємо повідомлення кожному клієнту
                foreach (var client in connectedClients)
                {
                    sender.Send(data, client);
                }

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Повідомлення відправлено {connectedClients.Count} клієнтам\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при відправленні: " + ex.Message);
            }
        }
    }
}