using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClientApp.Services;
using WPFClientApp.ViewModels;

namespace WPFClientApp.Ninject
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<MainViewVM>().ToSelf().InTransientScope();
            Bind<QLabConnectorVM>().ToSelf().InTransientScope();
            Bind<DeviceService>().ToSelf().InSingletonScope();
            Bind<QLabService>().ToSelf().InSingletonScope();
        }
    }
}
