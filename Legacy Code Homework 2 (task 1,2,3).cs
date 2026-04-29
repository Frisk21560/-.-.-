using System;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ProccessApp
{
    public partial class Form3 : Form
    {
        // Таймер для оновленння
        private Timer zminna_timer = new Timer();

        // ListBox для списку процесів
        private ListBox zminna_listbox_procesiv;

        // TextBox для деталей про процес
        private TextBox zminna_textbox_info;

        // TextBox для введення інтервалу
        private TextBox zminna_textbox_interval;

        // Кнопка для старту
        private Button zminna_button_start;

        // Кнопка для вбивання процесу (Kill)
        private Button zminna_button_kill;

        // Напис
        private Label zminna_label_interval;

        public Form3()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Налаштування вікна - трохи вище ніж попереднє бо додали кнопку Kill
            this.Text = "Protsesy - Zavdannya 3";
            this.Width = 800;
            this.Height = 650;  // Вище на 50 пікселів
            this.StartPosition = FormStartPosition.CenterScreen;

            //Label для інтервалу 
            zminna_label_interval = new Label();
            zminna_label_interval.Text = "Interval (ms):";
            zminna_label_interval.Location = new System.Drawing.Point(10, 10);
            zminna_label_interval.Width = 100;
            this.Controls.Add(zminna_label_interval);

            // TextBox для інтервалу 
            zminna_textbox_interval = new TextBox();
            zminna_textbox_interval.Location = new System.Drawing.Point(110, 10);
            zminna_textbox_interval.Width = 80;
            zminna_textbox_interval.Text = "1000";
            this.Controls.Add(zminna_textbox_interval);

            //Button для старту
            zminna_button_start = new Button();
            zminna_button_start.Text = "Start";
            zminna_button_start.Location = new System.Drawing.Point(200, 10);
            zminna_button_start.Click += Button_Click;
            this.Controls.Add(zminna_button_start);

            // Button для Kill (вбивання процесу)
            // Це нова кнопка порівняно з другим завданням
            zminna_button_kill = new Button();
            zminna_button_kill.Text = "Kill Process";  // Kill - це значит завершити
            zminna_button_kill.Location = new System.Drawing.Point(270, 10);
            zminna_button_kill.Click += Button_Kill_Click;  // Викликає функцію Button_Kill_Click
            this.Controls.Add(zminna_button_kill);

            // ListBox для процесів
            zminna_listbox_procesiv = new ListBox();
            zminna_listbox_procesiv.Location = new System.Drawing.Point(10, 50);
            zminna_listbox_procesiv.Width = 380;
            zminna_listbox_procesiv.Height = 550;  // На 50 пікселів більше
            zminna_listbox_procesiv.SelectedIndexChanged += ListBox_SelectedIndexChanged;
            this.Controls.Add(zminna_listbox_procesiv);

            // TextBox для інформації про процес
            zminna_textbox_info = new TextBox();
            zminna_textbox_info.Location = new System.Drawing.Point(400, 50);
            zminna_textbox_info.Width = 380;
            zminna_textbox_info.Height = 550;
            zminna_textbox_info.Multiline = true;
            zminna_textbox_info.ReadOnly = true;
            zminna_textbox_info.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(zminna_textbox_info);

            // Таймер
            zminna_timer.Tick += Timer_Tick;
        }

        // Функція для старту таймера
        private void Button_Click(object sender, EventArgs e)
        {
            int zminna_interval = int.Parse(zminna_textbox_interval.Text);
            zminna_timer.Interval = zminna_interval;
            zminna_timer.Start();
        }

        // Функція таймера - оновлює список процесів
        private void Timer_Tick(object sender, EventArgs e)
        {
            zminna_listbox_procesiv.Items.Clear();

            Process[] zminna_vsi_procesy = Process.GetProcesses();

            foreach (Process zminna_proces in zminna_vsi_procesy)
            {
                zminna_listbox_procesiv.Items.Add(zminna_proces.ProcessName);
            }
        }

        // Функція коли вибираємо процес
        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (zminna_listbox_procesiv.SelectedIndex == -1)
                return;

            string zminna_selected_name = zminna_listbox_procesiv.SelectedItem.ToString();

            Process[] zminna_vsi_procesy = Process.GetProcesses();
            int zminna_kількість_kopiy = 0;

            foreach (Process zminna_proces in zminna_vsi_procesy)
            {
                if (zminna_proces.ProcessName == zminna_selected_name)
                {
                    zminna_kількість_kopiy++;
                }
            }

            Process[] zminna_vybranyy_proces_masiv = Process.GetProcessesByName(zminna_selected_name);

            if (zminna_vybranyy_proces_masiv.Length > 0)
            {
                Process zminna_proces_info = zminna_vybranyy_proces_masiv[0];

                string zminna_info_text = "";
                zminna_info_text += $"Imya: {zminna_proces_info.ProcessName}\n";
                zminna_info_text += $"PID: {zminna_proces_info.Id}\n";
                zminna_info_text += $"Chas startu: {zminna_proces_info.StartTime}\n";
                zminna_info_text += $"CPU time: {zminna_proces_info.TotalProcessorTime}\n";
                zminna_info_text += $"Potokiv: {zminna_proces_info.Threads.Count}\n";
                zminna_info_text += $"Kopiy takoho: {zminna_kількість_kopiy}\n";
                zminna_info_text += $"Memoriya: {zminna_proces_info.WorkingSet64 / 1024 / 1024} MB\n";

                zminna_textbox_info.Text = zminna_info_text;
            }
        }

        // НОВ ФУНКЦІЯ ДЛЯ ВБИВАННЯ ПРОЦЕСУ
        // Це викликається коли натиснемо кнопку "Kill Process"
        private void Button_Kill_Click(object sender, EventArgs e)
        {
            // Перевіряємо чи вибраний процес
            if (zminna_listbox_procesiv.SelectedIndex == -1)
            {
                // Показуємо помилку якщо нічого не вибрано
                MessageBox.Show("Vybery proces!");
                return;
            }

            // Беремо ім'я обраного процесу
            string zminna_selected_name = zminna_listbox_procesiv.SelectedItem.ToString();

            try
            {
                // Шукаємо всі процеси з таким ім'ям
                Process[] zminna_procesy_dlya_vbyvstvy = Process.GetProcessesByName(zminna_selected_name);

                // Вбиваємо кожен процес з таким ім'ям
                foreach (Process zminna_proces_ubyvstvo in zminna_procesy_dlya_vbyvstvy)
                {
                    zminna_proces_ubyvstvo.Kill();  // Kill() - це завершити процес
                }

                // Показуємо повідомлення що вдалось
                MessageBox.Show($"Proces {zminna_selected_name} zavershen!");

                // Очищаємо інформацію в TextBox
                zminna_textbox_info.Clear();
            }
            catch (Exception zminna_error)
            {
                // Якщо помилка, показуємо її
                MessageBox.Show($"Pomylka: {zminna_error.Message}");
            }
        }
    }

    // Клас для запуску
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form3());
        }
    }
}