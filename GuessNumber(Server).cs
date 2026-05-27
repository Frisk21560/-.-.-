using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Homework_3__Server_
{
    static void Main(string[] args)
    {
        // Zadumuyemo chyslo vid 1 do 100
        Random random = new Random();
        int secretNumber = random.Next(1, 101);

        Console.WriteLine("Servер zapushcheno. Zadumane chyslo: " + secretNumber);

        IPAddress localIp = IPAddress.Any;
        int localPort = 9999;
        EndPoint localEp = new IPEndPoint(localIp, localPort);

        using Socket localSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Dgram,
            ProtocolType.Udp);

        localSocket.Bind(localEp);

        while (true)
        {
            byte[] buffer = new byte[4096];
            EndPoint? remoteEp = new IPEndPoint(IPAddress.Any, 0);

            // Otrymuemo sprovu vid klienta
            int bytesRead = localSocket.ReceiveFrom(buffer, ref remoteEp);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"[LOG] Nova sprava: {message}. Vidbyvach: {remoteEp}");

            string answer = "";

            // Pereviroymo chi mozhlivo sprava bude chyslom
            if (int.TryParse(message, out int guess))
            {
                if (guess < secretNumber)
                {
                    answer = "MALO! Chyslo bilshe.";
                }
                else if (guess > secretNumber)
                {
                    answer = "BAGATO! Chyslo menshe.";
                }
                else
                {
                    answer = "VGADAV! Hvala za ihru!";
                    Console.WriteLine($"Gravet z {remoteEp} peremig!");

                    // Rozsilayemo povidomlennya pro peremogu vsim
                    byte[] winMessage = Encoding.UTF8.GetBytes($"Gravet z {remoteEp} peremig!");
                    localSocket.SendTo(winMessage, remoteEp);

                    break; // Zavershuyemo sервер
                }
            }
            else
            {
                answer = "POMYLKA! Vvedit chyslo!";
            }

            byte[] data = Encoding.UTF8.GetBytes(answer);

            // Vidpravlyayemo vidpovid klienty
            localSocket.SendTo(data, remoteEp);

            Thread.Sleep(1000);
        }
    }
}