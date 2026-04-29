using System;
using System.IO;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MultithreadingApp
{
    // Додаток для шифрування файлів шифром Цезаря
    public partial class CipherForm : Form
    {
        // Змінна для потоку шифрування
        private Thread zminna_potik_syfruvannya = null;

        // Змінна для скасування операції
        private bool zminna_skasuvaty = false;

        // Label для вибору файлу
        private Label zminna_label_fayil;

        // TextBox для шляху до файлу
        private TextBox zminna_textbox_shlyakh;

        // Кнопка для вибору файлу
        private Button zminna_button_vybir;

        // Label для зсуву
        private Label zminna_label_zsyv;

        // TextBox для зсуву (ключ)
        private TextBox zminna_textbox_zsyv;

        // Label для виходу
        private Label zminna_label_vyhid;

        // TextBox для шляху виходу
        private TextBox zminna_textbox_vyhid;

        // Кнопка для старту шифрування
        private Button zminna_button_start;

        // Кнопка для скасування
        private Button zminna_button_cancel;

        // TextBox для логу
        private TextBox zminna_textbox_log;

        // ProgressBar для прогресу
        private ProgressBar zminna_progress;

        public CipherForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Налаштування форми
            this.Text = "Шифрування Файлів - Цезар";
            this.Width = 700;
            this.Height = 750;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Label для шляху файлу
            zminna_label_fayil = new Label();
            zminna_label_fayil.Text = "Вхідний файл:";
            zminna_label_fayil.Location = new System.Drawing.Point(10, 10);
            zminna_label_fayil.Width = 100;
            this.Controls.Add(zminna_label_fayil);

            // TextBox для шляху файлу
            zminna_textbox_shlyakh = new TextBox();
            zminna_textbox_shlyakh.Location = new System.Drawing.Point(110, 10);
            zminna_textbox_shlyakh.Width = 450;
            zminna_textbox_shlyakh.Text = "C:\\input.txt";
            this.Controls.Add(zminna_textbox_shlyakh);

            // Кнопка для вибору файлу
            zminna_button_vybir = new Button();
            zminna_button_vybir.Text = "Вибір";
            zminna_button_vybir.Location = new System.Drawing.Point(570, 10);
            zminna_button_vybir.Width = 80;
            zminna_button_vybir.Click += Button_Vybir_Click;
            this.Controls.Add(zminna_button_vybir);

            // Label для зсуву
            zminna_label_zsyv = new Label();
            zminna_label_zsyv.Text = "Зсув (1-25):";
            zminna_label_zsyv.Location = new System.Drawing.Point(10, 50);
            zminna_label_zsyv.Width = 100;
            this.Controls.Add(zminna_label_zsyv);

            // TextBox для зсуву
            zminna_textbox_zsyv = new TextBox();
            zminna_textbox_zsyv.Location = new System.Drawing.Point(110, 50);
            zminna_textbox_zsyv.Width = 100;
            zminna_textbox_zsyv.Text = "3";
            this.Controls.Add(zminna_textbox_zsyv);

            // Label для виходу
            zminna_label_vyhid = new Label();
            zminna_label_vyhid.Text = "Вихідний файл:";
            zminna_label_vyhid.Location = new System.Drawing.Point(10, 90);
            zminna_label_vyhid.Width = 100;
            this.Controls.Add(zminna_label_vyhid);

            // TextBox для виходу
            zminna_textbox_vyhid = new TextBox();
            zminna_textbox_vyhid.Location = new System.Drawing.Point(110, 90);
            zminna_textbox_vyhid.Width = 450;
            zminna_textbox_vyhid.Text = "C:\\output.txt";
            this.Controls.Add(zminna_textbox_vyhid);

            // Кнопка для старту
            zminna_button_start = new Button();
            zminna_button_start.Text = "Запустити";
            zminna_button_start.Location = new System.Drawing.Point(110, 130);
            zminna_button_start.Width = 120;
            zminna_button_start.Height = 40;
            zminna_button_start.Font = new System.Drawing.Font("Arial", 12);
            zminna_button_start.Click += Button_Start_Click;
            this.Controls.Add(zminna_button_start);

            // Кнопка для скасування
            zminna_button_cancel = new Button();
            zminna_button_cancel.Text = "Скасувати";
            zminna_button_cancel.Location = new System.Drawing.Point(240, 130);
            zminna_button_cancel.Width = 120;
            zminna_button_cancel.Height = 40;
            zminna_button_cancel.Font = new System.Drawing.Font("Arial", 12);
            zminna_button_cancel.Enabled = false;
            zminna_button_cancel.Click += Button_Cancel_Click;
            this.Controls.Add(zminna_button_cancel);

            // ProgressBar для прогресу
            zminna_progress = new ProgressBar();
            zminna_progress.Location = new System.Drawing.Point(10, 180);
            zminna_progress.Width = 650;
            zminna_progress.Height = 30;
            zminna_progress.Style = ProgressBarStyle.Continuous;
            this.Controls.Add(zminna_progress);

            // TextBox для логу
            zminna_textbox_log = new TextBox();
            zminna_textbox_log.Location = new System.Drawing.Point(10, 220);
            zminna_textbox_log.Width = 650;
            zminna_textbox_log.Height = 470;
            zminna_textbox_log.Multiline = true;
            zminna_textbox_log.ReadOnly = true;
            zminna_textbox_log.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(zminna_textbox_log);
        }

        // Обробка кнопки для вибору файлу
        private void Button_Vybir_Click(object sender, EventArgs e)
        {
            // Створюємо діалог для вибору файлу
            OpenFileDialog zminna_dialog = new OpenFileDialog();
            zminna_dialog.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
            zminna_dialog.Title = "Виберіть файл для шифрування";

            // Якщо користувач вибрав файл
            if (zminna_dialog.ShowDialog() == DialogResult.OK)
            {
                // Встановлюємо шлях у TextBox
                zminna_textbox_shlyakh.Text = zminna_dialog.FileName;
                Zapysaty_Log($"Вибраний файл: {zminna_dialog.FileName}");
            }
        }

        // Обробка кнопки старту
        private void Button_Start_Click(object sender, EventArgs e)
        {
            // Перевіряємо чи вже працює процес
            if (zminna_potik_syfruvannya != null && zminna_potik_syfruvannya.IsAlive)
            {
                MessageBox.Show("Шифрування вже в процесі!");
                return;
            }

            // Беремо параметри
            string zminna_shlyakh_vhodu = zminna_textbox_shlyakh.Text;
            string zminna_shlyakh_vyhodu = zminna_textbox_vyhid.Text;
            int zminna_zsyv = 0;

            // Перевіряємо чи введено шляхи
            if (string.IsNullOrWhiteSpace(zminna_shlyakh_vhodu) ||
                string.IsNullOrWhiteSpace(zminna_shlyakh_vyhodu))
            {
                MessageBox.Show("Введіть шляхи до файлів!");
                return;
            }

            // Перевіряємо чи введено зсув
            if (!int.TryParse(zminna_textbox_zsyv.Text, out zminna_zsyv) ||
                zminna_zsyv < 1 || zminna_zsyv > 25)
            {
                MessageBox.Show("Зсув повинен бути від 1 до 25!");
                return;
            }

            // Перевіряємо чи існує файл
            if (!File.Exists(zminna_shlyakh_vhodu))
            {
                MessageBox.Show("Файл не знайдено!");
                return;
            }

            // Скидаємо прапорець скасування
            zminna_skasuvaty = false;

            // Створюємо новий потік для шифрування
            zminna_potik_syfruvannya = new Thread(() =>
            {
                Syfruvannya(zminna_shlyakh_vhodu, zminna_shlyakh_vyhodu, zminna_zsyv);
            });

            // Запускаємо потік
            zminna_potik_syfruvannya.Start();

            // Вимикаємо кнопку старту
            zminna_button_start.Enabled = false;
            // Вмикаємо кнопку скасування
            zminna_button_cancel.Enabled = true;

            Zapysaty_Log("Шифрування розпочато...");
        }

        // Обробка кнопки скасування
        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            // Встановлюємо прапорець скасування
            zminna_skasuvaty = true;
            Zapysaty_Log("Скасування шифрування...");
        }

        // Функція для шифрування файлу
        private void Syfruvannya(string zminna_shlyakh_vhodu,
                                 string zminna_shlyakh_vyhodu,
                                 int zminna_zsyv)
        {
            try
            {
                // Читаємо весь текст з файлу
                string zminna_tekst_vhodu = File.ReadAllText(zminna_shlyakh_vhodu);

                // Лічильник для прогресу
                int zminna_obrobleno = 0;
                int zminna_vsyogo = zminna_tekst_vhodu.Length;

                // Створюємо результат
                string zminna_tekst_vyhodu = "";

                // Обробляємо кожен символ
                foreach (char zminna_symvol in zminna_tekst_vhodu)
                {
                    // Перевіряємо чи скасовано операцію
                    if (zminna_skasuvaty)
                    {
                        this.Invoke(new Action(() =>
                        {
                            Zapysaty_Log("Шифрування скасовано!");
                            zminna_button_start.Enabled = true;
                            zminna_button_cancel.Enabled = false;
                            zminna_progress.Value = 0;
                        }));
                        return;
                    }

                    // Шифруємо символ
                    char zminna_zashyfrovannyy = Zashyfruvaty_Symvol(zminna_symvol, zminna_zsyv);
                    zminna_tekst_vyhodu += zminna_zashyfrovannyy;

                    // Оновлюємо прогрес
                    zminna_obrobleno++;
                    int zminna_protsent = (zminna_obrobleno * 100) / zminna_vsyogo;

                    // Оновлюємо ProgressBar в головному потоці
                    this.Invoke(new Action(() =>
                    {
                        zminna_progress.Value = zminna_protsent;
                    }));

                    // Невелика затримка щоб показати прогрес
                    Thread.Sleep(1);
                }

                // Записуємо зашифрований текст у файл
                File.WriteAllText(zminna_shlyakh_vyhodu, zminna_tekst_vyhodu);

                // Оновлюємо UI в головному потоці
                this.Invoke(new Action(() =>
                {
                    zminna_progress.Value = 100;
                    Zapysaty_Log("Шифрування завершено!");
                    Zapysaty_Log($"Файл збережено: {zminna_shlyakh_vyhodu}");
                    zminna_button_start.Enabled = true;
                    zminna_button_cancel.Enabled = false;
                    MessageBox.Show("Шифрування завершено успішно!");
                }));
            }
            catch (Exception zminna_error)
            {
                // Обробляємо помилки
                this.Invoke(new Action(() =>
                {
                    Zapysaty_Log($"Помилка: {zminna_error.Message}");
                    zminna_button_start.Enabled = true;
                    zminna_button_cancel.Enabled = false;
                    MessageBox.Show($"Помилка: {zminna_error.Message}");
                }));
            }
        }

        // Функція для шифрування одного символу
        private char Zashyfruvaty_Symvol(char zminna_symvol, int zminna_zsyv)
        {
            // Якщо це велика латинська літера
            if (zminna_symvol >= 'A' && zminna_symvol <= 'Z')
            {
                // Зсуваємо у межах A-Z
                int zminna_pozyciya = zminna_symvol - 'A';
                zminna_pozyciya = (zminna_pozyciya + zminna_zsyv) % 26;
                return (char)('A' + zminna_pozyciya);
            }

            // Якщо це мала латинська літера
            if (zminna_symvol >= 'a' && zminna_symvol <= 'z')
            {
                // Зсуваємо у межах a-z
                int zminna_pozyciya = zminna_symvol - 'a';
                zminna_pozyciya = (zminna_pozyciya + zminna_zsyv) % 26;
                return (char)('a' + zminna_pozyciya);
            }

            // Інші символи не змінюємо
            return zminna_symvol;
        }

        // Допоміжна функція для запису в лог
        private void Zapysaty_Log(string zminna_text)
        {
            // Додаємо час і текст до логу
            zminna_textbox_log.Text += $"[{DateTime.Now:HH:mm:ss}] {zminna_text}\n";
            // Переміщаємо курсор на кінець
            zminna_textbox_log.SelectionStart = zminna_textbox_log.Text.Length;
            // Прокручуємо до курсору
            zminna_textbox_log.ScrollToCaret();
        }
    }

    // Клас програми - запуск додатка
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Включаємо візуальні стилі Windows
            Application.EnableVisualStyles();

            // Запускаємо форму
            Application.Run(new CipherForm());
        }
    }
}