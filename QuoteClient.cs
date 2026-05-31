using System.Net;
using System.Net.Sockets;
using System.Text;

namespace QuoteClient
{
    internal class QuoteClient
    {
        static async Task Main(string[] args)
        {
            TcpClient client = null;
            IPAddress remoteAddress = IPAddress.Loopback;
            int remotePort = 9999;

            try
            {
                // Pidklyuchaymosya do servera
                client = new TcpClient();
                await client.ConnectAsync(remoteAddress, remotePort);
                Console.WriteLine("Pidklyucheno do servera!");
                Console.WriteLine("Vvedit 'quote' dlya otrymannya tsytaty abo 'exit' dlya vykhodu\n");

                NetworkStream stream = client.GetStream();

                while (true)
                {
                    // Pytayemo korystuvalnyka shcho vvesty
                    Console.Write("Komanda: ");
                    string userInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userInput))
                        continue;

                    // Vidpravlyayemo komandu na server
                    byte[] data = Encoding.UTF8.GetBytes(userInput);
                    await stream.WriteAsync(data, 0, data.Length);

                    // Otrymuemo vidpovid vid servera
                    byte[] buffer = new byte[4096];
                    int received = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (received == 0)
                    {
                        Console.WriteLine("Server vidklyuchyvsya");
                        break;
                    }

                    string answer = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine($"Vidpovid: {answer}\n");

                    // Yakscho korystuvalnyky vviv exit
                    if (userInput.ToLower() == "exit")
                    {
                        Console.WriteLine("Vidklyuchennya...");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pomylka: {ex.Message}");
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                    Console.WriteLine("Z'yednannya zakryto");
                }
            }
        }
    }
}