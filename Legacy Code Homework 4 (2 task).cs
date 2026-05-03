using System.Diagnostics;

namespace Legacy_Code_Homework_4__2_task_
{
    public partial class Form1 : Form
    {
        private Stopwatch chasy;
        private int kilkist_opracovanykh_fajliv = 0;

        public Form1()
        {
            InitializeComponent();
        }

        // Koly natiskayemo na knopku "Vybrary papku"
        private void btnVybrary_Click(object sender, EventArgs e)
        {
            // Vidkryvayet'sya dialog dlya vyboru papky
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult rezultat = dialog.ShowDialog();

            // Yakscho korystuvacha vybrav papku, to pokazuyemo shlyakh
            if (rezultat == DialogResult.OK)
            {
                txtPapka.Text = dialog.SelectedPath;
            }
        }

        // Koly natiskayemo na knopku "Pochaty shifruvannya"
        private async void btnStart_Click(object sender, EventArgs e)
        {
            // Pereviryyemo chy vybrana papka
            if (string.IsNullOrEmpty(txtPapka.Text))
            {
                MessageBox.Show("Budh laska vybery papku!");
                return;
            }

            btnStart.Enabled = false;
            txtStatus.Clear();
            kilkist_opracovanykh_fajliv = 0;

            // Pochynayet'sya vidliky chasu
            chasy = Stopwatch.StartNew();

            // Znahodym usi txt fajly
            string[] fajly = Directory.GetFiles(txtPapka.Text, "*.txt", SearchOption.AllDirectories);

            if (fajly.Length == 0)
            {
                txtStatus.AppendText("Txt fajliv ne znaydenout!");
                btnStart.Enabled = true;
                return;
            }

            txtStatus.AppendText($"Znaydenout {fajly.Length} fajliv. Pochynayet'sya shifruvannya...\r\n\r\n");

            // Dlya kozhnoho fajlu zapuskayemy shifruvannya u threadi
            foreach (string fajl in fajly)
            {
                // ThreadPool zapuskaye metod u fonovomu threadi
                ThreadPool.QueueUserWorkItem(async (state) =>
                {
                    try
                    {
                        // Vyvodym u formi scho pochatyy shifruvannya
                        Invoke((Action)(() =>
                        {
                            txtStatus.AppendText($"Pochatyy shifruvannya: {Path.GetFileName(fajl)}\r\n");
                            txtStatus.Focus();
                            txtStatus.Select(txtStatus.TextLength, 0);
                            txtStatus.ScrollToCaret();
                        }));

                        // Chytayemy zmist fajlu
                        string zmist = File.ReadAllText(fajl);

                        // Shyfruyemy zmist
                        string zshyfrovanyy = ShyfrUvannyaBazhko(zmist);

                        // Tvorym novyy fajl z shyfrovanyym zmistom
                        string noviyfajl = fajl.Replace(".txt", "_zashyfrovannyy.txt");

                        // Pysemy u novyy fajl asynxronno
                        await File.WriteAllTextAsync(noviyfajl, zshyfrovanyy);

                        // Zbilshuyemy kilkist' opracovanykh fajliv
                        kilkist_opracovanykh_fajliv++;

                        // Vyvodym u formi scho shifruvannya zakinchylos
                        Invoke((Action)(() =>
                        {
                            txtStatus.AppendText($"Zakinchene shifruvannya: {Path.GetFileName(fajl)}\r\n");
                            txtStatus.Focus();
                            txtStatus.Select(txtStatus.TextLength, 0);
                            txtStatus.ScrollToCaret();
                        }));
                    }
                    catch (Exception osybka)
                    {
                        Invoke((Action)(() =>
                        {
                            txtStatus.AppendText($"Osybka: {osybka.Message}\r\n");
                        }));
                    }
                });
            }

            // Chekayemo troxu dlya vykonannya vsikh zadach
            System.Threading.Thread.Sleep(5000);

            // Zanapynyayemo vidliky chasu
            chasy.Stop();

            // Vyvodym statystiku
            Invoke((Action)(() =>
            {
                txtStatus.AppendText($"\r\nStatystyka:\r\n");
                txtStatus.AppendText($"Opracovano fajliv: {kilkist_opracovanykh_fajliv}\r\n");
                txtStatus.AppendText($"Chasu vytrachenout: {chasy.ElapsedMilliseconds} ms\r\n");
                btnStart.Enabled = true;
            }));
        }

        // Metod dlya shyfruvanny
        static string ShyfrUvannyaBazhko(string tekst)
        {
            string shifrovanyy = "";

            foreach (char simvol in tekst)
            {
                if (char.IsLetter(simvol))
                {
                    shifrovanyy += (char)(simvol + 3);
                }
                else
                {
                    shifrovanyy += simvol;
                }
            }

            return shifrovanyy;
        }
    }
}