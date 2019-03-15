using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClientApp.ViewModels;

namespace WPFClientApp.Ninject
{
    public class NinjectLocator
    {
        public MainViewVM MainViewVM
        {
            get { return IocKernel.Get<MainViewVM>(); }
        }
    }
}
