using DeviceInterface.Devices;
using QLabOSCInterface;
using QLabOSCInterface.QLabClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxPropServer
{
    class Program
    {
        static QLabOSCClient _client;
        static WorkSpace _workspace;
        static string cueId;
        static bool connectedToQLab = false;
        static void Main(string[] args)
        {
            Console.WriteLine("This is the simple prop theatre device interface");
            Device<SensorData> device = ChooseDeviceInterface();
            device.Recieved += Device_Recieved;
            device.Connect();
            Console.WriteLine("Connected!");
            SetupQLab().Wait();
            while(true)
            {

            }
            
        }

        private static void Device_Recieved(Device<SensorData> device, SensorData response)
        {
            if (connectedToQLab)
            {
                PrintSensorData(response);
            }


        }

        private static Device<SensorData> ChooseDeviceInterface()
        {
            Console.WriteLine("Please choose the interface your device will be using for comunication:");
            Console.WriteLine("Press 1 for Serial Port");
            Console.WriteLine("Press 2 for LAN");
            Console.WriteLine("Press C to exit the program");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.D2:
                            break;
                        case ConsoleKey.D1:
                            return ConfigureSerialDevice();
                        default:
                            break;
                    }
                }
            }
        }
        private static Device<SensorData> ConfigureSerialDevice()
        {
            Console.Clear();
            Console.WriteLine("Enter the COM port of the device");
            Device<SensorData> device;
            while (true)
            {
                string number = Console.ReadLine();
                int portNum;
                if (int.TryParse(number, out portNum))
                {
                    try
                    {
                        device = new SerialPortDevice<SensorData>(portNum, "ArduinoBoxProp");
                        device.Connect();
                        Console.WriteLine("Connected");
                        return device;
                    }
                    catch
                    {
                        Console.WriteLine("Could not connect to device, please select another COM port");
                    }
                }
            }
        }
        private static void PrintSensorData(SensorData response)
        {
            Console.Clear();
            Console.WriteLine("Sensor: " + response.Name);
            Console.WriteLine("Value: " + response.SensorValue);

        }
        private static async Task SetupQLab()
        {
            Console.Clear();
            Console.WriteLine("Connect to QLab");
            Console.WriteLine("Provide Ip Address");
            QLabOSCClient client;
            while (true)
            {
                string ipAddress = Console.ReadLine();
                try
                {
                    client = new QLabOSCClient(ipAddress);
                    client.Connect();
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not connect to QLab please enter another Ip Address");
                }
            }
            _client = client;
            // Now select a work space.
            var workspaces = (await _client.GetWorkSpaces()).data;
            int count = 0;
            foreach (WorkSpace workspace in workspaces)
            {
                Console.WriteLine("[" + count + "] " + workspace.displayName);
            }

            //Now choose a workspace
            Console.WriteLine("Choose the workspace you want to connect to");
            while (true)
            {
                int number;
                if(int.TryParse(Console.ReadLine(), out number))
                {
                    try
                    {
                        _workspace = workspaces[number];
                        Console.WriteLine("Connected to Workspace: " + _workspace.displayName);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid workspace secified, please try again");
                    }
                }

            }


        }
    }

    class SensorData
    {
        public string Name { get; set; }
        public long SensorValue { get; set; }
    }
}
