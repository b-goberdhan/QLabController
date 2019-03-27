using BoxPropServer.DataModels;
using BoxPropServer.DataModels.QLab;
using BoxPropServer.DataModels.Sensors;
using BoxPropServer.Extensions;
using DeviceInterface.Devices;
using QLabOSCInterface;
using QLabOSCInterface.QLabClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoxPropServer
{
    class Program
    {
        static QLabOSCClient _qLabClient;
        static WorkSpace _workspace;
        
        static bool connectedToQLab = false;

        static Group OrientationSensorGroup;
        static string LightSensorCueId;
        static string FlexSensorCueId;
        const bool IS_TESTING_DEVICE = false;
        static void Main(string[] args)
        {
            Console.WriteLine("This is the simple prop theatre device interface");
            Console.WriteLine("Created by Brandon Goberdhansingh");
            Console.WriteLine("University of Calgary");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Device<Sensors> device = ChooseDeviceInterface();
            device.Connect();
            SetupQLab().Wait();
            SetupPropEffect().Wait();
            device.Recieved += Device_Recieved;
            while (true) ;
            
        }
        private static async void Device_Recieved(Device<Sensors> device, Sensors sensors)
        {
            if (connectedToQLab || IS_TESTING_DEVICE)
            {
                PrintSensorData(sensors);
                await RunPropEffect(sensors);
            }
        }

        private static Device<Sensors> ChooseDeviceInterface()
        {
            Console.WriteLine("Please choose the interface your device will be using for comunication:");
            Console.WriteLine("Press 1 for Serial Port");
            Console.WriteLine("Press 2 for Config and Connect WAN AP");
            Console.WriteLine("Press 3 for Connect WAN AP");
            Console.WriteLine("Press C to exit the program");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.D3:
                            return ConnectToTCPDeviceAP();
                        case ConsoleKey.D2:
                            return ConfigureTcpNetworkDeviceAP();
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            return ConfigureSerialDevice();
                        case ConsoleKey.C:
                            Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static Device<Sensors> ConnectToTCPDeviceAP()
        {
            Console.Clear();
            Console.WriteLine("Connect enter prop IP Address: ");
            string ip = Console.ReadLine();

            return new TCPNetworkDevice<Sensors>(ip, 53005, "ArduinoProp");
        }

        private static bool isDoneConfig = false;
        private static bool isAPSetup = false;
        private static string deviceIpAddress;
        private static Device<Sensors> ConfigureTcpNetworkDeviceAP()
        {
            //first connect over serial to the device
            Console.Clear();
            Console.WriteLine("Setting up prop over usb...");
            Console.WriteLine("Enter the WiFi network SSID: ");
            string ssid = Console.ReadLine();
            Console.WriteLine("Enter the Wifi network Password: ");
            string pwd = Console.ReadLine();

            var serialConfig = new SerialPortDevice<Config>(4, "ArduinoConfig");
            serialConfig.Recieved += SerialConfigDevice_Recieved;
            serialConfig.Connect();
            serialConfig.Send(ssid);
            serialConfig.Send(pwd);
            while(!isDoneConfig)
            {
                //just keep running...
            }
            Console.WriteLine("Recieved config data from prop, press enter to continue");
            Console.ReadLine();
            serialConfig.Recieved -= SerialConfigDevice_Recieved;
            serialConfig.Disconnect();
            serialConfig.Dispose();

            var tcpDevice = new TCPNetworkDevice<Sensors>(deviceIpAddress, 53005, "ArduinoProp");
            Console.WriteLine("Now recieving data, you may now disconnect the device");
            return tcpDevice;
        }
        private static void SerialConfigDevice_Recieved(Device<Config> device, Config response)
        {            
            if (response.Data == 0)
            {
                Console.WriteLine("AP Setup Running...");
                isAPSetup = true;
            }
            else if (isAPSetup && !isDoneConfig)
            {
                byte[] bytes = BitConverter.GetBytes(response.Data);
                deviceIpAddress = new IPAddress(bytes).ToString();
                Console.WriteLine("Recieved IP Address: " + deviceIpAddress);
                isDoneConfig = true;               
            }
            else if (response.Data == -3)
            {
                Console.WriteLine("ERROR");
            }
        }

        private static Device<Sensors> ConfigureSerialDevice()
        {
            Console.Clear();
            Console.WriteLine("Enter the COM port of the device");
            Device<Sensors> device;
            //First upload code///

            while (true)
            {
                string number = Console.ReadLine();
                int portNum;
                if (int.TryParse(number, out portNum))
                {
                    try
                    {
                        device = new SerialPortDevice<Sensors>(portNum, "ArduinoBoxProp");
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
        private static void PrintSensorData(Sensors sensors)
        {
            Console.Clear();
            Console.WriteLine(sensors.ToString());
        }

        private static async Task SetupQLab()
        {
            if (IS_TESTING_DEVICE) return;
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
                catch
                {
                    Console.WriteLine("Could not connect to QLab please enter another Ip Address");
                }
            }
            _qLabClient = client;
            // Now select a work space.
            var workspaces = (await _qLabClient.GetWorkSpaces()).data;
            int count = 0;
            foreach (WorkSpace workspace in workspaces)
            {
                Console.WriteLine("[" + count + "] " + workspace.displayName);
                count++;
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
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid workspace secified, please try again");
                    }
                }

            }
           
            
            connectedToQLab = true;
        }

        private static async Task SetupPropEffect()
        {
            if (IS_TESTING_DEVICE) return;
            Console.WriteLine("Choose the lighting effect you wish to run:");
            Console.WriteLine("[0] Light Sensor effect");
            Console.WriteLine("[1] Orientation Sensor effect");
            Console.WriteLine("[2] Flex Sensor effect");
            while (true)
            {
                int number;
                if (int.TryParse(Console.ReadLine(), out number))
                {
                    if (number == 0)
                    {
                        LightSensorCueId = await _qLabClient.SetupLightSensorEffect(_workspace.uniqueID);
                        break;
                    }
                    else if (number == 1)
                    {
                        OrientationSensorGroup = await _qLabClient.SetupOrientationSensorGridEffect(_workspace.uniqueID);
                        break;
                    }
                    else if (number == 2)
                    {
                        FlexSensorCueId = await _qLabClient.SetupFlexSensorEffect(_workspace.uniqueID);
                        break;
                    }
                }
            }
        }
        private static async Task RunPropEffect(Sensors sensors)
        {
            if (!string.IsNullOrEmpty(LightSensorCueId))
            {
                await _qLabClient.RunLightSensorEffect(LightSensorCueId, _workspace.uniqueID, sensors.LightSensor);
            }
            else if (OrientationSensorGroup != null)
            {
                await _qLabClient.RunOrientationSensorGridEffect(_workspace.uniqueID, OrientationSensorGroup, sensors.OrientationSensor);
            }
            else if (!string.IsNullOrEmpty(FlexSensorCueId))
            {
                await _qLabClient.RunFlexSensorEffect(_workspace.uniqueID, FlexSensorCueId, sensors.FlexSensor);
            }
        }
    }

}
