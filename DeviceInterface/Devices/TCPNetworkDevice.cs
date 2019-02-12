using DeviceInterface.Delegates;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DeviceInterface.Devices
{
    /// <summary>
    /// For now this class is not functional, it will be used 
    /// when we get a hold of the arduino wifi model (MKR1000)
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class TCPNetworkDevice<TData> : Device<TData>, IDisposable
    {
        public EndPoint DeviceEndpoint { get; private set; }
       
        private readonly TcpClient _client;
        private Task _reciever;
        private bool _isDisposed;
        public override event MessageRecievedHandler<TData> Recieved;

        public TCPNetworkDevice(TcpClient client, string name) : base(name)
        {
            _client = client;
            DeviceEndpoint = _client.Client.RemoteEndPoint;
            Task.Run(RecvBackgroundAsync);
        }

        public override void Connect()
        {
            
        }

        protected override async Task SendAsync(object message)
        {
            NetworkStream stream = _client.GetStream();
            string serilizedMessage = JsonConvert.SerializeObject(message);
            byte[] date = Convert.FromBase64String(serilizedMessage);
            await stream.WriteAsync(date, 0, date.Length);
            stream.Dispose();
        }

        protected override async Task RecvBackgroundAsync()
        {
            _reciever = Task.Run(async() =>
            {
                NetworkStream stream = _client.GetStream();
                int i = 0;
                byte[] buffer = new byte[256];
                while(!_isDisposed)
                {
                    if(_client?.Connected != true)
                    {
                        stream.Close();
                        stream.Dispose();
                        break;
                    }
                    i = await stream.ReadAsync(buffer, 0, buffer.Length);
                    // need to add logic here if more of the message needs to be read!
                    string message = System.Text.Encoding.Default.GetString(buffer);

                }
            });

            await _reciever;
        }

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _client.Close();
                _client.Dispose();
            }
        }
    }
}
