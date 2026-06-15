namespace HTTP__practise_work_
{
    public partial class Form1 : Form
    {
        private HttpClient httpClient;

        public Form1()
        {
            InitializeComponent();
            httpClient = new HttpClient();
            btnSend.Enabled = true;
        }

        // Knopka dlya nadsilannya zaptytu
        private async void btnSend_Click(object sender, EventArgs e)
        {
            // Pereviriayemo chy vvedev URI
            if (string.IsNullOrWhiteSpace(txtUri.Text))
            {
                MessageBox.Show("Vvedit URI!");
                return;
            }

            try
            {
                // Zminyuyemo stan knopky
                btnSend.Enabled = false;
                btnSend.Text = "Zaluvannya...";

                // Poluchayemo URI vid textboxa
                string uri = txtUri.Text;

                // Pereviriayemo chy yest' protokol
                if (!uri.StartsWith("http://") && !uri.StartsWith("https://"))
                {
                    uri = "http://" + uri;
                }

                // Vidpravlyayemo zaptyty
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Zaptyty do: {uri}");
                HttpResponseMessage response = await httpClient.GetAsync(uri);

                // Chytayemo vidpovid
                string htmlContent = await response.Content.ReadAsStringAsync();

                // Vyvodymî v textbox
                txtResult.Text = htmlContent;

                // Pokazuyemo status
                lblStatus.Text = $"Status: {(int)response.StatusCode} {response.ReasonPhrase}";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Pomylka: {ex.Message}", "Pomylka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Status: Pomylka";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Pomylka: {ex.Message}", "Pomylka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Status: Pomylka";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                // Povernuyemo stan knopky
                btnSend.Enabled = true;
                btnSend.Text = "Vyslaty zapyt";
            }
        }

        // Knopka dlya ochistky danykh
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUri.Clear();
            txtResult.Clear();
            lblStatus.Text = "Status: Hotovyy";
            lblStatus.ForeColor = System.Drawing.Color.Black;
            txtUri.Focus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Zakryuyemo HttpClient
            if (httpClient != null)
            {
                httpClient.Dispose();
            }
        }
    }
}