using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Broadcast_Server__homework_
{
    // Typ povidomlennya
    public enum MessageType
    {
        News = 1,
        Reminder = 2,
        Entertainment = 3,
        Emergency = 4
    }

    // Kliyent z jyogo pidpyskoyu
    public class ClientSubscription
    {
        public IPEndPoint Endpoint { get; set; }
        public List<MessageType> SubscribedTypes { get; set; }

        public ClientSubscription(IPEndPoint endpoint)
        {
            Endpoint = endpoint;
            SubscribedTypes = new List<MessageType>();
        }
    }

    public class Program
    {
        // Dynamic spysok pidk'luchenyh klientiv
        static Dictionary<string, ClientSubscription> connectedClients = new Dictionary<string, ClientSubscription>();
        const int port = 9050;

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Broadcast Server z pidpyskamy";

            // Zapuskayemo server dlya otrymannya reyestraciy
            Thread serverThread = new Thread(ServerListening);
            serverThread.IsBackground = true;
            serverThread.Start();

            Console.WriteLine("Server zapushcheno. Administrator, vvodyte povidomlennya:\n");
            Console.WriteLine("Dostupni typy:");
            Console.WriteLine("1 - Novyna (News)");
            Console.WriteLine("2 - Nagaduyvannya (Reminder)");
            Console.WriteLine("3 - Rozvazhalne (Entertainment)");
            Console.WriteLine("4 - Ekstrene (Emergency)\n");

            // Osnovnyy cykl dlya vvedennya povidomlen'
            while (true)
            {
                Console.Write("Vybir typ (1-4): ");
                int typeChoice = Convert.ToInt32(Console.ReadLine() ?? "1");

                Console.Write("Vvedit tekst povidomlennya: ");
                string messageText = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(messageText))
                    continue;

                // Vyznachayemo typ povidomlennya
                MessageType messageType = (MessageType)typeChoice;

                // Vidpravlyayemo povidomlennya vidpovidnym klientam
                SendMessage(messageText, messageType);
            }
        }

        // Funkciya dlya prosluhhovuvannya reyestraciy ta pidpysok klientiv
        private static void ServerListening()
        {
            try
            {
                using UdpClient receiver = new UdpClient(port);
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, port);

                Console.WriteLine($"Server slukhaye na portu {port}\n");

                while (true)
                {
                    // Otrymuemo povidomlennya vid klienta
                    byte[] buffer = receiver.Receive(ref remoteEp);
                    string message = Encoding.UTF8.GetString(buffer);

                    // Yakscho REGISTER - dodayemo klienta
                    if (message == "REGISTER")
                    {
                        string clientKey = remoteEp.ToString();

                        if (!connectedClients.ContainsKey(clientKey))
                        {
                            connectedClients[clientKey] = new ClientSubscription(remoteEp);
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Novyy klient pidk'luchyvsya: {remoteEp}");
                            Console.WriteLine($"Vsogo pidk'luchenyh: {connectedClients.Count}\n");
                        }
                    }
                    // Yakscho SUBSCRIBE - dodayemo pidpysku
                    else if (message.StartsWith("SUBSCRIBE:"))
                    {
                        string clientKey = remoteEp.ToString();
                        string subscriptionData = message.Substring(10);
                        string[] types = subscriptionData.Split(',');

                        if (connectedClients.ContainsKey(clientKey))
                        {
                            connectedClients[clientKey].SubscribedTypes.Clear();

                            foreach (var typeStr in types)
                            {
                                if (int.TryParse(typeStr.Trim(), out int typeNum))
                                {
                                    connectedClients[clientKey].SubscribedTypes.Add((MessageType)typeNum);
                                }
                            }

                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Klient {remoteEp} ominiv pidpysku");
                            Console.WriteLine($"Pidpysanyy na: {string.Join(", ", connectedClients[clientKey].SubscribedTypes)}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Pomylka servera: " + ex.Message);
            }
        }

        // Funkciya dlya rozsilannya povidomlennya vidpovidnym klientam
        private static void SendMessage(string messageText, MessageType messageType)
        {
            try
            {
                using UdpClient sender = new UdpClient();

                // Formuemo povidomlennya z typom
                string formattedMessage = $"[{messageType}]: {messageText}";
                byte[] data = Encoding.UTF8.GetBytes(formattedMessage);

                int sentCount = 0;

                // Yakscho ekstrene - vysylaemo vsim
                if (messageType == MessageType.Emergency)
                {
                    foreach (var client in connectedClients.Values)
                    {
                        sender.Send(data, client.Endpoint);
                        sentCount++;
                    }
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] EKSTRENE POVIDOMLENNYA vysylano {sentCount} klientam\n");
                }
                // Inakshe - lyshe pidpysanym
                else
                {
                    foreach (var client in connectedClients.Values)
                    {
                        if (client.SubscribedTypes.Contains(messageType))
                        {
                            sender.Send(data, client.Endpoint);
                            sentCount++;
                        }
                    }
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Povidomlennya typu '{messageType}' vysylano {sentCount} klientam\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Pomylka pry vidpravlennyu: " + ex.Message);
            }
        }
    }
}