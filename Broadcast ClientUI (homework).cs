using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Broadcast_ClientUI__homework_
{
    public partial class Form1 : Form
    {
        private UdpClient udpClient;
        private IPAddress serverIp;
        const int serverPort = 9050;
        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();
            btnDisconnect.Enabled = false;
            btnSubscribe.Enabled = false;
        }

        // Knopka pidklyuchennya
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Pereviriayemo chy vvedev IP servera
                if (string.IsNullOrWhiteSpace(txtServerIp.Text))
                {
                    MessageBox.Show("Vvedit IP servera!");
                    return;
                }

                serverIp = IPAddress.Parse(txtServerIp.Text);

                // Reyestruemo klienta
                RegisterWithServer();

                isConnected = true;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnSubscribe.Enabled = true;
                txtServerIp.Enabled = false;

                MessageBox.Show("Pidklyucheno do servera!");

                // Zapuskayemo potik dlya otrymannya povidomlen'
                System.Threading.Thread receiveThread = new System.Threading.Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka pidklyuchennya: " + ex.Message);
            }
        }

        // Funkciya dlya reyestracyi
        private void RegisterWithServer()
        {
            try
            {
                using UdpClient client = new UdpClient();
                IPEndPoint serverEp = new IPEndPoint(serverIp, serverPort);

                // Vidpravlyayemo povidomlennya reyestracyi
                string registerMessage = "REGISTER";
                byte[] data = Encoding.UTF8.GetBytes(registerMessage);
                client.Send(data, serverEp);

                AppendMessage("Reyestraciya na serveri...", ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka reyestracyi: " + ex.Message);
            }
        }

        // Knopka dlya pidpysky na typy
        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            try
            {
                // Zibrrayemo vybranyy typy
                string selectedTypes = "";

                if (chbNews.Checked)
                    selectedTypes += "1,";
                if (chbReminder.Checked)
                    selectedTypes += "2,";
                if (chbEntertainment.Checked)
                    selectedTypes += "3,";

                // Vydalyayemo poslednyu komu
                if (selectedTypes.EndsWith(","))
                    selectedTypes = selectedTypes.Substring(0, selectedTypes.Length - 1);

                if (string.IsNullOrWhiteSpace(selectedTypes))
                {
                    MessageBox.Show("Vybir hocha by odyn typ!");
                    return;
                }

                // Vidpravlyayemo pidpysku na server
                using UdpClient client = new UdpClient();
                IPEndPoint serverEp = new IPEndPoint(serverIp, serverPort);

                string subscriptionMessage = $"SUBSCRIBE:{selectedTypes}";
                byte[] data = Encoding.UTF8.GetBytes(subscriptionMessage);
                client.Send(data, serverEp);

                MessageBox.Show("Pidpyska vyslana!");
                AppendMessage("Pidpyska onovlena!", ConsoleColor.Cyan);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka pidpysky: " + ex.Message);
            }
        }

        // Funkciya dlya otrymannya povidomlen'
        private void ReceiveMessages()
        {
            try
            {
                udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

                while (isConnected)
                {
                    // Otrymuemo povidomlennya
                    byte[] buffer = udpClient.Receive(ref remoteEp);
                    string message = Encoding.UTF8.GetString(buffer);

                    // Vyvodymî povidomlennya v formi
                    ConsoleColor color = message.Contains("[Emergency]") ? ConsoleColor.Red : ConsoleColor.Green;

                    this.Invoke((MethodInvoker)delegate
                    {
                        AppendMessage($"[{DateTime.Now:HH:mm:ss}] {message}", color);
                    });
                }
            }
            catch (Exception ex)
            {
                if (isConnected)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Pomylka pry otrymannyu: " + ex.Message);
                    });
                }
            }
        }

        // Funkciya dlya dodavannya tekstu do TextBox
        private void AppendMessage(string message, ConsoleColor color)
        {
            txtMessages.AppendText(message + Environment.NewLine);
            txtMessages.ScrollToCaret();
        }

        // Knopka vidklyuchennya
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                isConnected = false;

                if (udpClient != null)
                {
                    udpClient.Close();
                }

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnSubscribe.Enabled = false;
                txtServerIp.Enabled = true;

                AppendMessage("Vidklyucheno vid servera!", ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka pry vidklyuchennyu: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isConnected = false;
            if (udpClient != null)
            {
                udpClient.Close();
            }
        }
    }
}