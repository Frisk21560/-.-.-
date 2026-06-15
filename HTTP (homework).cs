using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace HTTP
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Stvoryuyemo HttpListener
            using HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine("Server zapushcheno na http://localhost:8080/");
            Console.WriteLine("Nazhmi Ctrl+C dlya zupynennya\n");

            using var cts = new System.Threading.CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    // Otrymuemo zapyt vid klienta
                    HttpListenerContext context = await listener.GetContextAsync();

                    // Obroblyayemo zapyt u okremomu potiku
                    _ = Task.Run(() => ObrobkyZaptytuAsync(context));
                }
            }
            catch (HttpListenerException ex) when (cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("Server zavershuye robotu");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Pomylka: " + ex.ToString());
            }
        }

        // Obrobka zaptytu
        private static async Task ObrobkyZaptytuAsync(HttpListenerContext context)
        {
            try
            {
                // Poluchayemo shlyakh zaptytu
                string path = context.Request.Url.AbsolutePath;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Novyy zapyt: {path}");

                string vidpovid = "";
                string contentType = "text/html; charset=utf-8";

                // Vyznachayemo yaku storunku vydaty
                if (path == "/" || path == "")
                {
                    vidpovid = GetGlavnaStoronka();
                }
                else if (path == "/autobiography")
                {
                    vidpovid = GetBiografiaStoronka();
                }
                else if (path == "/fav_countries")
                {
                    vidpovid = GetKrayinyStoronka();
                }
                else if (path == "/pc_data")
                {
                    vidpovid = GetKomputerDani();
                    contentType = "application/json; charset=utf-8";
                }
                else
                {
                    vidpovid = Get404Storonka();
                    context.Response.StatusCode = 404;
                }

                // Vidpravlyayemo vidpovid
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(vidpovid);
                using HttpListenerResponse response = context.Response;
                response.ContentLength64 = buffer.Length;
                response.ContentType = contentType;

                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Pomylka obrobky: " + ex.Message);
            }
        }

        // Hlavna storonka
        private static string GetGlavnaStoronka()
        {
            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Hlavna storonka</title>
                    <style>
                        body { font-family: Arial; margin: 40px; }
                        h1 { color: #333; }
                        a { display: block; margin: 10px 0; }
                    </style>
                </head>
                <body>
                    <h1>Pryvit!</h1>
                    <p>Tse moya osobysta veb-storonka</p>
                    <h2>Posilannya:</h2>
                    <a href='/autobiography'>Moya biografiya</a>
                    <a href='/fav_countries'>Moyy ulubleni krainy</a>
                    <a href='/pc_data'>Dani pro komp'yuter</a>
                </body>
                </html>";
        }

        // Biografiya storonka
        private static string GetBiografiaStoronka()
        {
            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Biografiya</title>
                    <style>
                        body { font-family: Arial; margin: 40px; }
                        h1 { color: #333; }
                        a { color: #0066cc; }
                    </style>
                </head>
                <body>
                    <h1>Moya Biografiya</h1>
                    <p>Mene zvuty Ivan. YA student IT spetsialnosti.</p>
                    <p>Lyubyyu prograuvannya ta rozvytok veb-zastosunkiv.</p>
                    <p>V moho vilnomy chasi hodiuyu zanutytysa sportom ta chytaty knygy.</p>
                    <h2>Navychky:</h2>
                    <ul>
                        <li>C# ta .NET</li>
                        <li>Web development</li>
                        <li>Databazy</li>
                    </ul>
                    <br>
                    <a href='/'>Nazad na glavnu</a>
                </body>
                </html>";
        }

        // Ulubleni krainy storonka
        private static string GetKrayinyStoronka()
        {
            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Ulubleni krainy</title>
                    <style>
                        body { font-family: Arial; margin: 40px; }
                        h1 { color: #333; }
                        ul { line-height: 1.8; }
                        a { color: #0066cc; }
                    </style>
                </head>
                <body>
                    <h1>Moyy Ulubleni Krainy</h1>
                    <ul>
                        <li>Yaponiyu - krasyvi pejzazhy ta kultura</li>
                        <li>Shveycariya - pryrodna krasa ta spokiy</li>
                        <li>Niderlandy - mistechka ta velosiped</li>
                        <li>Ispaniyu - teplo ta morye</li>
                        <li>Frantsiyu - mystecztvo ta arkhitektura</li>
                    </ul>
                    <br>
                    <a href='/'>Nazad na glavnu</a>
                </body>
                </html>";
        }

        // Dani pro komp'yuter u JSON formati
        private static string GetKomputerDani()
        {
            var dani = new
            {
                komputernomIm = Environment.MachineName,
                operacijnaSystema = Environment.OSVersion.VersionString,
                procesoriv = Environment.ProcessorCount,
                RAM_GB = GC.GetTotalMemory(false) / (1024 * 1024 * 1024),
                username = Environment.UserName,
                systemnyFolder = Environment.SystemDirectory,
                networkCards = GetNetworkInterfaces()
            };

            string json = JsonSerializer.Serialize(dani, new JsonSerializerOptions { WriteIndented = true });
            return json;
        }

        // Poluchayemo informaciyu pro merezhevy interfeysy
        private static string GetNetworkInterfaces()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            return $"Znayydeno {interfaces.Length} merezhevih interfeyiv";
        }

        // 404 storonka
        private static string Get404Storonka()
        {
            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <title>404 - Ne znayydeno</title>
                    <style>
                        body { font-family: Arial; margin: 40px; text-align: center; }
                        h1 { color: #cc0000; }
                        a { color: #0066cc; }
                    </style>
                </head>
                <body>
                    <h1>404 - Storonka ne znayydena</h1>
                    <p>Shukana storonka ne isnuye</p>
                    <a href='/'>Graty na glavnu</a>
                </body>
                </html>";
        }
    }
}