using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr zminna_okna, string zminna_tekst, string zminna_title, uint zminna_type);

    static void Main()
    {
        Console.WriteLine("Zavdannya 1: Informaciya pro mene");

        // Persha informaciya
        string zminna_imya = "Mene zovut Ivan Petrovich";
        MessageBox(IntPtr.Zero, zminna_imya, "Imya", 0);

        // Druga informaciya
        string zminna_vік = "Meni 25 rokiv";
        MessageBox(IntPtr.Zero, zminna_vік, "Vik", 0);

        // Tretya informaciya
        string zminna_miscya = "Ya zhivu v Kyyevi";
        MessageBox(IntPtr.Zero, zminna_miscya, "Mistse prozhyvannya", 0);

        // Chetverta informaciya
        string zminna_pracia = "Ya praczyu yak prohramist";
        MessageBox(IntPtr.Zero, zminna_pracia, "Profesia", 0);

        // P'yata informaciya
        string zminna_hobby = "Meni podobaetcya progrmuvanya, igry ta muzyka";
        MessageBox(IntPtr.Zero, zminna_hobby, "Hobі", 0);

        Console.WriteLine("Vsi MessageBox pokazani!");
    }
}