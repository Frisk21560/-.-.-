using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network_Homework_1
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                // Запитуємо користувача, яку адресу він хоче відкрити
                Console.Write("Введіть адресу сервера (IP або ім'я): ");
                string serverName = Console.ReadLine();

                // Запитуємо порт
                Console.Write("Введіть номер порту: ");
                int port = int.Parse(Console.ReadLine());

                // Створюємо адресу сервера
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverName), port);

                // Створюємо сокет для з'єднання
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // З'єднуємося з сервером
                clientSocket.Connect(serverEP);

                // Формуємо HTTP запит
                string request = "GET / HTTP/1.1\r\nHost: localhost\r\nConnection: close\r\n\r\n";

                // Відправляємо запит на сервер
                clientSocket.Send(Encoding.UTF8.GetBytes(request));

                // Буфер для отримання даних
                byte[] buffer = new byte[1024];
                int bytesReceived;

                // Змінна для накопичення результату
                StringBuilder result = new StringBuilder();

                // Отримуємо дані доки не буде пусто
                do
                {
                    bytesReceived = clientSocket.Receive(buffer);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    result.Append(receivedData);
                } while (bytesReceived > 0);

                // Виводимо результат
                Console.WriteLine("\nВідповідь від сервера:\n");
                Console.WriteLine(result.ToString());

                // Закриваємо сокет
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
    }
}