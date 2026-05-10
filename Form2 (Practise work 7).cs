using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Legacy_Code_Practies_work_7
{
    // ZAVDANNYA 2 - Knopka sho vtikaye vid myshi

    public partial class Form2 : Form
    {
        // Delegate dlya mouse hook callback
        // Vykorystovuemo jogo dlya funcij sho perehopljuyut podiyi myshi
        private delegate IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Struktura dlya zberihannya koordinat myshi
        // LayoutKind.Sequential - znachyt sho dannyye zberigsyatsya odyn za odynym
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            // x - koordinata po horyzontaly
            public int x;
            // y - koordinata po vertikaly
            public int y;
        }

        // Struktura dlya hooka myshi
        // Mistyt informatsiyu pro mysh event
        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            // pt - tochka de buv klyk (POINT struktura)
            public POINT pt;
            // mouseData - dodatkovi dannyye
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        // Konstanty dlya mouse hooka
        // WH_MOUSE_LL = 14 - ce kod dlya global mouse hooka
        private const int WH_MOUSE_LL = 14;
        // WM_LBUTTONDOWN = 0x0201 - ce kod podiji levoho kliku myshi
        private const int WM_LBUTTONDOWN = 0x0201;

        // Zminnye dlya hooka
        // proc - callback funkciya sho bude vyskana na podii myshi
        private static MouseHookProc proc = MouseHookCallback;
        // hookId - ID hooka dlya vyklyuchennya
        private static IntPtr hookId = IntPtr.Zero;
        // instanciya - statychna zminna dlya dostupa z callback funkcij
        private static Form2 instanciya = null;
        // random - generator vypadkovykh chysel dlya rukhu knopky
        private Random random = new Random();

        public Form2()
        {
            InitializeComponent();
            // Zuberihayemo instanciyu formy v statichniy zminniy
            instanciya = this;
        }

        // Podiya koly forma zavantuetsya
        private void Form2_Load(object sender, EventArgs e)
        {
            // Nastavlyayem nazvu formy
            this.Text = "Evading Button Game";
            // Nastavlyayem rozmir formy (500 szerokist, 400 vysota)
            this.Size = new System.Drawing.Size(500, 400);

            // Vstanovlyuemo hook na mysh
            // Dali kozhnyy klyk myshi bude perehopljuvannya
            hookId = SetHook(proc);
        }

        // Funkciya dlya vstanovlennya hooka
        // Regestruemo callback funkciju v systemi
        private static IntPtr SetHook(MouseHookProc proc)
        {
            // Berem iformation pro aktualnuy proces
            using (Process curProcess = Process.GetCurrentProcess())
            // Berem modul procesu
            using (ProcessModule curModule = curProcess.MainModule)
            {
                // Vstanovlyuemo hook na mysh i vovertayemo jogo ID
                // WH_MOUSE_LL - global mouse hook
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Callback dlya perehoplennya myshi
        // Bude vyskana kozhnyy raz koly budet podia z myshyu
        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Yakscho nCode >= 0, to ye podiya sho treba opracyuvaty
            if (nCode >= 0)
            {
                // Yakscho ye left button down podiya (lyvyy klyk)
                if (wParam == (IntPtr)WM_LBUTTONDOWN)
                {
                    // Yakscho forma isnuye i knopka isnuye
                    if (instanciya != null && instanciya.knopka_evade != null)
                    {
                        // Konvertuemo lParam u MSLLHOOKSTRUCT strukturu
                        // Marshal.PtrToStructure - peretvoryuemo pokazyvach v strukturu
                        MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                        // Berem globalnyye koordinaty myshi z hooka
                        int mouseX = hookStruct.pt.x;
                        int mouseY = hookStruct.pt.y;

                        // Berem koordinaty knopky na ekrani
                        // RectangleToScreen - peretvoryuemo lokalni koordinaty u globalni
                        var buttonRect = instanciya.knopka_evade.RectangleToScreen(
                            instanciya.knopka_evade.ClientRectangle
                        );

                        // Pereviry chy mysh v mezhakh knopky
                        // Contains - metod shcho pereverya chy tochka v prямokutnyku
                        if (buttonRect.Contains(mouseX, mouseY))
                        {
                            // Klyk vidbuvsya po knopci - zminyuemo yiy pozyciju!

                            // Generuemo novu x koordinatu (vypadkova)
                            // random.Next(0, Width - knopka.Width) - chyslo v rangu
                            int newX = instanciya.random.Next(0,
                                instanciya.Width - instanciya.knopka_evade.Width - 20);

                            // Generuemo novu y koordinatu (vypadkova)
                            // -60 tomu sho vnyzu ye paneli forma
                            int newY = instanciya.random.Next(0,
                                instanciya.Height - instanciya.knopka_evade.Height - 60);

                            // Vstanovlyuyemo novu pozyciju knopky
                            instanciya.knopka_evade.Location = new System.Drawing.Point(newX, newY);

                            // Blokuemo click - vovertayemo 1
                            // Yakscho vovernemo 1, to click ne projde dali v systemu
                            // Eto znachyt scho knopka ne bude vykonana
                            return (IntPtr)1;
                        }
                    }
                }
            }

            // Yakscho ne v mezhakh knopky - peredayemo podiyu dali
            // Eto vazhnо, inakshe vsya mysh ne bude pracyuvaty
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        // Podiya koly formu zakryvayut
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Vidklyuchaemo hook koly forma zakryvayetsya
            // Vazhnо - bez togo programa bude prodovzhuvaty perehopljuvaty eventi
            UnhookWindowsHookEx(hookId);
            // Vyskuemo bazihnuy metod
            base.OnFormClosed(e);
        }

        // SetWindowsHookEx - vstanovlyuemo hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, MouseHookProc lpfn, IntPtr hMod, uint dwThreadId);

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