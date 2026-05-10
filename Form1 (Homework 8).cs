using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Legacy_Code_Homework_8
{
    // ZAVDANNYA 1

    public partial class Form1 : Form
    {
        // Delegate dlya hook callback
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Konstanty dlya keyboard hooka
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        // Klyuchi dlya kombinaciy
        private const int VK_LCONTROL = 0xA2;
        private const int VK_RCONTROL = 0xA3;
        private const int VK_LSHIFT = 0xA0;
        private const int VK_RSHIFT = 0xA1;
        private const int VK_Q = 0x51;

        // Zminnye dlya vidstriyuvannya stanu klavish
        // Vikorostovuyemo masiv true/false dlya kozhnoy klavishi
        private static bool[] klavishshyStany = new bool[256];

        // Zminnye dlya hooka
        private static HookProc proc = HookCallback;
        private static IntPtr hookId = IntPtr.Zero;
        // formInstance - referenciya na formu dlya dostupa z callback
        private static Form1 formInstance = null;

        public Form1()
        {
            InitializeComponent();
            formInstance = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Nastavlyayem nazvu formy
            this.Text = "Hide/Show Window App";

            // Vstanovlyuemo hook dlya vidstriyuvannya klavish
            hookId = SetHook(proc);

            // Dodayemo label dlya inforrmaciy
            label1.Text = "Natysnit Ctrl + Shift + Q dlya prykhovennya/pokazuvannya vikna";
        }

        // Funkciya dlya vstanovlennya hooka
        private static IntPtr SetHook(HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Callback dlya perehoplennya klavish
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                // Vytyaguemo virtual key code z hooka
                int vkCode = Marshal.ReadInt32(lParam);

                // Yakscho ye keydown podiya
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    // Vstanovlyuyemo dlya ciyeyi klavishi True (natysnuta)
                    klavishshyStany[vkCode] = true;

                    // Pereviry kombinaciy
                    // Pereverya chy Ctrl natysnuta (lyavyy abo pravyy)
                    bool ctrlNatysnuta = klavishshyStany[VK_LCONTROL] || klavishshyStany[VK_RCONTROL];
                    // Pereviry chy Shift natysnuta
                    bool shiftNatysnuta = klavishshyStany[VK_LSHIFT] || klavishshyStany[VK_RSHIFT];
                    // Pereviry chy Q natysnuta
                    bool qNatysnuta = klavishshyStany[VK_Q];

                    // Yakscho vsya kombinaciya natysnuta
                    if (ctrlNatysnuta && shiftNatysnuta && qNatysnuta)
                    {
                        // Pereovernuty Visible status (yakscho vydyma to prykhovaty, oba ne to pokazaty)
                        if (formInstance != null)
                        {
                            formInstance.Visible = !formInstance.Visible;
                        }
                    }
                }
                // Yakscho ye keyup podiya (vidpuscheno klavishu)
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    // Vstanovlyuyemo dlya ciyeyi klavishi False (vidpuscheno)
                    klavishshyStany[vkCode] = false;
                }
            }

            // Peredayemo dali po lancyugu
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Vidklyuchaemo hook
            UnhookWindowsHookEx(hookId);
            base.OnFormClosed(e);
        }

        // DLL imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}