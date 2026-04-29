using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr zminna_okna, string zminna_tekst, string zminna_title, uint zminna_type);

    static void Main()
    {
        Console.WriteLine("Zavdannya 2: Gra v vgadyvanie");

        bool zminna_igra_prodolzhayetsya = true;

        while (zminna_igra_prodolzhayetsya)
        {
            Console.WriteLine("\nZagadaj chislo vid 0 do 100 i primy Enter.");
            Console.ReadLine();

            int zminna_min = 0;
            int zminna_max = 100;
            int zminna_spoba = 0;

            bool zminna_naydeno = false;

            while (!zminna_naydeno)
            {
                zminna_spoba++;
                int zminna_vhadka = (zminna_min + zminna_max) / 2;

                Console.WriteLine($"Spoba {zminna_spoba}: Ya vgaduyu {zminna_vhadka}");
                Console.WriteLine("Vvedi: > (tvoe chislo bilshe), < (tvoe chislo menshe), = (verno!)");

                string zminna_vidpovid = Console.ReadLine();

                if (zminna_vidpovid == "=")
                {
                    zminna_naydeno = true;
                    string zminna_result_text = $"Ya vgadav tvoe chislo {zminna_vhadka} za {zminna_spoba} spob!";
                    MessageBox(IntPtr.Zero, zminna_result_text, "Peremoha!", 0);
                    Console.WriteLine(zminna_result_text);
                }
                else if (zminna_vidpovid == ">")
                {
                    zminna_min = zminna_vhadka + 1;
                }
                else if (zminna_vidpovid == "<")
                {
                    zminna_max = zminna_vhadka - 1;
                }
                else
                {
                    Console.WriteLine("Nepravilna vidpovid! Vvedi >, <, abo =");
                }
            }

            Console.WriteLine("\nChy vy khochete yzhe raz? (y/n)");
            string zminna_vybir = Console.ReadLine();

            if (zminna_vybir != "y" && zminna_vybir != "Y")
            {
                zminna_igra_prodolzhayetsya = false;
                MessageBox(IntPtr.Zero, "Spasibo za igru! Do svidannya!", "Konec igry", 0);
            }
        }
    }
}