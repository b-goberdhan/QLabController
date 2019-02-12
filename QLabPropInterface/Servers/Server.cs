using QLabPropInterface.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropInterface.Servers
{
    public class Server<TDevice, TData> where TDevice : Device<TData>
    {

        private List<TDevice> _devices;
        public Server()
        {

        }

        public void AddDevice(TDevice device)
        {
            if (!_devices.Contains(device))
            {
                device.Recieved += Device_Recieved;
                _devices.Add(device);
            }

            
        }
        public bool RemoveDevice(TDevice device)
        {
            return _devices.Remove(device);
        }

        private void Device_Recieved(Device<TData> device, TData response)
        {
            
        }
    }
}
