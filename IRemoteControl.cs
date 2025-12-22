using System;

namespace Interfaces
{
    // Remote control interface for devices
    public interface IRemoteControl
    {
        void TurnOn();
        void TurnOff();
        void SetChannel(int channel);
    }
}
