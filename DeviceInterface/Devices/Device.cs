using DeviceInterface.Delegates;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DeviceInterface.Devices
{
    public abstract class Device<TData> : IDisposable
    {

        public event MessageRecievedHandler<TData> Recieved;
        protected abstract Task SendAsync(object message);
        protected abstract Task RecvBackgroundAsync();
        public abstract void Connect();
        public string Name { get; private set; }
        protected Device(string name)
        {
            Name = name;
        }

        protected void Send(object message)
        {
            var task = SendAsync(message);
            Task.WaitAll(task);
        }
        protected void NotifyRecieved(string jsonString)
        {
            var response = JsonConvert.DeserializeObject<TData>(jsonString);
            Recieved?.Invoke(this, response);
        }

        public abstract void Dispose();
     
    }
}
