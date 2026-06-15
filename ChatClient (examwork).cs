using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Network_Programing_Exam_work
{
    public partial class Form1 : Form
    {
        private TcpClient sock;
        private IPAddress srvip = IPAddress.Loopback;
        private int srvport = 9090;
        private string imen = "";
        private List<string> groupy = new List<string>();

        public Form1()
        {
            InitializeComponent();
            btnLog.Enabled = true;
            btnReg.Enabled = true;
            tabControl1.Enabled = false;
        }

        // Knopka reyestracyi
        private async void btnReg_Click(object sender, EventArgs e)
        {
            // Pereviriayemo vvedennya
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Vvedit user i pass!");
                return;
            }

            try
            {
                // Pidklyuchaymosya do servera
                sock = new TcpClient();
                await sock.ConnectAsync(srvip, srvport);

                // Vidpravlyayemo zapyt na reyestracyu
                string zapyt = $"REGISTER|{txtUser.Text}|{txtPass.Text}";
                await Vidpr(zapyt);

                // Chekaymo vidpovid
                string vidp = await Otry();
                if (vidp.StartsWith("OK"))
                {
                    MessageBox.Show("Reyestraciya OK!");
                    sock.Close();
                }
                else
                {
                    MessageBox.Show("Pomylka: " + vidp.Replace("ERROR|", ""));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Knopka vkhodu
        private async void btnLog_Click(object sender, EventArgs e)
        {
            // Pereviriayemo vvedennya
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Vvedit user i pass!");
                return;
            }

            try
            {
                // Pidklyuchaymosya do servera
                sock = new TcpClient();
                await sock.ConnectAsync(srvip, srvport);

                // Vidpravlyayemo zapyt na vkhid
                string zapyt = $"LOGIN|{txtUser.Text}|{txtPass.Text}";
                await Vidpr(zapyt);

                // Chekaymo vidpovid
                string vidp = await Otry();
                if (vidp.StartsWith("OK"))
                {
                    imen = txtUser.Text;
                    tabControl1.Enabled = true;
                    grpAuth.Enabled = false;
                    MessageBox.Show("Vkhid OK!");

                    // Zapuskayemo potik dlya slukhayennya
                    _ = Task.Run(() => Slukh());
                }
                else
                {
                    MessageBox.Show("Pomylka: " + vidp.Replace("ERROR|", ""));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Knopka dlya pryvat povidomlennya
        private async void btnSendPriv_Click(object sender, EventArgs e)
        {
            // Pereviriayemo vvedennya
            if (string.IsNullOrWhiteSpace(txtPrivUser.Text) || string.IsNullOrWhiteSpace(txtPrivMsg.Text))
            {
                MessageBox.Show("Vvedit user i msg!");
                return;
            }

            try
            {
                // Formuemo i vidpravlyayemo zapyt
                string zapyt = $"PRIVATE|{txtPrivUser.Text}|{txtPrivMsg.Text}";
                await Vidpr(zapyt);

                // Vyvodymî u chat
                txtPrivChat.AppendText($"[{imen}]: {txtPrivMsg.Text}\n");
                txtPrivMsg.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Knopka dlya stvorennya grupy
        private async void btnMkGroup_Click(object sender, EventArgs e)
        {
            // Pereviriayemo vvedennya
            if (string.IsNullOrWhiteSpace(txtGroupName.Text))
            {
                MessageBox.Show("Vvedit imya grupy!");
                return;
            }

            try
            {
                // Vidpravlyayemo zapyt na stvorennya grupy
                string zapyt = $"MKGROUP|{txtGroupName.Text}";
                await Vidpr(zapyt);

                // Chekaymo vidpovid
                string vidp = await Otry();
                if (vidp.StartsWith("OK"))
                {
                    groupy.Add(txtGroupName.Text);
                    cbGroups.Items.Add(txtGroupName.Text);
                    MessageBox.Show("Grupa OK!");
                    txtGroupName.Clear();
                }
                else
                {
                    MessageBox.Show("Pomylka: " + vidp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Knopka dlya priyednannya do grupy
        private async void btnJoinGr_Click(object sender, EventArgs e)
        {
            // Pereviriayemo vybir grupy
            if (cbGroups.SelectedItem == null)
            {
                MessageBox.Show("Vybir grupu!");
                return;
            }

            try
            {
                // Vidpravlyayemo zapyt na priyednannya
                string gname = cbGroups.SelectedItem.ToString();
                string zapyt = $"JOINGROUP|{gname}";
                await Vidpr(zapyt);

                // Chekaymo vidpovid
                string vidp = await Otry();
                if (vidp.StartsWith("OK"))
                {
                    MessageBox.Show("V grupi!");
                }
                else
                {
                    MessageBox.Show("Pomylka: " + vidp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Knopka dlya vidpravky u grupu
        private async void btnSendGr_Click(object sender, EventArgs e)
        {
            // Pereviriayemo vybir grupy i povidomlennya
            if (cbGroups.SelectedItem == null || string.IsNullOrWhiteSpace(txtGroupMsg.Text))
            {
                MessageBox.Show("Vybir grupu i msg!");
                return;
            }

            try
            {
                // Formuemo i vidpravlyayemo zapyt
                string gname = cbGroups.SelectedItem.ToString();
                string zapyt = $"GROUPMSG|{gname}|{txtGroupMsg.Text}";
                await Vidpr(zapyt);

                // Vyvodymî u chat
                txtGrChat.AppendText($"[{imen}]: {txtGroupMsg.Text}\n");
                txtGroupMsg.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Knopka dlya onovlennya spysku users
        private async void btnRefUsr_Click(object sender, EventArgs e)
        {
            try
            {
                // Vidpravlyayemo zapyt
                await Vidpr("GETUSERS");
                string vidp = await Otry();

                // Vyvodymî list
                if (vidp.StartsWith("USERS|"))
                {
                    string users = vidp.Replace("USERS|", "");
                    txtUsers.Text = users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Knopka dlya onovlennya spysku grup
        private async void btnRefGr_Click(object sender, EventArgs e)
        {
            try
            {
                // Vidpravlyayemo zapyt
                await Vidpr("GETGROUPS");
                string vidp = await Otry();

                // Onovlyayemo list
                if (vidp.StartsWith("GROUPS|"))
                {
                    string groups = vidp.Replace("GROUPS|", "");
                    cbGroups.Items.Clear();
                    foreach (var gr in groups.Split(','))
                    {
                        if (!string.IsNullOrWhiteSpace(gr))
                            cbGroups.Items.Add(gr);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pomylka: " + ex.Message);
            }
        }

        // Funkciya dlya vidpravky zaptytu
        private async Task Vidpr(string zapyt)
        {
            byte[] data = Encoding.UTF8.GetBytes(zapyt);
            await sock.GetStream().WriteAsync(data, 0, data.Length);
        }

        // Funkciya dlya otrymannya vidpovidi
        private async Task<string> Otry()
        {
            byte[] buf = new byte[1024];
            int recv = await sock.GetStream().ReadAsync(buf, 0, buf.Length);
            return Encoding.UTF8.GetString(buf, 0, recv);
        }

        // Potik dlya prosluhhovuvannya serveriv
        private async Task Slukh()
        {
            try
            {
                byte[] buf = new byte[1024];

                while (sock.Connected)
                {
                    int recv = await sock.GetStream().ReadAsync(buf, 0, buf.Length);
                    if (recv == 0) break;

                    string vidp = Encoding.UTF8.GetString(buf, 0, recv);

                    // Obroblyuyemo vidpovid u hlavnomu potiku
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Pryvat povidomlennya
                        if (vidp.StartsWith("PRIV|"))
                        {
                            string[] ch = vidp.Split('|');
                            string vid = ch[1];
                            string msg = ch[2];
                            txtPrivChat.AppendText($"[{vid}]: {msg}\n");
                        }
                        // Grupy povidomlennya
                        else if (vidp.StartsWith("GMSG|"))
                        {
                            string[] ch = vidp.Split('|');
                            string gr = ch[1];
                            string vid = ch[2];
                            string msg = ch[3];
                            txtGrChat.AppendText($"[{gr}] {vid}: {msg}\n");
                        }
                    });
                }
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Zakryuyemo z'yednannya
            if (sock != null && sock.Connected)
            {
                sock.Close();
            }
        }
    }
}