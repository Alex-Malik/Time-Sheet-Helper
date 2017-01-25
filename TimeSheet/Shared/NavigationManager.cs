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

        public void GoTo<T>() where T : Page
        {
            InternalGoTo<T>();
        }

        public void GoTo<T>(params object[] args) where T : Page
        {
            InternalGoTo<T>(args);
        }

        private void InternalGoTo<T>(params object[] args)
        {
            throw new NotImplementedException();

            //_control.GoTo(page, args);
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
