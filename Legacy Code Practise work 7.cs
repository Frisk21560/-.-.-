namespace Legacy_Code_Practies_work_7
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Vklyuchaemo vizualni styli dlya komponentiv
            Application.EnableVisualStyles();
            // Vstanovlyuyemo shablon dlya vyvedennya tekstu
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MouseHookApp.Form2());
        }
    }
}