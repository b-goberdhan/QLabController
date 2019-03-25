using BoxPropServer.DataModels;
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
        static string _cueId;
        static bool connectedToQLab = false;
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
            //SetupQLab().Wait();
            device.Recieved += Device_Recieved;
            while (true)
            {

            }
            
        }
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        private static async void Device_Recieved(Device<Sensors> device, Sensors sensors)
        {
            PrintSensorData(sensors);
            if (connectedToQLab)
            {
                PrintSensorData(sensors);
                // if a new request comes before the current one is complete
                // we will just ignore this message (again, the longest this request would
                // take would be ~100ms).
                await _qLabClient.LightSensorEffect(_cueId, _workspace.uniqueID, sensors.LightSensor);


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
            // Finally create a cue that will be used to adjust lighting.
            _cueId =  (await _qLabClient.CreateWorkSpaceCue(_workspace.uniqueID, QLabOSCInterface.Enums.CueType.Light)).data;
            await _qLabClient.SetCueDuration(_workspace.uniqueID, _cueId, 0);
            Console.WriteLine("Cue Id used: " + _cueId);
            await Task.Delay(1000);
            connectedToQLab = true;
        }
        private static async Task SendSensorDataToQLab(LightSensor data)
        {
            // Stop the running cue, we need to set a new value then run it
            await _qLabClient.HardStopCue(_workspace.uniqueID, _cueId, 50);
            // the max value from the light sensor is ~~1000
            // we want the value to be in relation to the max value of light in qlab which is 255
            float lightIntensity = (((float)data.Intensity / 50) * 100f);
            if (lightIntensity > 50)
            {
                lightIntensity = 50;
            }
            else if (lightIntensity < 0)
            {
                lightIntensity = 0;
            }
            string lightingCommand = "all.intensity = " + lightIntensity;
            // we will timeout for 100ms since the arduino sends messges every 100ms
            await _qLabClient.SetCueLightCommand(_workspace.uniqueID, _cueId, lightingCommand, 0);   
            await _qLabClient.StartCue(_workspace.uniqueID, _cueId, 0);

        }
    }

}
