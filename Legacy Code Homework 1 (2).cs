using System;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    [DllImport("kernel32.dll")]
    public static extern bool Beep(uint zminna_chastota, uint zminna_trivalist);

    [DllImport("user32.dll")]
    public static extern bool MessageBeep(uint zminna_tip_zvuka);

    static void Main()
    {
        Console.WriteLine("Zavdannya 2: Zvukovi signaly");
        Console.WriteLine("Pochynayu vytvoryuvaty zvuky...\n");

        // Pershyy nabir zvukiv - zwychaynyy Beep
        Console.WriteLine("Nabir 1: Zvychayni beepy (1000 Hz)");
        for (int i = 1; i <= 3; i++)
        {
            Console.WriteLine($"Beep {i}...");
            Beep(1000, 300);  // 1000 Hz, 300 ms
            Thread.Sleep(500); // Pauza 500 ms
        }

        Thread.Sleep(1000); // Dovsha pauza mizh naboramy

        // Druhyy nabir - zvuky z riznymy chastotamy
        Console.WriteLine("\nNabir 2: Zukovy syhnal - skhodyayushchy zvuk");
        uint[] zminna_chastoty = { 1500, 1200, 900, 600, 300 };
        foreach (uint zminna_ch in zminna_chastoty)
        {
            Console.WriteLine($"Chastota: {zminna_ch} Hz");
            Beep(zminna_ch, 200);
            Thread.Sleep(400);
        }

        Thread.Sleep(1000);

        // Tretiy nabir - MessageBeep (sistemni zvuky Windows)
        Console.WriteLine("\nNabir 3: Systemni zvuky Windows");

        Console.WriteLine("Zvuk: Default (0)");
        MessageBeep(0);
        Thread.Sleep(800);

        Console.WriteLine("Zvuk: OK (0xFFFFFFFF)");
        MessageBeep(0xFFFFFFFF);
        Thread.Sleep(800);

        Console.WriteLine("Zvuk: Hand (0x00000000)");
        MessageBeep(0);
        Thread.Sleep(800);

        // Chetvertyy nabir - shvydkyy shumy zvukiv
        Console.WriteLine("\nNabir 4: Shvydkyy shumy zvukiv");
        for (int i = 1; i <= 5; i++)
        {
            Beep(2000 - (i * 300), 100);
            Thread.Sleep(150);
        }

        Console.WriteLine("\nUsi zvuky vykonchanі!");
    }
}