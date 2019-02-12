using DeviceInterface.Devices;

namespace DeviceInterface.Delegates
{
    public delegate void MessageRecievedHandler<TData>(Device<TData> device, TData response);
    
}
