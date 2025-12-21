using System;

namespace Inheritence
{
    public class Cello : MusicalInstrument
    {
        public Cello(string name, string? chara, string? origin)
            : base(name, chara, origin)
        {
        }

        public override void Sound()
        {
            Console.WriteLine($"{Name} sound: deep and warm bow notes");
        }

        public override void Show()
        {
            Console.WriteLine($"Show instrument: {Name} (cello)");
        }

        public override void Desc()
        {
            Console.WriteLine($"Cello desc: {Characteristic}");
        }

        public override void History()
        {
            Console.WriteLine($"Cello history: used for orchestras since long time in {Origin}");
        }
    }
}