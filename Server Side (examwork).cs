using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server_Side__examwork_
{
    // Klasa dlya korystuvalnyкa
    public class User
    {
        public Socket socket { get; set; }
        public string name { get; set; }
        public string pass { get; set; }
        public List<string> groups { get; set; }

        public User()
        {
            groups = new List<string>();
        }
    }

    // Klasa dlya grupy
    public class Group
    {
        public string name { get; set; }
        public List<string> members { get; set; }
        public List<string> history { get; set; }

        public Group(string n)
        {
            name = n;
            members = new List<string>();
            history = new List<string>();
        }
    }

    static class Program
    {
        // Zberigayemo reyestrovanyh korystuvalnykov
        static Dictionary<string, User> reg = new Dictionary<string, User>();
        // Zberigayemo pidklyuchenyh korystuvalnykov
        static Dictionary<string, User> online = new Dictionary<string, User>();
        // Zberigayemo grupy
        static Dictionary<string, Group> chaty = new Dictionary<string, Group>();

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Chat Server";

            // Stvoryuyemo server
            IPAddress ip = IPAddress.Any;
            int port = 9090;
            IPEndPoint ep = new IPEndPoint(ip, port);
            using TcpListener srv = new TcpListener(ep);

            srv.Start();
            Console.WriteLine($"Server pracyuye na {srv.LocalEndpoint}\n");

            // Osnovnyy cykl - chekaymo klientiv
            while (true)
            {
                TcpClient klient = await srv.AcceptTcpClientAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Novyy klient");
                _ = Task.Run(() => Obroblyuvaty(klient));
            }
        }

        // Obrobka zapytiv vid klienta
        static async Task Obroblyuvaty(TcpClient klient)
        {
            NetworkStream potik = klient.GetStream();
            string user = "";

            try
            {
                byte[] buf = new byte[1024];

                while (true)
                {
                    // Chytayemo zapyt
                    int recv = await potik.ReadAsync(buf, 0, buf.Length);
                    if (recv == 0) break;

                    string zapyt = Encoding.UTF8.GetString(buf, 0, recv);
                    string[] chastyny = zapyt.Split('|');

                    string cmd = chastyny[0];

                    // Reyestraciya
                    if (cmd == "REGISTER")
                    {
                        string u = chastyny[1];
                        string p = chastyny[2];

                        if (!reg.ContainsKey(u))
                        {
                            reg[u] = new User
                            {
                                name = u,
                                pass = p
                            };
                            await Vidp(potik, "OK|Reyestraciya OK");
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Novyy user: {u}");
                        }
                        else
                        {
                            await Vidp(potik, "ERROR|User vzhe ye");
                        }
                    }
                    // Vkhid
                    else if (cmd == "LOGIN")
                    {
                        string u = chastyny[1];
                        string p = chastyny[2];

                        if (reg.ContainsKey(u) && reg[u].pass == p)
                        {
                            user = u;
                            online[u] = new User { name = u };
                            await Vidp(potik, "OK|Vkhid OK");
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {u} v systemi");
                        }
                        else
                        {
                            await Vidp(potik, "ERROR|Neverno");
                        }
                    }
                    // Pryvat
                    else if (cmd == "PRIVATE")
                    {
                        string komu = chastyny[1];
                        string msg = chastyny[2];

                        if (online.ContainsKey(komu))
                        {
                            string pov = $"PRIV|{user}|{msg}";
                            await Vidp(potik, pov);
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {user} -> {komu}: {msg}");
                        }
                        else
                        {
                            await Vidp(potik, "ERROR|User ne znayydenyy");
                        }
                    }
                    // Stvorennya grupy
                    else if (cmd == "MKGROUP")
                    {
                        string gname = chastyny[1];

                        if (!chaty.ContainsKey(gname))
                        {
                            chaty[gname] = new Group(gname);
                            chaty[gname].members.Add(user);
                            online[user].groups.Add(gname);
                            await Vidp(potik, "OK|Grupa OK");
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Grupa '{gname}' tvorena {user}");
                        }
                        else
                        {
                            await Vidp(potik, "ERROR|Grupa ye");
                        }
                    }
                    // Priyednannya do grupy
                    else if (cmd == "JOINGROUP")
                    {
                        string gname = chastyny[1];

                        if (chaty.ContainsKey(gname))
                        {
                            if (!chaty[gname].members.Contains(user))
                            {
                                chaty[gname].members.Add(user);
                                online[user].groups.Add(gname);
                                await Vidp(potik, "OK|V grupi");
                                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {user} v grupi {gname}");
                            }
                            else
                            {
                                await Vidp(potik, "ERROR|Vzhe v grupi");
                            }
                        }
                        else
                        {
                            await Vidp(potik, "ERROR|Grupy nema");
                        }
                    }
                    // Povidomlennya v grupu
                    else if (cmd == "GROUPMSG")
                    {
                        string gname = chastyny[1];
                        string msg = chastyny[2];

                        if (chaty.ContainsKey(gname) && chaty[gname].members.Contains(user))
                        {
                            string pov = $"GMSG|{gname}|{user}|{msg}";
                            chaty[gname].history.Add(pov);
                            await Vidp(potik, "OK|Vyslano");
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {user} v {gname}: {msg}");
                        }
                        else
                        {
                            await Vidp(potik, "ERROR|Ne v grupi");
                        }
                    }
                    // Poluchennya spysku users
                    else if (cmd == "GETUSERS")
                    {
                        string users = string.Join(",", online.Keys);
                        await Vidp(potik, $"USERS|{users}");
                    }
                    // Poluchennya spysku grup
                    else if (cmd == "GETGROUPS")
                    {
                        string groups = string.Join(",", chaty.Keys);
                        await Vidp(potik, $"GROUPS|{groups}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pomylka: {ex.Message}");
            }
            finally
            {
                // Vydalyayemo korystuvalnyкa
                if (!string.IsNullOrEmpty(user))
                {
                    online.Remove(user);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {user} vyjshov");
                }
                potik.Close();
                klient.Close();
            }
        }

        // Funkciya dlya vidpravky
        static async Task Vidp(NetworkStream potik, string vidpovid)
        {
            byte[] data = Encoding.UTF8.GetBytes(vidpovid);
            await potik.WriteAsync(data, 0, data.Length);
        }
    }
}