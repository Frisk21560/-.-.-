using System;

namespace Inheritence
{
    public class Course
    {
        public string Name { get; set; } = null!;
        public int Duration { get; set; } // hours

        public Course(string name, int duration)
        {
            Name = name;
            Duration = duration;
        }

        public override string ToString()
        {
            return $"Course: {Name}, Duration: {Duration} hours";
        }
    }
}