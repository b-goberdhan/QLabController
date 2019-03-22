using System.IO.Ports;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DeviceInterface.Delegates;
using System;

namespace DeviceInterface.Devices
{
    public class SerialPortDevice<TData> : Device<TData>
    {
        public int BaudRate { get; private set; }
        public int PortNumber { get; private set; }
        
        private SerialPort _serialPort;
        private Task _reciever;
        private bool _isDisposed;
        protected override Func<string> BackgroundReciever
        {
            get
            {
                return () =>
                {
                    string json = "";
                    if (!_isDisposed)
                    {
                        try
                        {
                            json = _serialPort.ReadLine();
                        }
                        catch
                        {
                            //do nothing
                        }
                        
                    }
                    return json;
                };
            }
        }
        public SerialPortDevice(int portNumber, string name, int baudRate = 9600) : base(name)
        {
            BaudRate = baudRate;
            PortNumber = portNumber;
            _serialPort = new SerialPort("COM" + portNumber, baudRate);
        }
        public override void Connect()
        {
            if (!IsConnected)
            {
                _serialPort.Open();
                base.Connect();
            }
            
        }
        public override void Disconnect()
        {
            if (IsConnected)
            {
                _serialPort.Close();
                base.Disconnect();
            }
            
        }
        public override void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _reciever?.Dispose();
                _serialPort?.Close();
                _serialPort?.Dispose();
                base.Dispose();
            }
        }
    }
}
