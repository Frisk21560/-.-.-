using System.Diagnostics;

namespace Legacy_Code_Homework_5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Koly natiskayemo na knopku dlya vyboru papky
        private void btnVybrary_Click(object sender, EventArgs e)
        {
            // FolderBrowserDialog - tse vikno de korystuvacha mozhe vybrary papku z disku
            // Vin pokazuye derev'yanuyu strukturu papok i dozvol'yayet' vybrary odnu z nykh
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult rezultat = dialog.ShowDialog();

            if (rezultat == DialogResult.OK)
            {
                // Pokazuyemo shlyakh do vybranoyi papky v tekstboksku
                txtPapka.Text = dialog.SelectedPath;
            }
        }

        // Koly natiskayemo na knopku "Vykonaty poslidovno"
        private void btnPoslidovno_Click(object sender, EventArgs e)
        {
            // Pereviryyemo chy vvedeni dani - yakscho user ne vviv papku abo rozshyrennya, to ne robimo nychogo
            if (string.IsNullOrEmpty(txtPapka.Text) || string.IsNullOrEmpty(txtRozshyrennya.Text))
            {
                MessageBox.Show("Budh laska vvedity papku i rozshyrennya!");
                return;
            }

            // Ochyshuyemo pole z rezultatamy, shchob tam ne bulo staroho tekstu
            txtRezultaty.Clear();
            txtRezultaty.AppendText("Poslidovnyy poshuk...\r\n");

            // Stopwatch - eto sey chas dlya vimiryuvannya chasu vykonannya
            // StartNew() pochnayet vidlik vidrazu
            Stopwatch chasy = Stopwatch.StartNew();

            try
            {
                // Directory.GetFiles - znajdy usi fajly v papci
                // txtPapka.Text - shlyakh de shukaty
                // "*" + txtRozshyrennya.Text - shukaty fajly z takyam rozshyrennyam
                // SearchOption.AllDirectories - shukaty ne tilky v papci, ale i vo vsikh pidpapkakh
                string[] fajly = Directory.GetFiles(
                    txtPapka.Text,
                    "*" + txtRozshyrennya.Text,
                    SearchOption.AllDirectories
                );

                // chasy.Stop() - zanapynyayemo vidliky chasu
                chasy.Stop();

                // Vyvodym rezultaty
                // fajly.Length - kilkist' znajdenykh fajliv
                txtRezultaty.AppendText($"Znajdenout {fajly.Length} fajliv\r\n");

                // chasy.ElapsedMilliseconds - skilky millisekund mynulo z pochatku do teper
                txtRezultaty.AppendText($"Chas vykonannya: {chasy.ElapsedMilliseconds} ms\r\n\r\n");

                // Pokazuyemo kilka znajdenykh fajliv dlya priykladu
                txtRezultaty.AppendText("Znajdeni fajly:\r\n");

                // Math.Min(10, fajly.Length) - pokazuyemo tilky 10 fajliv, abo menshe yakscho yikh menshe
                for (int i = 0; i < Math.Min(10, fajly.Length); i++)
                {
                    // Path.GetFileName(fajly[i]) - berem tilky imen fajlu bez shlyakhu
                    // Napr. yakscho shlyakh "C:\Users\Documents\file.txt", to imen "file.txt"
                    txtRezultaty.AppendText($"{Path.GetFileName(fajly[i])}\r\n");
                }

                // Yakscho fajliv bilshe nizh 10, to pokazuyemo scho yikh shche ye
                if (fajly.Length > 10)
                {
                    txtRezultaty.AppendText($"...i shche {fajly.Length - 10} fajliv\r\n");
                }
            }
            // Robimo error yakscho nemaye dostupu do papky
            catch (UnauthorizedAccessException)
            {
                txtRezultaty.AppendText("Pomylka: Brak dostupu do deyakykh papok!\r\n");
            }
            // Robimo bud-yaku inshu osyblku
            catch (Exception osybka)
            {
                txtRezultaty.AppendText($"Pomylka: {osybka.Message}\r\n");
            }
        }

        // Koly natiskayemo na knopku "Vykonaty paralelno"
        private void btnParalel_Click(object sender, EventArgs e)
        {
            // Pereviryyemo chy vvedeni dani
            if (string.IsNullOrEmpty(txtPapka.Text) || string.IsNullOrEmpty(txtRozshyrennya.Text))
            {
                MessageBox.Show("Budh laska vvedity papku i rozshyrennya!");
                return;
            }

            // Ochyshuyemo pole z rezultatamy
            txtRezultaty.Clear();
            txtRezultaty.AppendText("Paralel'nyy poshuk...\r\n");

            Stopwatch chasy = Stopwatch.StartNew();

            try
            {
                // Spochatku berem VSI fajly v papci bez filtruvannya
                // Tse treba robyty, bo pislya mi filtruemo yih paralelno
                string[] vsi_fajly = Directory.GetFiles(
                    txtPapka.Text,
                    "*",
                    SearchOption.AllDirectories
                );

                // AsParallel() - pretvoryuye zvychnuy LINQ zapyt u paralelnu
                // Tse oznachaye scho chleny zapytu (Where v nashomu vypadku) vykonuvatymetsya na kilkokh radkah odnochasno
                // Where(f => f.EndsWith(txtRozshyrennya.Text)) - filtruemo fajly tak shcho zalishat'sya tilky ti sho zakinchuvatysya na neobkhidnym rozshyrennyam
                // .ToList() - peretvoryuyet' rezultat u List<string>, shchob mi mohly z nym robyty
                var fajly = vsi_fajly.AsParallel()
                    .Where(f => f.EndsWith(txtRozshyrennya.Text))
                    .ToList();

                chasy.Stop();

                // Vyvodym rezultaty - fajly.Count teper bo tse List, ne masiv
                txtRezultaty.AppendText($"Znajdenout {fajly.Count} fajliv\r\n");
                txtRezultaty.AppendText($"Chas vykonannya: {chasy.ElapsedMilliseconds} ms\r\n\r\n");

                // Pokazuyemo kilka znajdenykh fajliv
                txtRezultaty.AppendText("Znajdeni fajly:\r\n");
                for (int i = 0; i < Math.Min(10, fajly.Count); i++)
                {
                    txtRezultaty.AppendText($"{Path.GetFileName(fajly[i])}\r\n");
                }

                if (fajly.Count > 10)
                {
                    txtRezultaty.AppendText($"...i shche {fajly.Count - 10} fajliv\r\n");
                }
            }
            catch (UnauthorizedAccessException)
            {
                txtRezultaty.AppendText("Pomylka: Brak dostupu do deyakykh papok!\r\n");
            }
            catch (Exception osybka)
            {
                txtRezultaty.AppendText($"Pomylka: {osybka.Message}\r\n");
            }
        }

        // Koly natiskayemo na knopku "Porivnyaty"
        // Tsey metod vykonuyet oba metody (poslidovnyy i paralel'nyy) i pokazuyet' rezultaty
        private void btnPorivnyaty_Click(object sender, EventArgs e)
        {
            // Pereviryyemo chy vvedeni dani
            if (string.IsNullOrEmpty(txtPapka.Text) || string.IsNullOrEmpty(txtRozshyrennya.Text))
            {
                MessageBox.Show("Budh laska vvedity papku i rozshyrennya!");
                return;
            }

            // Ochyshuyemo pole z rezultatamy
            txtRezultaty.Clear();

            // Zminnuy dlya zberihannya rezultativ
            long vremya_poslidovna = 0;
            long vremya_paralel = 0;
            int kilkist_fajliv_poslidovna = 0;
            int kilkist_fajliv_paralel = 0;

            try
            {
                // POSLIDOVNIY POSHUK
                // Tsey chastyna kodu vykonuyet poshuk bez paralelnosti
                Stopwatch chasy = Stopwatch.StartNew();

                // Directory.GetFiles z filtrom rozshyrennya - Windows sam filtruyet fajly za rozshyrennyam
                // Tse shvydko bo robyty na rivni operatyvnoyi systemy
                string[] fajly_poslidovna = Directory.GetFiles(
                    txtPapka.Text,
                    "*" + txtRozshyrennya.Text,
                    SearchOption.AllDirectories
                );

                chasy.Stop();
                // Zberigayemo rezultaty
                vremya_poslidovna = chasy.ElapsedMilliseconds;
                kilkist_fajliv_poslidovna = fajly_poslidovna.Length;

                // PARALELNIY POSHUK
                // Tsey chastyna vykonuyet poshuk z paralelnistyu
                chasy = Stopwatch.StartNew();

                // Spochatku berem VSI fajly
                string[] vsi_fajly = Directory.GetFiles(
                    txtPapka.Text,
                    "*",
                    SearchOption.AllDirectories
                );

                // Paralelno filtrytymo yikh
                // AsParallel() - rozpodil'ayemy robotu na kilka threadiv
                // Kozhnyy thread obroblyaet' chastyny masyvu odnochasno
                var fajly_paralel = vsi_fajly.AsParallel()
                    .Where(f => f.EndsWith(txtRozshyrennya.Text))
                    .ToList();

                chasy.Stop();
                // Zberigayemo rezultaty
                vremya_paralel = chasy.ElapsedMilliseconds;
                kilkist_fajliv_paralel = fajly_paralel.Count;

                //VYVODYM PORIVNYANNYA
                txtRezultaty.AppendText("PORIVNYANNYA\r\n\r\n");

                txtRezultaty.AppendText("Poslidovniy poshuk:\r\n");
                txtRezultaty.AppendText($"Fajliv znajdeno: {kilkist_fajliv_poslidovna}\r\n");
                txtRezultaty.AppendText($"Chas: {vremya_poslidovna} ms\r\n\r\n");

                txtRezultaty.AppendText("Paralelniy poshuk:\r\n");
                txtRezultaty.AppendText($"Fajliv znajdeno: {kilkist_fajliv_paralel}\r\n");
                txtRezultaty.AppendText($"Chas: {vremya_paralel} ms\r\n\r\n");

                // Vyvodym na skilky procentiv odyn shvydshe nizh drugryy
                // Formula: (pomichky - novyy) / staryy * 100
                // Yakscho rezultat pozytyvnyy, to novyy shvydshe, yakscho negatyvnyy - to staryy shvydshe
                double vidkhylennya = ((double)(vremya_poslidovna - vremya_paralel) / vremya_poslidovna) * 100;

                if (vremya_paralel < vremya_poslidovna)
                {
                    // Paralelniy poshuk shvydshe
                    txtRezultaty.AppendText($"Paralelniy poshuk shvydshe na {(int)vidkhylennya}%");
                }
                else
                {
                    // Poslidovnyy poshuk shvydshe
                    // Math.Abs() - prosto berbryty znaky na pozytyvnuy, shchob bylo prosto chytaty
                    txtRezultaty.AppendText($"Poslidovniy poshuk shvydshe na {(int)Math.Abs(vidkhylennya)}%");
                }
            }
            catch (UnauthorizedAccessException)
            {
                txtRezultaty.AppendText("Pomylka: Brak dostupu do deyakykh papok!\r\n");
            }
            catch (Exception osybka)
            {
                txtRezultaty.AppendText($"Pomylka: {osybka.Message}\r\n");
            }
        }
    }
}