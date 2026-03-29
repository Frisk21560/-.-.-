using System;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Dapper3
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}