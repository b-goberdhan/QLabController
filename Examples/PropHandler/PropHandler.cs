using DeviceInterface.Devices;
using System;

namespace PropHandler
{
    public class PropHandler
    {
        public static Device<TData> ConnectToPropUsingSerial<TData>(int port, string name="ArduinoProp") where : TData
        {
            Console.Clear();
            Console.WriteLine("Enter the COM port of the device");
            Device<TData> device;
            while (true)
            {
                string number = Console.ReadLine();
                int portNum;
                if (int.TryParse(number, out portNum))
                {
                    try
                    {
                        device = new SerialPortDevice<TData>(portNum, "ArduinoBoxProp");
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
    }
}
