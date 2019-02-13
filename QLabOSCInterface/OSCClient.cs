using Newtonsoft.Json;
using Rug.Osc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace QLabOSCInterface
{
    public abstract class OSCClient : IDisposable
    {
        public IPAddress DestinationIpAddress { get; private set; }
        private OscSender _sender;
        private OscReceiver _reciever;

        private bool _isDisposed = false;

        private const int RETRY_1_MS = 1;
        private const int DEFAULT_500_MS_TIMEOUT = 500;
       

        public OSCClient(string ipAddress, int sendPort, int recvPort)
        {
            DestinationIpAddress = IPAddress.Parse(ipAddress);
            _sender = new OscSender(DestinationIpAddress, sendPort);
            _reciever = new OscReceiver(recvPort);
        }

       

        protected async Task<T> Send<T>(string path, int timeout = DEFAULT_500_MS_TIMEOUT, params object[] args)
        {
            //wait for the messages to complete being sent
            await Task.Run(() => _sender.WaitForAllMessagesToComplete());

            if (args == null || args.Length == 0)
            {
                _sender.Send(new OscMessage(path));
            }
            else
            {
                _sender.Send(new OscMessage(path, args));
            }

            OscPacket packet = await RecieveOSC(timeout);
            return FormatResponsePacket<T>(packet?.ToString(), path);
            
            
        }

        private T FormatResponsePacket<T>(string packetString, string originalPath)
        {
            if(packetString == null)
            {
                return default(T);
            }

            packetString = packetString.TrimStart(("/reply/" + originalPath + ", ").ToCharArray());
            packetString = packetString.Trim("\"".ToCharArray());
            try
            {
                return JsonConvert.DeserializeObject<T>(packetString);
            }
            catch
            {
                return default(T);
            }
            
        }

        public virtual void Connect()
        {
            if(!_isDisposed)
            {
                _sender.Connect();
                _reciever.Connect();          
            }
            
        }

        private async Task<OscPacket> RecieveOSC(int timeoutMS)
        {
            if(_reciever?.State != OscSocketState.Closed && !_isDisposed)
            {
                if(_reciever?.State == OscSocketState.Connected)
                {
                    //changed this, need to test!
                    return await Task.Run(() => RetryRecieveOSC(timeoutMS));
                }
            }
            return null;
        }
        private OscPacket RetryRecieveOSC(int timeout)
        {
            int startTime = (int)DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
            int currentTimeMS =  startTime;
            int timeElapsed = 0;
            OscPacket packet = null;
            while (timeElapsed < timeout && !_reciever.TryReceive(out packet))
            {
                currentTimeMS = (int)DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
                timeElapsed = currentTimeMS - startTime;
            }
            return packet;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _sender.Close();
                _sender.Dispose();
                _reciever.Close();
                _reciever.Dispose();
            }


        }
    }
}
