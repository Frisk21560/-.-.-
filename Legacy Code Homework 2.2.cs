using System;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ChildProcessApp
{
    // Головна форма з усіма завданнями
    public partial class MainForm : Form
    {
        // Змінна для зберігання поточного процесу
        private TabControl zminna_tabcontrol;
        private Process zminna_teperishniy_proces = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Встановлюємо назву вікна
            this.Text = "Dochirni Procesy - Usima Zavdannyamy";
            this.Width = 800;
            this.Height = 650;
            // Центруємо вікно на екрані
            this.StartPosition = FormStartPosition.CenterScreen;

            // Створюємо TabControl - це коробка з вкладками
            zminna_tabcontrol = new TabControl();
            zminna_tabcontrol.Location = new System.Drawing.Point(10, 10);
            zminna_tabcontrol.Width = 770;
            zminna_tabcontrol.Height = 600;
            this.Controls.Add(zminna_tabcontrol);

            // Додаємо вкладки для кожного завдання
            zminna_tabcontrol.TabPages.Add(CreateTab1());
            zminna_tabcontrol.TabPages.Add(CreateTab2());
            zminna_tabcontrol.TabPages.Add(CreateTab3());
            zminna_tabcontrol.TabPages.Add(CreateTab4());
        }

        // ЗАВДАННЯ 1 - Запуск і чекання на завершення
        private TabPage CreateTab1()
        {
            // Створюємо нову вкладку
            TabPage zminna_tab = new TabPage("Zavdannya 1 - Zapusk i Chekanie");

            // Напис для введення програми
            Label zminna_label_prog = new Label();
            zminna_label_prog.Text = "Programa:";
            zminna_label_prog.Location = new System.Drawing.Point(10, 10);
            zminna_label_prog.Width = 100;
            zminna_tab.Controls.Add(zminna_label_prog);

            // Текстбокс для введення імені програми
            TextBox zminna_textbox_prog = new TextBox();
            zminna_textbox_prog.Location = new System.Drawing.Point(110, 10);
            zminna_textbox_prog.Width = 250;
            zminna_textbox_prog.Text = "notepad.exe";
            zminna_tab.Controls.Add(zminna_textbox_prog);

            // Кнопка для запуску
            Button zminna_button_run = new Button();
            zminna_button_run.Text = "Zapustyty i Chekaty";
            zminna_button_run.Location = new System.Drawing.Point(370, 10);
            zminna_button_run.Width = 150;
            zminna_tab.Controls.Add(zminna_button_run);

            // Текстбокс для виведення результату
            TextBox zminna_textbox_result = new TextBox();
            zminna_textbox_result.Location = new System.Drawing.Point(10, 50);
            zminna_textbox_result.Width = 510;
            zminna_textbox_result.Height = 470;
            zminna_textbox_result.Multiline = true;
            zminna_textbox_result.ReadOnly = true;
            zminna_textbox_result.ScrollBars = ScrollBars.Vertical;
            zminna_tab.Controls.Add(zminna_textbox_result);

            // Обробка натискання кнопки
            zminna_button_run.Click += (sender, e) =>
            {
                // Беремо шлях до програми з текстбокса
                string zminna_program = zminna_textbox_prog.Text;

                // Перевіряємо чи користувач щось ввів
                if (string.IsNullOrWhiteSpace(zminna_program))
                {
                    zminna_textbox_result.Text = "Vvid' shlyakh do prohrawy!";
                    return;
                }

                try
                {
                    // Створюємо новий процес
                    Process zminna_process = new Process();
                    zminna_process.StartInfo.FileName = zminna_program;
                    // Запускаємо процес
                    zminna_process.Start();

                    // Виводимо інформацію про запущений процес
                    zminna_textbox_result.Text = $"Proces '{zminna_program}' zapusheno!\n";
                    zminna_textbox_result.Text += $"PID: {zminna_process.Id}\n";
                    zminna_textbox_result.Text += $"Chas startu: {zminna_process.StartTime}\n\n";
                    zminna_textbox_result.Text += "Chekayemo na zavershenie...\n";

                    // ВАЖЛИВО - WaitForExit() чекає поки процес завершиться
                    zminna_process.WaitForExit();

                    // Отримуємо код завершення процесу
                    int zminna_kod_zavershen = zminna_process.ExitCode;

                    // Виводимо результат
                    zminna_textbox_result.Text += "\nPROCES ZAVERSHENO\n";
                    zminna_textbox_result.Text += $"Kod zavershenia: {zminna_kod_zavershen}\n";

                    // Перевіряємо статус завершення
                    if (zminna_kod_zavershen == 0)
                    {
                        zminna_textbox_result.Text += "Status: Uspishno zakinchuvsya!";
                    }
                    else
                    {
                        zminna_textbox_result.Text += "Status: Zavershyvsya z pomylkoyu!";
                    }

                    // Звільняємо ресурси
                    zminna_process.Dispose();
                }
                catch (Exception zminna_error)
                {
                    zminna_textbox_result.Text = $"Pomylka: {zminna_error.Message}";
                }
            };

            return zminna_tab;
        }

        // ЗАВДАННЯ 2 - Запуск з вибором (чекати або kill)
        private TabPage CreateTab2()
        {
            // Створюємо вкладку
            TabPage zminna_tab = new TabPage("Zavdannya 2 - Zapusk abo Kill");

            // Напис для програми
            Label zminna_label_prog = new Label();
            zminna_label_prog.Text = "Programa:";
            zminna_label_prog.Location = new System.Drawing.Point(10, 10);
            zminna_label_prog.Width = 100;
            zminna_tab.Controls.Add(zminna_label_prog);

            // Текстбокс для введення програми
            TextBox zminna_textbox_prog = new TextBox();
            zminna_textbox_prog.Location = new System.Drawing.Point(110, 10);
            zminna_textbox_prog.Width = 200;
            zminna_textbox_prog.Text = "notepad.exe";
            zminna_tab.Controls.Add(zminna_textbox_prog);

            // Кнопка для запуску
            Button zminna_button_run = new Button();
            zminna_button_run.Text = "Zapustyty";
            zminna_button_run.Location = new System.Drawing.Point(320, 10);
            zminna_button_run.Width = 80;
            zminna_tab.Controls.Add(zminna_button_run);

            // Кнопка для чекання завершення
            Button zminna_button_wait = new Button();
            zminna_button_wait.Text = "Chekaty";
            zminna_button_wait.Location = new System.Drawing.Point(410, 10);
            zminna_button_wait.Width = 80;
            zminna_tab.Controls.Add(zminna_button_wait);

            // Кнопка для вбивання процесу
            Button zminna_button_kill = new Button();
            zminna_button_kill.Text = "Kill";
            zminna_button_kill.Location = new System.Drawing.Point(500, 10);
            zminna_button_kill.Width = 80;
            zminna_tab.Controls.Add(zminna_button_kill);

            // Текстбокс для результатів
            TextBox zminna_textbox_result = new TextBox();
            zminna_textbox_result.Location = new System.Drawing.Point(10, 50);
            zminna_textbox_result.Width = 510;
            zminna_textbox_result.Height = 470;
            zminna_textbox_result.Multiline = true;
            zminna_textbox_result.ReadOnly = true;
            zminna_textbox_result.ScrollBars = ScrollBars.Vertical;
            zminna_tab.Controls.Add(zminna_textbox_result);

            // Обробка кнопки Запусити
            zminna_button_run.Click += (sender, e) =>
            {
                // Беремо шлях до програми
                string zminna_program = zminna_textbox_prog.Text;

                if (string.IsNullOrWhiteSpace(zminna_program))
                {
                    zminna_textbox_result.Text = "Vvid' shlyakh do prohrawy!";
                    return;
                }

                try
                {
                    // Якщо уже є запущений процес, видаляємо його
                    if (zminna_teperishniy_proces != null && !zminna_teperishniy_proces.HasExited)
                    {
                        zminna_textbox_result.Text += "\nPoperednyy proces zaversheno.\n";
                    }

                    // Запускаємо новий процес
                    zminna_teperishniy_proces = new Process();
                    zminna_teperishniy_proces.StartInfo.FileName = zminna_program;
                    zminna_teperishniy_proces.Start();

                    // Виводимо інформацію
                    zminna_textbox_result.Text += $"[{DateTime.Now:HH:mm:ss}] Proces '{zminna_program}' zapusheno!\n";
                    zminna_textbox_result.Text += $"PID: {zminna_teperishniy_proces.Id}\n";
                    zminna_textbox_result.Text += $"Status: Vykonuyetsya\n\n";

                    zminna_textbox_result.SelectionStart = zminna_textbox_result.Text.Length;
                    zminna_textbox_result.ScrollToCaret();
                }
                catch (Exception zminna_error)
                {
                    zminna_textbox_result.Text += $"\nPomylka: {zminna_error.Message}\n";
                }
            };

            // Обробка кнопки Чекати
            zminna_button_wait.Click += (sender, e) =>
            {
                // Перевіряємо чи є запущений процес
                if (zminna_teperishniy_proces == null)
                {
                    zminna_textbox_result.Text += "\n[!] Proces ne zapusheno!\n";
                    return;
                }

                // Перевіряємо чи процес уже завершився
                if (zminna_teperishniy_proces.HasExited)
                {
                    zminna_textbox_result.Text += "\n[!] Proces vzhe zavershyvsya!\n";
                    return;
                }

                try
                {
                    zminna_textbox_result.Text += "\n[*] Chekayemo na zavershenie...\n";

                    // Чекаємо на завершення процесу
                    zminna_teperishniy_proces.WaitForExit();

                    // Отримуємо код завершення
                    int zminna_exit_code = zminna_teperishniy_proces.ExitCode;

                    zminna_textbox_result.Text += $"[OK] Proces zaversheno!\n";
                    zminna_textbox_result.Text += $"Kod zavershenia: {zminna_exit_code}\n";

                    if (zminna_exit_code == 0)
                    {
                        zminna_textbox_result.Text += "Status: Uspishno!\n";
                    }
                    else
                    {
                        zminna_textbox_result.Text += "Status: Z pomylkoyu!\n";
                    }

                    zminna_teperishniy_proces.Dispose();
                    zminna_teperishniy_proces = null;

                    zminna_textbox_result.SelectionStart = zminna_textbox_result.Text.Length;
                    zminna_textbox_result.ScrollToCaret();
                }
                catch (Exception zminna_error)
                {
                    zminna_textbox_result.Text += $"\nPomylka: {zminna_error.Message}\n";
                }
            };

            // Обробка кнопки Kill
            zminna_button_kill.Click += (sender, e) =>
            {
                // Перевіряємо чи є процес
                if (zminna_teperishniy_proces == null)
                {
                    zminna_textbox_result.Text += "\n[!] Proces ne zapusheno!\n";
                    return;
                }

                if (zminna_teperishniy_proces.HasExited)
                {
                    zminna_textbox_result.Text += "\n[!] Proces vzhe zavershyvsya!\n";
                    return;
                }

                try
                {
                    zminna_textbox_result.Text += "\n[*] Vbyvayemo proces...\n";

                    // Kill() - вбиває процес
                    zminna_teperishniy_proces.Kill();

                    zminna_textbox_result.Text += $"[OK] Proces PID {zminna_teperishniy_proces.Id} zaversheno!\n";

                    zminna_teperishniy_proces.Dispose();
                    zminna_teperishniy_proces = null;

                    zminna_textbox_result.SelectionStart = zminna_textbox_result.Text.Length;
                    zminna_textbox_result.ScrollToCaret();
                }
                catch (Exception zminna_error)
                {
                    zminna_textbox_result.Text += $"\nPomylka: {zminna_error.Message}\n";
                }
            };

            return zminna_tab;
        }

        // ЗАВДАННЯ 3 - Запуск з аргументами (калькулятор)
        private TabPage CreateTab3()
        {
            // Створюємо вкладку
            TabPage zminna_tab = new TabPage("Zavdannya 3 - Argumenty Kalkulator");

            // Напис для першого числа
            Label zminna_label_chyslo1 = new Label();
            zminna_label_chyslo1.Text = "Pershe chyslo:";
            zminna_label_chyslo1.Location = new System.Drawing.Point(10, 10);
            zminna_label_chyslo1.Width = 100;
            zminna_tab.Controls.Add(zminna_label_chyslo1);

            // Текстбокс для першого числа
            TextBox zminna_textbox_chyslo1 = new TextBox();
            zminna_textbox_chyslo1.Location = new System.Drawing.Point(110, 10);
            zminna_textbox_chyslo1.Width = 100;
            zminna_textbox_chyslo1.Text = "7";
            zminna_tab.Controls.Add(zminna_textbox_chyslo1);

            // Напис для другого числа
            Label zminna_label_chyslo2 = new Label();
            zminna_label_chyslo2.Text = "Druhe chyslo:";
            zminna_label_chyslo2.Location = new System.Drawing.Point(10, 50);
            zminna_label_chyslo2.Width = 100;
            zminna_tab.Controls.Add(zminna_label_chyslo2);

            // Текстбокс для другого числа
            TextBox zminna_textbox_chyslo2 = new TextBox();
            zminna_textbox_chyslo2.Location = new System.Drawing.Point(110, 50);
            zminna_textbox_chyslo2.Width = 100;
            zminna_textbox_chyslo2.Text = "3";
            zminna_tab.Controls.Add(zminna_textbox_chyslo2);

            // Напис для операції
            Label zminna_label_operaciya = new Label();
            zminna_label_operaciya.Text = "Operaciya:";
            zminna_label_operaciya.Location = new System.Drawing.Point(10, 90);
            zminna_label_operaciya.Width = 100;
            zminna_tab.Controls.Add(zminna_label_operaciya);

            // Комбобокс для вибору операції
            ComboBox zminna_combobox_oper = new ComboBox();
            zminna_combobox_oper.Location = new System.Drawing.Point(110, 90);
            zminna_combobox_oper.Width = 100;
            zminna_combobox_oper.Items.Add("+");
            zminna_combobox_oper.Items.Add("-");
            zminna_combobox_oper.Items.Add("*");
            zminna_combobox_oper.Items.Add("/");
            zminna_combobox_oper.SelectedIndex = 0;
            zminna_tab.Controls.Add(zminna_combobox_oper);

            // Кнопка для обчислення
            Button zminna_button_calc = new Button();
            zminna_button_calc.Text = "Obchyslyty";
            zminna_button_calc.Location = new System.Drawing.Point(110, 130);
            zminna_button_calc.Width = 100;
            zminna_button_calc.Height = 30;
            zminna_tab.Controls.Add(zminna_button_calc);

            // Текстбокс для результату
            TextBox zminna_textbox_result = new TextBox();
            zminna_textbox_result.Location = new System.Drawing.Point(10, 170);
            zminna_textbox_result.Width = 500;
            zminna_textbox_result.Height = 350;
            zminna_textbox_result.Multiline = true;
            zminna_textbox_result.ReadOnly = true;
            zminna_textbox_result.ScrollBars = ScrollBars.Vertical;
            zminna_tab.Controls.Add(zminna_textbox_result);

            // Обробка натискання кнопки
            zminna_button_calc.Click += (sender, e) =>
            {
                try
                {
                    // Беремо значення з текстбоксів
                    string zminna_chyslo1_str = zminna_textbox_chyslo1.Text;
                    string zminna_chyslo2_str = zminna_textbox_chyslo2.Text;
                    string zminna_operaciya_str = zminna_combobox_oper.SelectedItem.ToString();

                    // Перевіряємо чи введені праильні числа
                    if (!int.TryParse(zminna_chyslo1_str, out int zminna_chyslo1) ||
                        !int.TryParse(zminna_chyslo2_str, out int zminna_chyslo2))
                    {
                        zminna_textbox_result.Text = "Pomylka: Vvedit' pravilni chysla!";
                        return;
                    }

                    zminna_textbox_result.Text = "Vykonuyemo rozrahunok...\n\n";

                    // Виконуємо розрахунок в батьківському процесі
                    int zminna_rezultat = 0;
                    bool zminna_uspish = true;

                    // Вибираємо операцію
                    switch (zminna_operaciya_str)
                    {
                        case "+":
                            zminna_rezultat = zminna_chyslo1 + zminna_chyslo2;
                            zminna_textbox_result.Text += $"Operaciya: {zminna_chyslo1} + {zminna_chyslo2}\n";
                            break;
                        case "-":
                            zminna_rezultat = zminna_chyslo1 - zminna_chyslo2;
                            zminna_textbox_result.Text += $"Operaciya: {zminna_chyslo1} - {zminna_chyslo2}\n";
                            break;
                        case "*":
                            zminna_rezultat = zminna_chyslo1 * zminna_chyslo2;
                            zminna_textbox_result.Text += $"Operaciya: {zminna_chyslo1} * {zminna_chyslo2}\n";
                            break;
                        case "/":
                            // Перевіряємо ділення на нуль
                            if (zminna_chyslo2 == 0)
                            {
                                zminna_textbox_result.Text += "Pomylka: Dilennya na 0!\n";
                                zminna_uspish = false;
                            }
                            else
                            {
                                zminna_rezultat = zminna_chyslo1 / zminna_chyslo2;
                                zminna_textbox_result.Text += $"Operaciya: {zminna_chyslo1} / {zminna_chyslo2}\n";
                            }
                            break;
                    }

                    // Показуємо результат
                    if (zminna_uspish)
                    {
                        zminna_textbox_result.Text += $"\nREZULTAT\n";
                        zminna_textbox_result.Text += $"Rezultat: {zminna_rezultat}\n";
                    }
                    else
                    {
                        zminna_textbox_result.Text += "Rozrahunok ne vykonano!";
                    }
                }
                catch (Exception zminna_error)
                {
                    zminna_textbox_result.Text = $"Pomylka: {zminna_error.Message}";
                }
            };

            return zminna_tab;
        }

        // ЗАВДАННЯ 4 - Запуск стандартних програм
        private TabPage CreateTab4()
        {
            // Створюємо вкладку
            TabPage zminna_tab = new TabPage("Zavdannya 4 - Standartni Prohrawy");

            // Кнопка для Notepad
            Button zminna_button_notepad = new Button();
            zminna_button_notepad.Text = "Zapustyty Notepad";
            zminna_button_notepad.Location = new System.Drawing.Point(10, 10);
            zminna_button_notepad.Width = 120;
            zminna_button_notepad.Height = 40;
            zminna_tab.Controls.Add(zminna_button_notepad);

            // Кнопка для Калькулятора
            Button zminna_button_calc = new Button();
            zminna_button_calc.Text = "Zapustyty Kalkulator";
            zminna_button_calc.Location = new System.Drawing.Point(140, 10);
            zminna_button_calc.Width = 120;
            zminna_button_calc.Height = 40;
            zminna_tab.Controls.Add(zminna_button_calc);

            // Кнопка для Paint
            Button zminna_button_paint = new Button();
            zminna_button_paint.Text = "Zapustyty Paint";
            zminna_button_paint.Location = new System.Drawing.Point(270, 10);
            zminna_button_paint.Width = 120;
            zminna_button_paint.Height = 40;
            zminna_tab.Controls.Add(zminna_button_paint);

            // Напис для своєї програми
            Label zminna_label_custom = new Label();
            zminna_label_custom.Text = "Svoya prohrama:";
            zminna_label_custom.Location = new System.Drawing.Point(10, 60);
            zminna_label_custom.Width = 100;
            zminna_tab.Controls.Add(zminna_label_custom);

            // Текстбокс для введення своєї програми
            TextBox zminna_textbox_custom = new TextBox();
            zminna_textbox_custom.Location = new System.Drawing.Point(110, 60);
            zminna_textbox_custom.Width = 250;
            zminna_textbox_custom.Text = "notepad.exe";
            zminna_tab.Controls.Add(zminna_textbox_custom);

            // Кнопка для запуску своєї програми
            Button zminna_button_custom = new Button();
            zminna_button_custom.Text = "Zapustyty";
            zminna_button_custom.Location = new System.Drawing.Point(370, 60);
            zminna_button_custom.Width = 100;
            zminna_button_custom.Height = 25;
            zminna_tab.Controls.Add(zminna_button_custom);

            // Текстбокс для логу
            TextBox zminna_textbox_log = new TextBox();
            zminna_textbox_log.Location = new System.Drawing.Point(10, 100);
            zminna_textbox_log.Width = 500;
            zminna_textbox_log.Height = 420;
            zminna_textbox_log.Multiline = true;
            zminna_textbox_log.ReadOnly = true;
            zminna_textbox_log.ScrollBars = ScrollBars.Vertical;
            zminna_tab.Controls.Add(zminna_textbox_log);

            // Notepad
            zminna_button_notepad.Click += (sender, e) =>
            {
                try
                {
                    // Запускаємо notepad.exe
                    Process zminna_p = Process.Start("notepad.exe");
                    Zapysaty_V_Log(zminna_textbox_log, $"Notepad zapusheno! PID: {zminna_p.Id}");
                }
                catch (Exception zminna_err)
                {
                    Zapysaty_V_Log(zminna_textbox_log, $"Pomylka: {zminna_err.Message}");
                }
            };

            // Calculator
            zminna_button_calc.Click += (sender, e) =>
            {
                try
                {
                    // Запускаємо calc.exe
                    Process zminna_p = Process.Start("calc.exe");
                    Zapysaty_V_Log(zminna_textbox_log, $"Kalkulator zapusheno! PID: {zminna_p.Id}");
                }
                catch (Exception zminna_err)
                {
                    Zapysaty_V_Log(zminna_textbox_log, $"Pomylka: {zminna_err.Message}");
                }
            };

            // Paint
            zminna_button_paint.Click += (sender, e) =>
            {
                try
                {
                    // Запускаємо mspaint.exe
                    Process zminna_p = Process.Start("mspaint.exe");
                    Zapysaty_V_Log(zminna_textbox_log, $"Paint zapusheno! PID: {zminna_p.Id}");
                }
                catch (Exception zminna_err)
                {
                    Zapysaty_V_Log(zminna_textbox_log, $"Pomylka: {zminna_err.Message}");
                }
            };

            // Custom
            zminna_button_custom.Click += (sender, e) =>
            {
                try
                {
                    // Беремо шлях до програми з текстбокса
                    string zminna_prog_path = zminna_textbox_custom.Text;

                    if (string.IsNullOrWhiteSpace(zminna_prog_path))
                    {
                        Zapysaty_V_Log(zminna_textbox_log, "Vvedit shlyakh do prohrawy!");
                        return;
                    }

                    // Запускаємо програму
                    Process zminna_p = Process.Start(zminna_prog_path);
                    Zapysaty_V_Log(zminna_textbox_log, $"Prohrama {zminna_prog_path} zapushena! PID: {zminna_p.Id}");
                }
                catch (Exception zminna_err)
                {
                    Zapysaty_V_Log(zminna_textbox_log, $"Pomylka: {zminna_err.Message}");
                }
            };

            return zminna_tab;
        }

        // Допоміжна функція для запису в лог
        private void Zapysaty_V_Log(TextBox zminna_box, string zminna_text)
        {
            // Додаємо час і текст до логу
            zminna_box.Text += $"[{DateTime.Now:HH:mm:ss}] {zminna_text}\n";
            // Переміщаємо курсор на кінець
            zminna_box.SelectionStart = zminna_box.Text.Length;
            // Прокручуємо до курсору
            zminna_box.ScrollToCaret();
        }
    }

    // Клас для запуску додатка
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Створюємо стиль Windows Forms
            Application.EnableVisualStyles();

            // Запускаємо застосунок
            Application.Run(new MainForm());
        }
    }
}