
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Legacy_Code_Homework_8
{
    // ZAVDANNYA 2

    public partial class Form2 : Form
    {
        // Delegate dlya mouse hook callback
        private delegate IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam);
        // Delegate dlya keyboard hook callback
        private delegate IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Struktura dlya myshi koordinat
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        // Konstanty dlya myshi hooka
        private const int WH_MOUSE_LL = 14;
        private const int WM_MOUSEMOVE = 0x0200;

        // Konstanty dlya klaviatury
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int VK_LALT = 0xA4;
        private const int VK_RALT = 0xA5;

        // Zminnye dlya hookiv
        private static MouseHookProc mouseProc = MouseHookCallback;
        private static KeyboardHookProc keyboardProc = KeyboardHookCallback;
        private static IntPtr mouseHookId = IntPtr.Zero;
        private static IntPtr keyboardHookId = IntPtr.Zero;

        // Zminna dlya vidstriyuvannya Alt stanu
        private static bool altNatysnuta = false;

        // Instanciya formy dlya dostapa z callback
        private static Form2 formInstance = null;

        // Parametry kvadrata sho obmezhuye cursor
        // Kvadrat 500x500 pikseliv
        private const int SQUARE_SIZE = 500;
        // Pochatkok x (centrovano na ekrani)
        private int squareX;
        // Pochatkok y (centrovano na ekrani)
        private int squareY;

        public Form2()
        {
            InitializeComponent();
            formInstance = this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "Cursor Lock App";
            this.Size = new System.Drawing.Size(600, 500);

            // Vyrahovuemo centr ekrana dlya kvadrata
            // Screen.PrimaryScreen.Bounds - rozmir ekrana
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Poznachuemo kvadrat v centri ekrana
            squareX = (screenWidth - SQUARE_SIZE) / 2;
            squareY = (screenHeight - SQUARE_SIZE) / 2;

            // Vstanovlyuemo hookiv
            mouseHookId = SetMouseHook(mouseProc);
            keyboardHookId = SetKeyboardHook(keyboardProc);

            // Label dlya informaciy
            label1.Text = "Natysnit Alt dlya blokuvannya rukhu myshi v kvadrati";
        }

        // Funkciya dlya vstanovlennya myshi hooka
        private static IntPtr SetMouseHook(MouseHookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Funkciya dlya vstanovlennya klaviatury hooka
        private static IntPtr SetKeyboardHook(KeyboardHookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Callback dlya myshi rukhu
        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                // Yakscho ye event rukhu myshi
                if (wParam == (IntPtr)WM_MOUSEMOVE)
                {
                    // Yakscho Alt natysnuta - zastosovuemo obmezhennya
                    if (altNatysnuta && formInstance != null)
                    {
                        // Vytyaguemo strukturu z hooka
                        MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                        // Berem globalni koordinaty myshi
                        int mouseX = hookStruct.pt.x;
                        int mouseY = hookStruct.pt.y;

                        // Oznachuemo kvadrat
                        int squareRight = formInstance.squareX + SQUARE_SIZE;
                        int squareBottom = formInstance.squareY + SQUARE_SIZE;

                        // Zminnyye dlya novoyi pozyciy
                        int newX = mouseX;
                        int newY = mouseY;

                        // Pereviry chy cursor za mezhamy kvadrata za x
                        if (mouseX < formInstance.squareX)
                            newX = formInstance.squareX;
                        else if (mouseX > squareRight)
                            newX = squareRight;

                        // Pereviry chy cursor za mezhamy kvadrata za y
                        if (mouseY < formInstance.squareY)
                            newY = formInstance.squareY;
                        else if (mouseY > squareBottom)
                            newY = squareBottom;

                        // Yakscho cursor vyshov za mezhay - vratyayemo jogo nazad
                        if (newX != mouseX || newY != mouseY)
                        {
                            // SetCursorPos - vstanovlyuemo novyy pozyciju cursora
                            SetCursorPos(newX, newY);
                            // Vovertayemo 1 dlya zabuty ciyeyi podiyi
                            return (IntPtr)1;
                        }
                    }
                }
            }

            // Peredayemo dali
            return CallNextHookEx(mouseHookId, nCode, wParam, lParam);
        }

        // Callback dlya klaviatury
        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                // Vytyaguemo virtual key code
                int vkCode = Marshal.ReadInt32(lParam);

                // Yakscho ye keydown podiya
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    // Pereviry chy Alt natysnuta (lyavyy abo pravyy)
                    if (vkCode == VK_LALT || vkCode == VK_RALT)
                    {
                        altNatysnuta = true;
                    }
                }
                // Yakscho ye keyup podiya
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    // Pereviry chy Alt vidpuscheno
                    if (vkCode == VK_LALT || vkCode == VK_RALT)
                    {
                        altNatysnuta = false;
                    }
                }
            }

            // Peredayemo dali
            return CallNextHookEx(keyboardHookId, nCode, wParam, lParam);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Vidklyuchaemo hookiv
            UnhookWindowsHookEx(mouseHookId);
            UnhookWindowsHookEx(keyboardHookId);
            base.OnFormClosed(e);
        }

        // DLL imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, Delegate lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
    }
}