using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using PropInterface.Delegates;
using Newtonsoft.Json;

namespace QLabPropInterface.Devices
{
    public class SerialPortDevice<TData> : Device<TData>
    {
        public override event MessageRecievedHandler<TData> Recieved;
        public int BaudRate { get; private set; }
        public int PortNumber { get; private set; }
        
        private SerialPort _serialPort;
        private Task _reciever;
        private bool _isDisposed;

        public SerialPortDevice(int portNumber, string name, int baudRate = 9600) : base(name)
        {
            BaudRate = baudRate;
            PortNumber = portNumber;
            _serialPort = new SerialPort("COM" + portNumber, baudRate);
        }
        public override async void Open()
        {
            _serialPort.Open();
            _reciever = Task.Run(() => RunBackground());
            await _reciever;
        }
        protected override async Task RecvBackgroundAsync()
        {
            _reciever = Task.Run(() => RunBackground());
            await _reciever;
        }
        private void RunBackground()
        {
            while (!_isDisposed)
            {
                string json = _serialPort.ReadLine();
                var response = JsonConvert.DeserializeObject<TData>(json);
                Recieved?.Invoke(this, response);
            }
        }

        protected override async Task SendAsync(object message)
        {
            string serilizedMessage = JsonConvert.SerializeObject(message);
            await Task.Run(() => 
            {
                _serialPort.WriteLine(serilizedMessage);
            });
        }
    }
}
