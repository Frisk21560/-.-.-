using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GuessNumber_Clientt_
{
    public partial class Form1 : Form
    {
        private Socket clientSocket;
        private IPAddress remoteIp;
        private int remotePort = 9999;

        public Form1()
        {
            InitializeComponent();
            btnSend.Enabled = false;
            remoteIp = IPAddress.Loopback;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Stvoryuyemo socket dlya z'yednannya
                clientSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp);

                MessageBox.Show("Pidklyucheno do ihry!");
                btnConnect.Enabled = false;
                btnSend.Enabled = true;
                txtGuess.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Pomylka",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (clientSocket == null)
                return;

            try
            {
                // Berem chyslo vid korystuvalnykov
                string message = txtGuess.Text;

                if (string.IsNullOrWhiteSpace(message))
                {
                    MessageBox.Show("Vvedit chyslo!");
                    return;
                }

                // Konvertuemo v bajty
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Adresa servera
                IPAddress remoteIp = IPAddress.Loopback;
                int remotePort = 9999;
                EndPoint remoteEp = new IPEndPoint(remoteIp, remotePort);

                // Vidpravlyayemo sprovu na server
                clientSocket.SendTo(data, 0, data.Length, SocketFlags.None, remoteEp);

                // Otrymuemo vidpovid vid servera
                byte[] buffer = new byte[4096];
                int bytesRead = clientSocket.ReceiveFrom(buffer, ref remoteEp);

                string answer = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // Vyvodym vidpovid
                txtAnswer.Text = answer;
                txtGuess.Clear();

                // Yakscho vidpovid - zupynyayemo
                if (answer.Contains("VGADAV"))
                {
                    btnSend.Enabled = false;
                    MessageBox.Show("Ty vygrau!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Pomylka",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
        }
    }
}