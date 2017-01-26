using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TimeSheet.Shared
{
    public class NavigationManager
    {
        #region Singleton Implementation

        public static readonly NavigationManager Instance = new NavigationManager();

        private NavigationManager() { }

        #endregion

        private const string MessageControlNotRegistered = "Navigation control should be registered before navigation operation.";

        private INavigationControl _control;

        public void GoTo<T>() where T : Page
        {
            InternalGoTo<T>();
        }

        public void GoTo<T>(params object[] args) where T : Page
        {
            InternalGoTo<T>(args);
        }

        public void GoBack()
        {
            if (_control == null)
                throw new NullReferenceException(MessageControlNotRegistered);

            if (_control.CanGoBack)
                _control.GoBack();
        }

        public void Register(INavigationControl control)
        {
            _control = control;
        }

        public void Unregister(INavigationControl control)
        {
            if (_control == control)
                _control = null;
        }

        private void InternalGoTo<T>(params object[] args)
        {
            if (_control == null)
                throw new NullReferenceException(MessageControlNotRegistered);

            var pageScope = App.Instance.BeginPageScope();
            var page = pageScope.Resolve(typeof(T)) as Page;
            //if (page is IAcceptParameters)
            //    ((IAcceptParameters)page).SetParams(args);

            _control.GoTo(page, args);
        }
    }

    public interface INavigationControl
    {
        void GoTo(Page page, params object[] args);
        void GoBack();
        void GoForward();

        bool CanGoBack { get; }

        void ClearHistory();
    }
}
