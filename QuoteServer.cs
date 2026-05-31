using System.Net;
using System.Net.Sockets;
using System.Text;

namespace QuoteServer
{
    internal class QuoteServer
    {
        static async Task Main(string[] args)
        {
            // Masyv tsytat
            string[] quotes = new string[]
            {
        "Zhyttya - tse те, shcho vidbuvayetsya, koly ty robyshy inshi plany.",
        "Molodist - tse dosvid, yakogo she ne maye.",
        "Uspikh - tse suma malenkykh zusyl, roblyh den za dnem.",
        "Budh zavzhdy samymy soboyu. Shche nikto ne buv krashe vid vas.",
        "Ne baiysya kinchytysa - baiysya ne pochydaty.",
        "Chym bilshe chornyy, tym bilshe svitla treba.",
        "Lyuby - tse shche ne robia lidi idealnymy.",
        "Uspikh - tse ne dostatochna, ale neobkhidna umova dlya shchastya.",
        "Zhyttya ye nadto korotke, shchob pyty poganu vyno.",
        "Kozzhen den - tse nova mozhlyvist."
            };

            IPAddress serverIp = IPAddress.Any;
            int serverPort = 9999;
            IPEndPoint serverEp = new IPEndPoint(serverIp, serverPort);
            using TcpListener server = new TcpListener(serverEp);

            server.Start();
            Console.WriteLine($"Server zapushcheno: {server.LocalEndpoint}");
            Console.WriteLine("Ochikuvannya pidklyuchennya klientiv...\n");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Klient pidklyuchyvsya: {client.Client.RemoteEndPoint}");
                _ = Task.Run(() => HandleClientAsync(client, quotes));
            }
        }

        private static async Task HandleClientAsync(TcpClient client, string[] quotes)
        {
            NetworkStream stream = client.GetStream();
            string clientAddress = client.Client.RemoteEndPoint.ToString();

            try
            {
                while (true)
                {
                    byte[] buffer = new byte[4096];
                    int received = await stream.ReadAsync(buffer, 0, buffer.Length);

                    // Yakscho 0 bajtiv - klient vidklyuchyvsya
                    if (received == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Zapyt vid {clientAddress}: {message}");

                    // Yakscho "exit" - zakrivayemo z'yednannya
                    if (message.ToLower() == "exit")
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Klient {clientAddress} prysyp zaproshuvaty");
                        break;
                    }

                    // Yakscho "quote" - vidpravlyayemo tsytatu
                    if (message.ToLower() == "quote")
                    {
                        Random random = new Random();
                        string quote = quotes[random.Next(quotes.Length)];

                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Tsytata nadislana {clientAddress}: {quote}");

                        byte[] data = Encoding.UTF8.GetBytes(quote);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                    else
                    {
                        string answer = "Napyshit 'quote' dlya otrymannya tsytaty abo 'exit' dlya vykhodu";
                        byte[] data = Encoding.UTF8.GetBytes(answer);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Pomylka: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Klient {clientAddress} vidklyuchyvsya.\n");
                stream.Close();
                client.Close();
            }
        }
    }
}