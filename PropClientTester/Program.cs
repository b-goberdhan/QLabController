using QLabPropInterface.Devices;
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
            var device = new SerialPortDevice<ExampleA>(4, "Arduino1");
            device.Recieved += Device_Recieved;
            device.Open();
            while(true) { }
        }

        private static void Device_Recieved(Device<ExampleA> device, ExampleA response)
        {
            Console.WriteLine("msg recieved!");
        }
    }
    class ExampleA
    {
        public string hello { get; set; }
        public int hi { get; set; }
    }
}
