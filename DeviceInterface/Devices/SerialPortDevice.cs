using System.IO.Ports;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DeviceInterface.Delegates;
using System.Threading;

namespace DeviceInterface.Devices
{
    public class SerialPortDevice<TData> : Device<TData>
    {
        public override event MessageRecievedHandler<TData> Recieved;
        public int BaudRate { get; private set; }
        public int PortNumber { get; private set; }
        
        private SerialPort _serialPort;
        private Task _reciever;
        private bool _isDisposed = false;
        private CancellationTokenSource _cancelReadSource = new CancellationTokenSource();

        public SerialPortDevice(int portNumber, string name, int baudRate = 9600) : base(name)
        {
            BaudRate = baudRate;
            PortNumber = portNumber;
            _serialPort = new SerialPort("COM" + portNumber, baudRate);
        }
        public override async void Connect()
        {
            _serialPort.Open();
            _reciever = Task.Run(() => RunBackground(), _cancelReadSource.Token);
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
                try
                {
                    var response = JsonConvert.DeserializeObject<TData>(json);
                    Recieved?.Invoke(this, response);
                }
                catch
                {
                    //Do nothing, the message is not valid!
                }               
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
        public override void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _cancelReadSource.Cancel();
                _reciever.Dispose();
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
                _serialPort.Dispose();
                
            }
        }
    }
}
