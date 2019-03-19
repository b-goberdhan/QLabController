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
        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }
        private readonly TcpClient _client;
        private Task _reciever;
        private bool _isDisposed;

        protected override Func<string> BackgroundReciever
        {
            get
            {
                return () =>
                {
                    NetworkStream stream = _client.GetStream();
                    int i = 0;
                    byte[] buffer = new byte[1024];
                    string json = "";
                    if (!_isDisposed)
                    {
                        if (_client?.Connected != true)
                        {
                            stream.Close();
                            stream.Dispose();
                        }
                        else
                        {
                            i = stream.Read(buffer, 0, buffer.Length);
                            // need to add logic here if more of the message needs to be read!
                            json = System.Text.Encoding.Default.GetString(buffer);
                        }          
                    }
                    stream.Dispose();
                    return json;
                };
            }
        }
        public TCPNetworkDevice(TcpClient client, string ipAddress, int portNum, string name) : base(name)
        {
            _client = client;
            IPAddress = IPAddress.Parse(ipAddress);
            Port = portNum;
            DeviceEndpoint = _client.Client.RemoteEndPoint;
        }

        public override void Connect()
        {
            _client.Connect(IPAddress, Port);
            base.Connect();
        }

        public override void Dispose()
        {
            if(!_isDisposed)
            {
                _isDisposed = true;
                _reciever.Dispose();
                _client.Close();
                _client.Dispose();
                base.Dispose();
            }
        }
    }
}
