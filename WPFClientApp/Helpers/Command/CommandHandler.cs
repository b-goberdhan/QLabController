using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFClientApp.Helpers.Command
{
    public class CommandHandler : ICommand
    {
        private Action<object> _action;
        public CommandHandler(Action action)
        {
            _action = (obj) => action();
        }
        public CommandHandler(Action<object> action)
        {
            _action = action;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
