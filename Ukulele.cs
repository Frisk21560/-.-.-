using System;

namespace Inheritence
{
    public class Ukulele : MusicalInstrument
    {
        public Ukulele(string name, string? chara, string? origin)
            : base(name, chara, origin)
        {
        }

        public override void Sound()
        {
            Console.WriteLine($"{Name} sound: light and happy strum");
        }

        public override void Show()
        {
            Console.WriteLine($"Show instrument: {Name} (ukulele)");
        }

        public override void Desc()
        {
            Console.WriteLine($"Ukulele desc: {Characteristic}");
        }

        public override void History()
        {
            Console.WriteLine($"Ukulele history: small guitar from {Origin}");
        }
    }
}