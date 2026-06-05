using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Broadcast_Client__homework_
{
    public class Program
{
    const int serverPort = 9050;
    static IPAddress serverIp;

    static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Broadcast Client z pidpyskamy";

        // Zapytuyemo IP servera
        Console.Write("Vvedit IP servera: ");
        serverIp = IPAddress.Parse(Console.ReadLine() ?? "127.0.0.1");

        try
        {
            // Reyestruemo klienta na serveri
            RegisterWithServer();

            // Zapytuyemo yaki typy povidomlen' klient khoche
            SendSubscription();

            Console.WriteLine("Klient zarejestrovano. Ochikuvannya povidomlen'...\n");

            // Slukhayemo povidomlennya vid servera
            ReceiveMessages();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Pomylka: " + ex.Message);
        }
    }

    // Funkciya dlya reyestracyi klienta na serveri
    private static void RegisterWithServer()
    {
        try
        {
            using UdpClient client = new UdpClient();
            IPEndPoint serverEp = new IPEndPoint(serverIp, serverPort);

            // Vidpravlyayemo povidomlennya reyestracyi
            string registerMessage = "REGISTER";
            byte[] data = Encoding.UTF8.GetBytes(registerMessage);
            client.Send(data, serverEp);

            Console.WriteLine("Reyestraciya na serveri...\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Pomylka reyestracyi: " + ex.Message);
        }
    }

    // Funkciya dlya pidpysky na typy povidomlen'
    private static void SendSubscription()
    {
        Console.WriteLine("Oberyit typy povidomlen' dlya pidpysky:");
        Console.WriteLine("1 - Novyna (News)");
        Console.WriteLine("2 - Nagaduyvannya (Reminder)");
        Console.WriteLine("3 - Rozvazhalne (Entertainment)");
        Console.Write("Vvedit nomery cherez komu (napryklad: 1,3): ");

        string subscriptionInput = Console.ReadLine();

        try
        {
            using UdpClient client = new UdpClient();
            IPEndPoint serverEp = new IPEndPoint(serverIp, serverPort);

            // Formuemo povidomlennya pidpysky
            string subscriptionMessage = $"SUBSCRIBE:{subscriptionInput}";
            byte[] data = Encoding.UTF8.GetBytes(subscriptionMessage);
            client.Send(data, serverEp);

            Console.WriteLine("Pidpyska vyslana!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Pomylka pidpysky: " + ex.Message);
        }
    }

    // Funkciya dlya otrymannya povidomlen' vid servera
    private static void ReceiveMessages()
    {
        try
        {
            using UdpClient receiver = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                // Otrymuemo povidomlennya
                byte[] buffer = receiver.Receive(ref remoteEp);
                string message = Encoding.UTF8.GetString(buffer);

                // Vyvodymо otrymanе povidomlennya
                // Ekstrene povidomlennya v chervonomy
                if (message.Contains("[Emergency]"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
                    Console.ResetColor();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Pomylka pry otrymannyu: " + ex.Message);
        }
    }
}
}