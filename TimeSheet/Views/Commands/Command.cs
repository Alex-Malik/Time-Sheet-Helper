using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeSheet.Views.Commands
{
    /// <summary>
    /// Simple command which can execute given method with param or without.
    /// </summary>
    public class Command : ICommand
    {
        public static ICommand NotImplementedCommand => new Command(() => { });

        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public Command(Action execute)
        {
            _execute = _ => execute();
            _canExecute = _ => true;
        }

        public Command(Action execute, Func<bool> canExecute)
        {
            _execute = _ => execute();
            _canExecute = _ => canExecute();
        }

        public Command(Action<object> execute)
        {
            _execute = execute;
            _canExecute = _ => true;
        }

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        //public Command(Func<object> execute)
        //{
        //    _execute = _ => execute();
        //    _canExecute = _ => true;
        //}

        //public Command(Func<object, object> execute)
        //{
        //    _execute = _ => execute(_);
        //    _canExecute = _ => true;
        //}

        #region ICommand support

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion

        public void Refresh()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
