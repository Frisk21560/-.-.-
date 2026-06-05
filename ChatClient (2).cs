using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UPD__homework_
{
    public partial class Form1 : Form
    {
        private TcpClient socket;
        private int remotePort = 9090;
        private IPAddress remoteIp = IPAddress.Loopback;
        private string username = "";
        private string currentChatRoom = "General";

        public Form1()
        {
            InitializeComponent();
            btnSend.Enabled = false;
            btnJoinChat.Enabled = false;
            btnDisconnect.Enabled = false;
            cbChatRooms.Items.Add("General");
            cbChatRooms.Items.Add("Random");
            cbChatRooms.Items.Add("Gaming");
            cbChatRooms.Items.Add("Tech");
            cbChatRooms.SelectedIndex = 0;
        }

        // Knopka pidklyuchennya
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            // Proveruemo username
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
                btnJoinChat.Enabled = true;
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

        // Knopka peremikannya mizh chatamy
        private async void btnJoinChat_Click(object sender, EventArgs e)
        {
            if (socket == null || !socket.Connected)
                return;

            string selectedChat = cbChatRooms.SelectedItem.ToString();

            if (selectedChat == currentChatRoom)
            {
                MessageBox.Show("Vy vzhe u tsyomu chati!");
                return;
            }

            currentChatRoom = selectedChat;

            // Vidpravlyayemo komandy na server
            string command = $"/join {selectedChat}";
            byte[] data = Encoding.UTF8.GetBytes(command);

            try
            {
                await socket.SendAsync(data);
                lblCurrentChat.Text = $"Potochnyy chat: {currentChatRoom}";
                txtChat.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
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
                btnJoinChat.Enabled = false;
                btnDisconnect.Enabled = false;
                txtChat.Clear();
                lblCurrentChat.Text = "Potochnyy chat: Bud' yakisnyy";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Proslushyuvannya povidomlen
        private async Task ReceiveMessagesAsync()
        {
            try
            {
                byte[] buffer = new byte[4096];

                while (socket != null && socket.Connected)
                {
                    int bytes = await socket.ReceiveAsync(buffer);
                    if (bytes == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytes);

                    // Yakscho istoriya
                    if (message.StartsWith("HISTORY:"))
                    {
                        string historyData = message.Substring(8);
                        string[] historyMessages = historyData.Split('|');

                        this.Invoke((MethodInvoker)delegate
                        {
                            txtChat.Clear();
                            foreach (var msg in historyMessages)
                            {
                                if (!string.IsNullOrWhiteSpace(msg))
                                {
                                    txtChat.AppendText(msg + Environment.NewLine);
                                }
                            }
                        });
                    }
                    else
                    {
                        // Zvychaynyy chat
                        this.Invoke((MethodInvoker)delegate
                        {
                            txtChat.AppendText(message + Environment.NewLine);
                        });
                    }
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