using System;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MultithreadingApp
{
    // Завдання 2 - Годинник у WinForms
    public partial class ClockForm : Form
    {
        // Змінна для Timer'а
        private Timer zminna_timer = null;

        // Label для виведення часу
        private Label zminna_label_chas;

        // Label для дня тижня
        private Label zminna_label_den;

        // Label для дати
        private Label zminna_label_data;

        // Кнопка для старту
        private Button zminna_button_start;

        // Кнопка для зупинки
        private Button zminna_button_stop;

        public ClockForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Налаштування форми
            this.Text = "Годинник - Завдання 2";
            this.Width = 500;
            this.Height = 400;
            // Центруємо вікно на екрані
            this.StartPosition = FormStartPosition.CenterScreen;
            // Чорний фон
            this.BackColor = System.Drawing.Color.Black;

            // Label для часу
            zminna_label_chas = new Label();
            zminna_label_chas.Text = "00:00:00";
            zminna_label_chas.Location = new System.Drawing.Point(50, 50);
            zminna_label_chas.Width = 400;
            zminna_label_chas.Height = 100;
            // Великий шрифт для часу
            zminna_label_chas.Font = new System.Drawing.Font("Arial", 60, System.Drawing.FontStyle.Bold);
            // Блакитний колір
            zminna_label_chas.ForeColor = System.Drawing.Color.Cyan;
            // Вирівнювання по центру
            zminna_label_chas.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Controls.Add(zminna_label_chas);

            // Label для дня тижня
            zminna_label_den = new Label();
            zminna_label_den.Text = "День тижня";
            zminna_label_den.Location = new System.Drawing.Point(50, 160);
            zminna_label_den.Width = 400;
            zminna_label_den.Height = 30;
            zminna_label_den.Font = new System.Drawing.Font("Arial", 16);
            // Білий колір
            zminna_label_den.ForeColor = System.Drawing.Color.White;
            zminna_label_den.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Controls.Add(zminna_label_den);

            // Label для дати
            zminna_label_data = new Label();
            zminna_label_data.Text = "01.01.2025";
            zminna_label_data.Location = new System.Drawing.Point(50, 200);
            zminna_label_data.Width = 400;
            zminna_label_data.Height = 30;
            zminna_label_data.Font = new System.Drawing.Font("Arial", 16);
            // Жовтий колір
            zminna_label_data.ForeColor = System.Drawing.Color.Yellow;
            zminna_label_data.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Controls.Add(zminna_label_data);

            // Кнопка для старту
            zminna_button_start = new Button();
            zminna_button_start.Text = "Start";
            zminna_button_start.Location = new System.Drawing.Point(100, 260);
            zminna_button_start.Width = 120;
            zminna_button_start.Height = 40;
            zminna_button_start.Font = new System.Drawing.Font("Arial", 12);
            // Коли натиснемо, викликається функція Button_Start_Click
            zminna_button_start.Click += Button_Start_Click;
            this.Controls.Add(zminna_button_start);

            // Кнопка для зупинки
            zminna_button_stop = new Button();
            zminna_button_stop.Text = "Stop";
            zminna_button_stop.Location = new System.Drawing.Point(280, 260);
            zminna_button_stop.Width = 120;
            zminna_button_stop.Height = 40;
            zminna_button_stop.Font = new System.Drawing.Font("Arial", 12);
            // Коли натиснемо, викликається функція Button_Stop_Click
            zminna_button_stop.Click += Button_Stop_Click;
            this.Controls.Add(zminna_button_stop);
        }

        // Обробка кнопки Start
        private void Button_Start_Click(object sender, EventArgs e)
        {
            // Перевіряємо чи вже працює Timer
            if (zminna_timer != null)
            {
                MessageBox.Show("Годинник вже працює!");
                return;
            }

            // Створюємо новий Timer
            // TimerCallback - це метода що виконується
            // null - параметр для методи
            // 0 - відразу запустити
            // 1000 - інтервал у мс (1 секунда)
            zminna_timer = new Timer(TimerCallback, null, 0, 1000);

            // Вимикаємо кнопку Start
            zminna_button_start.Enabled = false;
            // Вмикаємо кнопку Stop
            zminna_button_stop.Enabled = true;
        }

        // Обробка кнопки Stop
        private void Button_Stop_Click(object sender, EventArgs e)
        {
            // Перевіряємо чи працює Timer
            if (zminna_timer == null)
            {
                MessageBox.Show("Годинник вже зупинен!");
                return;
            }

            // Затримуємо Timer і звільняємо ресурси
            zminna_timer.Dispose();
            zminna_timer = null;

            // Вмикаємо кнопку Start
            zminna_button_start.Enabled = true;
            // Вимикаємо кнопку Stop
            zminna_button_stop.Enabled = false;
        }

        // Метода що виконується кожну секунду
        private void TimerCallback(object zminna_state)
        {
            // Отримуємо поточний час
            DateTime zminna_chas_teper = DateTime.Now;

            // Форматуємо час у вигляді HH:mm:ss
            string zminna_chas_forma = zminna_chas_teper.ToString("HH:mm:ss");
            // Форматуємо день тижня
            string zminna_den_forma = zminna_chas_teper.ToString("dddd");
            // Форматуємо дату у вигляді dd.MM.yyyy
            string zminna_data_forma = zminna_chas_teper.ToString("dd.MM.yyyy");

            // Оновлюємо Label'и
            // Потрібно використати Invoke тому що робочий потік не може безпосередньо змінювати UI
            this.Invoke(new Action(() =>
            {
                // Встановлюємо текст для часу
                zminna_label_chas.Text = zminna_chas_forma;
                // Встановлюємо текст для дня
                zminna_label_den.Text = zminna_den_forma;
                // Встановлюємо текст для дати
                zminna_label_data.Text = zminna_data_forma;
            }));
        }
    }

    // Клас Program - запуск програми
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Включаємо візуальні стилі Windows
            Application.EnableVisualStyles();

            // Запускаємо форму з годинником
            Application.Run(new ClockForm());
        }
    }
}