using System;

namespace Inheritence
{
    public class Violin : MusicalInstrument
    {
        public Violin(string name, string? chara, string? origin)
            : base(name, chara, origin)
        {
        }

        public override void Sound()
        {
            Console.WriteLine($"{Name} sound: screechy and sweet - bow on strings");
        }

        public override void Show()
        {
            Console.WriteLine($"Show instrument: {Name} (violin)");
        }

        public override void Desc()
        {
            Console.WriteLine($"Violin desc: {Characteristic}");
        }

        public override void History()
        {
            Console.WriteLine($"Violin history: made many years ago in {Origin}");
        }
    }
}