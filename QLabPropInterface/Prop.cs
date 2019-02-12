
using PropInterface.PropObjects;
using QLabPropInterface.Devices;
using System;

namespace QLabPropInterface
{
    public class Prop
    {
        private readonly Device<PropData> _device;
        public Prop(Device<PropData> device)
        {
            _device = device;

        }
        private void RegisterEvents()
        {
            _device.Recieved += Device_Recieved;
        }

        private void Device_Recieved(Device<PropData> device, PropData response)
        {
            throw new NotImplementedException();
        }

    }
    
}
