using System;

namespace Inheritence
{
    public class Trombone : MusicalInstrument
    {
        public Trombone(string name, string? chara, string? origin)
            : base(name, chara, origin)
        {
        }

        public override void Sound()
        {
            Console.WriteLine($"{Name} sound: brassy slide go brrr");
        }

        public override void Show()
        {
            Console.WriteLine($"Show instrument: {Name} (trombone)");
        }

        public override void Desc()
        {
            Console.WriteLine($"Trombone desc: {Characteristic}");
        }

        public override void History()
        {
            Console.WriteLine($"Trombone history: was used in old bands from {Origin}");
        }
    }
}