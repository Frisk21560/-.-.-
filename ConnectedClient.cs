using System.Net.Sockets;

namespace ChatServer
{
    public class ConnectedClient
    {
        // Socket dlya z'yednannya z klientom
        public Socket Socket { get; set; }

        // Username klienta
        public string Username { get; set; }
    }
}