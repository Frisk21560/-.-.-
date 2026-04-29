using System;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ProccessApp
{
    public partial class Form4 : Form
    {
        // Кнопка для запуску Notepad
        private Button zminna_button_notepad;

        // Кнопка для запуску Калькулятора
        private Button zminna_button_calc;

        // Кноп��а для запуску Paint
        private Button zminna_button_paint;

        // Кнопка для запуску своєї програми
        private Button zminna_button_custom;

        // TextBox де користувач пише шлях до своєї програми
        private TextBox zminna_textbox_custom;

        // Напис для TextBox
        private Label zminna_label_custom;

        // TextBox для логу - де показуються всі операції
        private TextBox zminna_textbox_log;

        public Form4()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Налаштування вікна
            this.Text = "Zapusk Prohram - Zavdannya 4";
            this.Width = 600;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Button для Notepad
            zminna_button_notepad = new Button();
            zminna_button_notepad.Text = "Zapustyty Notepad";  // Запустити Notepad
            zminna_button_notepad.Location = new System.Drawing.Point(20, 20);
            zminna_button_notepad.Width = 150;
            zminna_button_notepad.Height = 40;
            // Коли натиснемо, викликається функція Button_Notepad_Click
            zminna_button_notepad.Click += Button_Notepad_Click;
            this.Controls.Add(zminna_button_notepad);

            // Button для Calculator
            zminna_button_calc = new Button();
            zminna_button_calc.Text = "Zapustyty Kalkulator";
            zminna_button_calc.Location = new System.Drawing.Point(200, 20);
            zminna_button_calc.Width = 150;
            zminna_button_calc.Height = 40;
            zminna_button_calc.Click += Button_Calc_Click;
            this.Controls.Add(zminna_button_calc);

            // Button для Paint
            zminna_button_paint = new Button();
            zminna_button_paint.Text = "Zapustyty Paint";
            zminna_button_paint.Location = new System.Drawing.Point(380, 20);
            zminna_button_paint.Width = 150;
            zminna_button_paint.Height = 40;
            zminna_button_paint.Click += Button_Paint_Click;
            this.Controls.Add(zminna_button_paint);

            // Label для своєї програми
            zminna_label_custom = new Label();
            zminna_label_custom.Text = "Svoya prohrama:";  // Своя програма
            zminna_label_custom.Location = new System.Drawing.Point(20, 80);
            zminna_label_custom.Width = 100;
            this.Controls.Add(zminna_label_custom);

            // TextBox для своєї програми
            // Туди користувач пише назву програми або шлях до неї
            zminna_textbox_custom = new TextBox();
            zminna_textbox_custom.Location = new System.Drawing.Point(120, 80);
            zminna_textbox_custom.Width = 300;
            zminna_textbox_custom.Text = "notepad.exe";  // За замовчуванням notepad
            this.Controls.Add(zminna_textbox_custom);

            // Button для запуску своєї програми
            zminna_button_custom = new Button();
            zminna_button_custom.Text = "Zapustyty";
            zminna_button_custom.Location = new System.Drawing.Point(430, 80);
            zminna_button_custom.Width = 100;
            zminna_button_custom.Height = 25;
            zminna_button_custom.Click += Button_Custom_Click;
            this.Controls.Add(zminna_button_custom);

            // TextBox для логу
            // Тут показується історія всього що ми запускали
            zminna_textbox_log = new TextBox();
            zminna_textbox_log.Location = new System.Drawing.Point(20, 130);
            zminna_textbox_log.Width = 530;
            zminna_textbox_log.Height = 320;
            zminna_textbox_log.Multiline = true;  // Багаторядковий текст
            zminna_textbox_log.ReadOnly = true;   // Не можна редагувати
            zminna_textbox_log.ScrollBars = ScrollBars.Vertical;  // Смуга прокручування
            this.Controls.Add(zminna_textbox_log);
        }

        // Функція для запуску Notepad
        private void Button_Notepad_Click(object sender, EventArgs e)
        {
            try
            {
                // Process.Start() - це функція що запускає програму
                Process zminna_process = Process.Start("notepad.exe");

                // Показуємо в логу що вдалось запустити
                Zapysaty_Log($"Notepad zapusheno! PID: {zminna_process.Id}");
            }
            catch (Exception zminna_error)
            {
                // Якщо помилка, показуємо її в логу
                Zapysaty_Log($"Pomylka: {zminna_error.Message}");
            }
        }

        // Функція для запуску Calculator
        private void Button_Calc_Click(object sender, EventArgs e)
        {
            try
            {
                // calc.exe - це стандартна програма для розрахунків на Windows
                Process zminna_process = Process.Start("calc.exe");
                Zapysaty_Log($"Kalkulator zapusheno! PID: {zminna_process.Id}");
            }
            catch (Exception zminna_error)
            {
                Zapysaty_Log($"Pomylka: {zminna_error.Message}");
            }
        }

        // Функція для запуску Paint
        private void Button_Paint_Click(object sender, EventArgs e)
        {
            try
            {
                // mspaint.exe - це програма для малювання на Windows
                Process zminna_process = Process.Start("mspaint.exe");
                Zapysaty_Log($"Paint zapusheno! PID: {zminna_process.Id}");
            }
            catch (Exception zminna_error)
            {
                Zapysaty_Log($"Pomylka: {zminna_error.Message}");
            }
        }

        // Функція для запуску своєї програми
        private void Button_Custom_Click(object sender, EventArgs e)
        {
            try
            {
                // Беремо текст з TextBox - це шлях до програми
                string zminna_program_path = zminna_textbox_custom.Text;

                // Перевіряємо чи користувач щось ввів
                if (string.IsNullOrWhiteSpace(zminna_program_path))
                {
                    Zapysaty_Log("Vvedit shlyakh do prohrawy!");  // Введіть шлях до програми
                    return;
                }

                // Запускаємо програму
                Process zminna_process = Process.Start(zminna_program_path);
                Zapysaty_Log($"Prohrama {zminna_program_path} zapushena! PID: {zminna_process.Id}");
            }
            catch (Exception zminna_error)
            {
                Zapysaty_Log($"Pomylka: {zminna_error.Message}");
            }
        }

        // Функція для запису в лог
        // Ця функція додає нову строку до логу з часовою міткою
        private void Zapysaty_Log(string zminna_text)
        {
            // Додаємо час [HH:mm:ss], потім текст, потім новий рядок
            zminna_textbox_log.Text += $"[{DateTime.Now:HH:mm:ss}] {zminna_text}\n";

            // Переміщаємо курсор на кінець тексту щоб бачити останнє повідомлення
            zminna_textbox_log.SelectionStart = zminna_textbox_log.Text.Length;

            // ScrollToCaret() - прокручує до курсору щоб показати нову строку
            zminna_textbox_log.ScrollToCaret();
        }
    }

    // Клас для запуску додатка
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Включаємо стилі Windows
            Application.EnableVisualStyles();

            // Запускаємо вікно
            Application.Run(new Form4());
        }
    }
}