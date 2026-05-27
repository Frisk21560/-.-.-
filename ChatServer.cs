using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    public class ChatServer
    {
        private static List<ConnectedClient> connectedClients = new List<ConnectedClient>();
        private static List<string> chatHistory = new List<string>();

        static async Task Main(string[] args)
        {
            using Socket server = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            int localPort = 9090;
            IPAddress localIp = IPAddress.Any;
            IPEndPoint localEp = new IPEndPoint(localIp, localPort);
            server.Bind(localEp);
            server.Listen();

            Console.WriteLine("Server zapushcheno na portu " + localPort);

            while (true)
            {
                Socket client = await server.AcceptAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        private static async Task HandleClientAsync(Socket client)
        {
            try
            {
                // Otrymuemo username vid klienta
                byte[] bufferUsername = new byte[1024];
                int bytesUsername = await client.ReceiveAsync(bufferUsername);
                string username = Encoding.UTF8.GetString(bufferUsername, 0, bytesUsername);

                // Stvoryuyemo ob'yekt klienta
                ConnectedClient connectedClient = new ConnectedClient
                {
                    Socket = client,
                    Username = username
                };

                // Dodayemo do spysku pidk'luchenyh
                lock (connectedClients)
                {
                    connectedClients.Add(connectedClient);
                }

                // Dodayemo povidomlennya v istoriyuyu
                string joinMessage = $"[SYSTEM]: {username} priyednuvsya do chatu";
                lock (chatHistory)
                {
                    chatHistory.Add(joinMessage);
                }

                Console.WriteLine(joinMessage);

                // Rozsilayemo sys povidomlennya vsim klientam
                await BroadcastMessageAsync(joinMessage);

                // Slyshayemo povidomlennya vid klienta
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int bytes = await client.ReceiveAsync(buffer);
                    if (bytes == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytes);
                    string formattedMessage = $"[{username}]: {message}";

                    lock (chatHistory)
                    {
                        chatHistory.Add(formattedMessage);
                    }

                    Console.WriteLine(formattedMessage);

                    // Rozsilayemo povidomlennya vsim klientam
                    await BroadcastMessageAsync(formattedMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Pomylka: " + ex.Message);
            }
            finally
            {
                // Vydalyayemo klienta iz spysku
                lock (connectedClients)
                {
                    connectedClients.RemoveAll(c => c.Socket == client);
                }

                client.Close();
            }
        }

        private static async Task BroadcastMessageAsync(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            lock (connectedClients)
            {
                foreach (var client in connectedClients)
                {
                    try
                    {
                        _ = client.Socket.SendAsync(data);
                    }
                    catch
                    {
                        // Yakscho pomylka, propuskayemo
                    }
                }
            }
        }
    }
}