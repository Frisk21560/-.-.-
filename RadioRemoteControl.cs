using System;

namespace Interfaces
{
    public class RadioRemoteControl : IRemoteControl
    {
        public bool IsOn { get; private set; } = false;
        public int CurrentChannel { get; private set; } = 88; // simple number for fm

        public void TurnOn()
        {
            IsOn = true;
            Console.WriteLine("Radio: on now.");
        }

        public void TurnOff()
        {
            IsOn = false;
            Console.WriteLine("Radio: off now.");
        }

        public void SetChannel(int channel)
        {
            if (!IsOn)
            {
                Console.WriteLine("Radio: cannot change channel when off.");
                return;
            }

            if (channel < 70 || channel > 108)
            {
                Console.WriteLine("Radio: channel out of fm range (70-108), but i will set it anyway.");
            }

            CurrentChannel = channel;
            Console.WriteLine($"Radio: channel is now {CurrentChannel}");
        }
    }
}