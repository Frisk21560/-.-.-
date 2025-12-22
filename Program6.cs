using System;

namespace Inheritence
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1
            var myViolin = new Violin("OldViolin", "wood, 4 strings", "Italy");
            var myTrombone = new Trombone("BigSlide", "brass, slide", "Germany");
            var myUkulele = new Ukulele("TinyUke", "small, 4 strings", "Hawaii");
            var myCello = new Cello("DeepCello", "large, 4 strings", "Austria");

            MusicalInstrument[] instruments = { myViolin, myTrombone, myUkulele, myCello };

            Console.WriteLine("=== Instruments demo ===");
            foreach (var ins in instruments)
            {
                ins.Show();
                ins.Sound();
                ins.Desc();
                ins.History();
                Console.WriteLine();
            }

            // 2
            var c1 = new Course("BasicMusic", 10);
            var oc1 = new OnlineCourse("GuitarForBeginners", 20, "YouLearn");

            Console.WriteLine("=== Courses demo ===");
            Console.WriteLine(c1.ToString());
            Console.WriteLine(oc1.ToString());

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
