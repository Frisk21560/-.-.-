using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network_Homework_2
{
    public partial class Form1 : Form
    {
        private Socket socket;
        private int remotePort = 9090;
        private IPAddress remoteIp = IPAddress.Loopback;
        private string username = "";

        public Form1()
        {
            InitializeComponent();
            btnSend.Enabled = false;
            btnDisconnect.Enabled = false;
        }

        // Knopka pidklyuchennya
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            // Proveruemo chy vvedev korystuvalnyky username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vvedit vashe imya!");
                return;
            }

            username = txtUsername.Text;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEp = new IPEndPoint(remoteIp, remotePort);

            try
            {
                // Z'yednuemosya z serverom
                await socket.ConnectAsync(remoteEp);

                // Vidpravlyayemo username na server
                byte[] usernameData = Encoding.UTF8.GetBytes(username);
                await socket.SendAsync(usernameData);

                MessageBox.Show("Pidklyucheno do chatu!");

                // Zminyuyemo stan knopok
                btnConnect.Enabled = false;
                txtUsername.Enabled = false;
                btnSend.Enabled = true;
                btnDisconnect.Enabled = true;

                // Pochinayemo otrymuvalnyy task
                _ = Task.Run(() => ReceiveMessagesAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka pidklyuchennya: " + ex.Message);
            }
        }

        // Knopka vidpravky povidomlennya
        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (socket == null || !socket.Connected)
                return;

            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Napyshit' povidomlennya!");
                return;
            }

            string message = txtMessage.Text;
            byte[] data = Encoding.UTF8.GetBytes(message);

            try
            {
                // Vidpravlyayemo povidomlennya na server
                await socket.SendAsync(data);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka vidpravky: " + ex.Message);
            }
        }

        // Knopka vidklyuchennya
        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (socket == null || !socket.Connected)
                return;

            try
            {
                await socket.DisconnectAsync(false);
                socket.Close();
                socket = null;

                MessageBox.Show("Vidklyucheno!");

                btnConnect.Enabled = true;
                txtUsername.Enabled = true;
                btnSend.Enabled = false;
                btnDisconnect.Enabled = false;
                txtChat.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Proslushyuvannya povidomlen'
        private async Task ReceiveMessagesAsync()
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (socket != null && socket.Connected)
                {
                    int bytes = await socket.ReceiveAsync(buffer);
                    if (bytes == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytes);

                    // Vyvodym v hlavniy potik
                    this.Invoke((MethodInvoker)delegate
                    {
                        txtChat.AppendText(message + Environment.NewLine);
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Pomylka proslushyvannya: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (socket == null)
                return;

            if (socket.Connected)
            {
                socket.Close();
            }
        }
    }
}