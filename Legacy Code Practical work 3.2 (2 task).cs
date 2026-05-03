namespace Legacy_Code_Homework_3._2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Коли натискаємо на кнопку, запускаємо приготування снідданку асинхронно
        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            // Очищаємо текстбокс перед тим як писати новий статус
            txtStatus.Clear();
            progressBar1.Value = 0;

            // Готуємо каву, чекаємо 2 секунди і виводимо статус
            txtStatus.AppendText("Вливаємо каву в чашку...\r\n");
            await Task.Delay(2000);
            txtStatus.AppendText("Кава готова!\r\n");
            progressBar1.Value = 20;

            // Готуємо яйця, це займає більше часу тому затримка довша
            txtStatus.AppendText("Беремо яйця...\r\n");
            await Task.Delay(1000);
            txtStatus.AppendText("Кладемо яйця на сковороду...\r\n");
            await Task.Delay(3000);
            txtStatus.AppendText("Яйця готові!\r\n");
            progressBar1.Value = 40;

            // Готуємо беконь, його найдовше готувати з усіх
            txtStatus.AppendText("Кладемо беконь на сковороду...\r\n");
            await Task.Delay(4000);
            txtStatus.AppendText("Беконь готовий!\r\n");
            progressBar1.Value = 60;

            // Готуємо тост, спочатку беремо хлеб потім чекаємо поки він пригорить
            txtStatus.AppendText("Беремо хлеб...\r\n");
            await Task.Delay(500);
            txtStatus.AppendText("Кладемо в тостер...\r\n");
            await Task.Delay(2000);
            txtStatus.AppendText("Тост готовий!\r\n");
            progressBar1.Value = 80;

            // Наливаємо сік в склянку
            txtStatus.AppendText("Наливаємо сік...\r\n");
            await Task.Delay(1500);
            txtStatus.AppendText("Сік готовий!\r\n");
            progressBar1.Value = 100;

            // Коли все готово, показуємо повідомлення що все закінчилось
            txtStatus.AppendText("\r\nСнідданок повністю готовий!");

            btnStart.Enabled = true;
        }
    }
}