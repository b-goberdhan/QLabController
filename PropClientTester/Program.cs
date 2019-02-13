using DeviceInterface.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropClientTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var device = new SerialPortDevice<SensorData>(4, "Arduino1");
            device.Recieved += Device_Recieved;
            device.Connect();
            while(true) { }
        }

        private static void Device_Recieved(Device<SensorData> device, SensorData response)
        {
            Console.WriteLine(response.Name + ": " + response.SensorValue);
        }
    }
    class SensorData
    {
        public string Name { get; set; }
        public long SensorValue { get; set; }
    }
}
