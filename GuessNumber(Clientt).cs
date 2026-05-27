namespace GuessNumber_Clientt_
{

    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Inicializuyemo Win Forms
            ApplicationConfiguration.Initialize();

            // Zapuskayemo formu
            Application.Run(new Form1());
        }
    }
}