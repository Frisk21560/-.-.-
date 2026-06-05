using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    static async Task Main(string[] args)
    {
        // Zberihayemo chaty ta yikhniu istoriyuyu
        Dictionary<string, ChatRoom> chatRooms = new Dictionary<string, ChatRoom>();
        Dictionary<string, List<string>> chatHistories = new Dictionary<string, List<string>>();

        IPAddress serverIp = IPAddress.Any;
        int serverPort = 9090;
        IPEndPoint serverEp = new IPEndPoint(serverIp, serverPort);
        using TcpListener server = new TcpListener(serverEp);

        server.Start();
        Console.WriteLine($"Server zapushcheno: {server.LocalEndpoint}");

        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Klient pidklyuchyvsya: {client.Client.RemoteEndPoint}");
            _ = Task.Run(() => HandleClientAsync(client, chatRooms, chatHistories));
        }
    }

    private static async Task HandleClientAsync(
        TcpClient client,
        Dictionary<string, ChatRoom> chatRooms,
        Dictionary<string, List<string>> chatHistories)
    {
        NetworkStream stream = client.GetStream();
        string clientAddress = client.Client.RemoteEndPoint.ToString();
        string currentChatRoom = "General";
        string username = "";

        try
        {
            // Otrymuemo username vid klienta
            byte[] bufferUsername = new byte[1024];
            int receivedUsername = await stream.ReadAsync(bufferUsername, 0, bufferUsername.Length);
            username = Encoding.UTF8.GetString(bufferUsername, 0, receivedUsername);

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Klient {clientAddress} predstavyvsya: {username}");

            // Stvoryuyemo chat room yakscho ne isnuye
            if (!chatRooms.ContainsKey(currentChatRoom))
            {
                chatRooms[currentChatRoom] = new ChatRoom(currentChatRoom);
                chatHistories[currentChatRoom] = new List<string>();
            }

            // Dodayemo do chata
            chatRooms[currentChatRoom].AddClient(client, username);

            // Spovidom pro pidklyuchennya
            string joinMessage = $"[SYSTEM]: {username} priyednuvsya do {currentChatRoom}";
            chatHistories[currentChatRoom].Add(joinMessage);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {joinMessage}");
            BroadcastMessageToRoom(chatRooms[currentChatRoom], joinMessage);

            while (true)
            {
                byte[] buffer = new byte[4096];
                int received = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (received == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, received);

                // Peremikat' mizh chatamy
                if (message.StartsWith("/join "))
                {
                    string newChatRoom = message.Substring(6).Trim();

                    // Vydalyayemo z popeerednoho chata
                    chatRooms[currentChatRoom].RemoveClient(client);
                    string leftMessage = $"[SYSTEM]: {username} vinyshov z {currentChatRoom}";
                    chatHistories[currentChatRoom].Add(leftMessage);
                    BroadcastMessageToRoom(chatRooms[currentChatRoom], leftMessage);

                    // Perekhodymo v novyy chat
                    currentChatRoom = newChatRoom;

                    if (!chatRooms.ContainsKey(currentChatRoom))
                    {
                        chatRooms[currentChatRoom] = new ChatRoom(currentChatRoom);
                        chatHistories[currentChatRoom] = new List<string>();
                    }

                    chatRooms[currentChatRoom].AddClient(client, username);

                    // Vidpravlyayemo istoriyuyu novogo chata
                    string historyMessage = "HISTORY:" + string.Join("|", chatHistories[currentChatRoom]);
                    byte[] historyData = Encoding.UTF8.GetBytes(historyMessage);
                    await stream.WriteAsync(historyData, 0, historyData.Length);

                    string joinNewMessage = $"[SYSTEM]: {username} priyednuvsya do {currentChatRoom}";
                    chatHistories[currentChatRoom].Add(joinNewMessage);
                    BroadcastMessageToRoom(chatRooms[currentChatRoom], joinNewMessage);

                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {username} pereshov u {currentChatRoom}");
                }
                // Zvychaynyy chat
                else
                {
                    string formattedMessage = $"[{username}]: {message}";
                    chatHistories[currentChatRoom].Add(formattedMessage);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {formattedMessage} (chat: {currentChatRoom})");
                    BroadcastMessageToRoom(chatRooms[currentChatRoom], formattedMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Pomylka: {ex.Message}");
        }
        finally
        {
            // Vydalyayemo klienta
            if (chatRooms.ContainsKey(currentChatRoom))
            {
                chatRooms[currentChatRoom].RemoveClient(client);
                string disconnectMessage = $"[SYSTEM]: {username} vidklyuchyvsya z {currentChatRoom}";
                chatHistories[currentChatRoom].Add(disconnectMessage);
                BroadcastMessageToRoom(chatRooms[currentChatRoom], disconnectMessage);
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Klient {clientAddress} ({username}) vidklyuchyvsya\n");
            stream.Close();
            client.Close();
        }
    }

    private static void BroadcastMessageToRoom(ChatRoom room, string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        foreach (var client in room.Clients)
        {
            try
            {
                _ = client.Key.GetStream().WriteAsync(data, 0, data.Length);
            }
            catch { }
        }
    }
}

public class ChatRoom
{
    public string Name { get; set; }
    public Dictionary<TcpClient, string> Clients { get; set; }

    public ChatRoom(string name)
    {
        Name = name;
        Clients = new Dictionary<TcpClient, string>();
    }

    public void AddClient(TcpClient client, string username)
    {
        Clients[client] = username;
    }

    public void RemoveClient(TcpClient client)
    {
        Clients.Remove(client);
    }
}