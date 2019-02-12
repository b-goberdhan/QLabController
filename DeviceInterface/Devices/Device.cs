using DeviceInterface.Delegates;
using System.Threading.Tasks;

namespace DeviceInterface.Devices
{
    public abstract class Device<TData>
    {

        public abstract event MessageRecievedHandler<TData> Recieved;
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

        
    }
}
