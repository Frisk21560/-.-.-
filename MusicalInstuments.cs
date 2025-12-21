using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritence
{
    public class MusicalInstrument
    {
        public string Name { get; set; } = null!;
        public string? Characteristic { get; set; }
        public string? Origin { get; set; }

        public MusicalInstrument(string name, string? chara, string? origin)
        {
            Name = name;
            Characteristic = chara;
            Origin = origin;
        }

        public virtual void Sound()
        {
            Console.WriteLine($"The {Name} makes a sound: ...");
        }

        public virtual void Show()
        {
            Console.WriteLine($"Instrument: {Name}");
        }

        public virtual void Desc()
        {
            Console.WriteLine($"Description of {Name}: {Characteristic}");
        }

        public virtual void History()
        {
            Console.WriteLine($"History of {Name}: {Origin}");
        }
    }
}