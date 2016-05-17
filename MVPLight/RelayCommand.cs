using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVPLight
{
    class RelayCommand : ICommand
    {
        private Action action;
        private Func<bool> canExecute;
        public event EventHandler CanExecuteChanged;
        public RelayCommand(
            Action action, ref EventHandler propertyChanged, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
            propertyChanged += RaiseCanExecuteChanged;//subscribing event
        }
        public RelayCommand(Action action)
        {
            this.action = action;
            this.canExecute = null;
        }
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute.Invoke();
        }
        public void Execute(object parameter)
        {
            action.Invoke();
        }
        void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
