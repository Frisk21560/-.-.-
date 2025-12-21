using System;

namespace Inheritence
{
    public class OnlineCourse : Course
    {
        public string Platform { get; set; } = null!;

        public OnlineCourse(string name, int duration, string platform)
            : base(name, duration)
        {
            Platform = platform;
        }

        public override string ToString()
        {
            return $"Online course: {Name}, Duration: {Duration} hours, Platform: {Platform}";
        }
    }
}