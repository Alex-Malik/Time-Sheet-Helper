using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using TimeSheet.Shared;

namespace TimeSheet.Controls
{
    public class FrameAdapter : Frame, INavigationControl
    {
        #region INavigationControl Support

        bool INavigationControl.CanGoBack => CanGoBack;

        void INavigationControl.GoBack()
        {
            if (CanGoBack) GoBack();
        }

        void INavigationControl.GoForward()
        {
            throw new NotImplementedException();
        }

        void INavigationControl.GoTo(Page page, params object[] args)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            Navigate(page, args);
        }

        void INavigationControl.ClearHistory()
        {
            if (!CanGoBack && !CanGoForward) return;

            JournalEntry entry = RemoveBackEntry();
            while (entry != null)
                entry = RemoveBackEntry();

            Navigate(new PageFunction<string>() { RemoveFromJournal = true });
        }

        #endregion
    }
}
