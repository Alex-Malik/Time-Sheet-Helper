using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeSheet.Views.Commands
{
    public class CommandFactory
    {
        public static ICommand CreateFor(Action action)
        {
            return new Command(action);
        }

        public static ICommand CreateFor(Action action, Func<bool> canExecute)
        {
            return new Command(action, canExecute);
        }

        public static ICommand CreateFor(Action<object> action)
        {
            return new Command(action);
        }

        public static ICommand CreateFor(Action<object> action, Func<object, bool> canExecute)
        {
            return new Command(action, canExecute);
        }
    }
}
