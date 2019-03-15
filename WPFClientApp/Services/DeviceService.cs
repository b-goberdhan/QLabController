using DeviceInterface.Delegates;
using DeviceInterface.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClientApp.Services
{
    public class DeviceService
    {
        private Device<DeviceSensorModel> _device;
        public event MessageRecievedHandler<DeviceSensorModel> MessageRecieved;
        public void ConnectToCOM(int port)
        {
            _device = new SerialPortDevice<DeviceSensorModel>(port, "Arduino");
            _device.Recieved += Device_Data_Recieved;
            _device.Connect();
            
        }

        public void ConnectToTCP(string ipAddress)
        {

        }
        private void Device_Data_Recieved(Device<DeviceSensorModel> device, DeviceSensorModel response)
        {
            MessageRecieved?.Invoke(device, response);
        }
    }
}
