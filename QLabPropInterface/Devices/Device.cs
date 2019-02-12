using PropInterface.Delegates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QLabPropInterface.Devices
{
    public abstract class Device<TData>
    {

        public abstract event MessageRecievedHandler<TData> Recieved;
        protected abstract Task SendAsync(object message);
        protected abstract Task RecvBackgroundAsync();
        public abstract void Open();
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
