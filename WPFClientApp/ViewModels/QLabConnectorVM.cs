using DeviceInterface.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFClientApp.Helpers.Command;
using WPFClientApp.Services;

namespace WPFClientApp.ViewModels
{
    public class QLabConnectorVM : BaseVM
    {
        private readonly QLabService _qLabService;
        private readonly DeviceService _deviceService;
        public QLabConnectorVM(QLabService qLabService, DeviceService deviceService)
        {
            _qLabService = qLabService;
            _deviceService = deviceService;
        }
        protected override void RegisterEvents()
        {
            _deviceService.MessageRecieved += DeviceService_MessageRecieved;
            base.RegisterEvents();
        }
        protected override void UnRegisterEvents()
        {
            _deviceService.MessageRecieved -= DeviceService_MessageRecieved;
            base.UnRegisterEvents();
        }


        private ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new CommandHandler(() =>
                {
                    _deviceService.ConnectToCOM(3);
                   
                }));
            }
        }

        private void DeviceService_MessageRecieved(Device<DeviceSensorModel> device, DeviceSensorModel response)
        {
            
        }
    }

}
