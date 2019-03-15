using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClientApp.ViewModels
{
    public class MainViewVM : BaseVM
    {
        public QLabConnectorVM QLabConnectorVM { get; private set; }
        public MainViewVM(QLabConnectorVM qLabConnectorVM)
        {
            QLabConnectorVM = qLabConnectorVM;
        }
        protected override void RegisterEvents()
        {
            QLabConnectorVM.PropertyChanged += QLabConnectorVM_PropertyChanged;
            base.RegisterEvents();
        }

        protected override void UnRegisterEvents()
        {
            QLabConnectorVM.PropertyChanged -= QLabConnectorVM_PropertyChanged;
            base.UnRegisterEvents();
        }

        private void QLabConnectorVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
    }
}
