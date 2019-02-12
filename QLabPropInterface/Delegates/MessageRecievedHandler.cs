using PropInterface.PropObjects;
using QLabPropInterface.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropInterface.Delegates
{
    public delegate void MessageRecievedHandler<TData>(Device<TData> device, TData response);
    
}
