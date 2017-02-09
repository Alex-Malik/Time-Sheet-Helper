using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface IControl
    {

    }

    public interface IMessageControl : IControl
    {
        void ShowInfo(string message);
        void ShowError(string message);
        void ShowWarning(string message);
    }

    public interface IControlsRouter
    {
        void Register<T>(T control)   where T : class, IControl;
        void Unregister<T>(T control) where T : class, IControl;

        T Get<T>() where T: class, IControl;
    }
}
