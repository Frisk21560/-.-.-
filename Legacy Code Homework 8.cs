namespace Legacy_Code_Homework_8
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Dlya zavdannya 1 - prykhovennya vikna
            // Application.Run(new HideShowApp.Form1());

            // Dlya zavdannya 2 - obmezhennya cursora
            Application.Run(new CursorLockApp.Form2());
        }
    }
}