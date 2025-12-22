using System;

namespace Interfaces
{
    public class TvRemoteControl : IRemoteControl
    {
        public bool IsOn { get; private set; } = false;
        public int CurrentChannel { get; private set; } = 1;

        public void TurnOn()
        {
            IsOn = true;
            Console.WriteLine("TV: turned on. yay!");
        }

        public void TurnOff()
        {
            IsOn = false;
            Console.WriteLine("TV: turned off. bye!");
        }

        public void SetChannel(int channel)
        {
            if (!IsOn)
            {
                Console.WriteLine("TV: cant set channel, tv is off.");
                return;
            }

            if (channel < 1)
            {
                Console.WriteLine("TV: channel must be >= 1");
                return;
            }

            CurrentChannel = channel;
            Console.WriteLine($"TV: channel set to {CurrentChannel}");
        }
    }
}