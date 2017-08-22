using System;
using System.Windows.Input;

namespace Common
{
    public class RelayCommand<TParameter> : ICommand where TParameter : class
    {
        private Action<TParameter> execute;
        private Func<TParameter, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<TParameter> execute, Func<TParameter, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter as TParameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter as TParameter);
        }
    }
}
