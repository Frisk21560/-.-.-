namespace Legacy_Code_Homework_3._2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Koly natiskayemo na knopku, zapuskayemo prygotuvannya snidanku asynxronno
        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            // Ochyshuyemo tekstboks pered tym yak pysaty novyy status
            txtStatus.Clear();
            progressBar1.Value = 0;

            // Gotuyemo kavu, chekayemo 2 sekundy i vyvodym status
            txtStatus.AppendText("Vlyvayemo kavu v chashku...\r\n");
            await Task.Delay(2000);
            txtStatus.AppendText("Kava hotova!\r\n");
            progressBar1.Value = 20;

            // Gotuyemo yajcya, tse zajmaye bilshe chasu tomu zatryamka dovsha
            txtStatus.AppendText("Beremo yajcya...\r\n");
            await Task.Delay(1000);
            txtStatus.AppendText("Klayemo yajcya na skvorodu...\r\n");
            await Task.Delay(3000);
            txtStatus.AppendText("Yajcya hotovi!\r\n");
            progressBar1.Value = 40;

            // Gotuyemo bekon, yogo naydovshe hotuvaty z usix
            txtStatus.AppendText("Klayemo bekon na skvorodu...\r\n");
            await Task.Delay(4000);
            txtStatus.AppendText("Bekon hotoviy!\r\n");
            progressBar1.Value = 60;

            // Gotuyemo tost, spochatku beremo hlib potim chekayemo poky vin pryhorit
            txtStatus.AppendText("Beremo hlib...\r\n");
            await Task.Delay(500);
            txtStatus.AppendText("Klayemo v toster...\r\n");
            await Task.Delay(2000);
            txtStatus.AppendText("Tost hotoviy!\r\n");
            progressBar1.Value = 80;

            // Nalyvayemo sok v sklyanku
            txtStatus.AppendText("Nalyvayemo sok...\r\n");
            await Task.Delay(1500);
            txtStatus.AppendText("Sok hotoviy!\r\n");
            progressBar1.Value = 100;

            // Koly vse hotovo, pokazuyemo povidomlennya scho vse zakinchylos
            txtStatus.AppendText("\r\nSnidanok povnistyu hotoviy!");

            btnStart.Enabled = true;
        }
    }
}