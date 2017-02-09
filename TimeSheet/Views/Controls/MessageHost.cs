using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeSheet.Interfaces;
using TimeSheet.Shared;

namespace TimeSheet.Views.Controls
{
    public class MessageHost : Control, IMessageControl
    {
        #region Static Members
        static MessageHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageHost),
                new FrameworkPropertyMetadata(typeof(MessageHost)));
        }

        public static readonly DependencyProperty ErrorMessageStyleProperty =
            DependencyProperty.Register(nameof(ErrorMessageStyle), typeof(Style), typeof(MessageHost), new PropertyMetadata(null));

        public static readonly DependencyProperty InfoMessageStyleProperty =
            DependencyProperty.Register(nameof(InfoMessageStyle), typeof(Style), typeof(MessageHost), new PropertyMetadata(null));

        public static readonly DependencyProperty WarningMessageStyleProperty =
            DependencyProperty.Register(nameof(WarningMessageStyle), typeof(Style), typeof(MessageHost), new PropertyMetadata(null));
        #endregion Static Members

        #region Init
        private Message _inf;
        private Message _err;
        private Message _wrn;

        public MessageHost()
        {
            ControlsRouter.Instance.Register<IMessageControl>(this);
        }
        #endregion

        #region Properties
        public Style ErrorMessageStyle
        {
            get { return (Style)GetValue(ErrorMessageStyleProperty); }
            set { SetValue(ErrorMessageStyleProperty, value); }
        }

        public Style InfoMessageStyle
        {
            get { return (Style)GetValue(InfoMessageStyleProperty); }
            set { SetValue(InfoMessageStyleProperty, value); }
        }

        public Style WarningMessageStyle
        {
            get { return (Style)GetValue(WarningMessageStyleProperty); }
            set { SetValue(WarningMessageStyleProperty, value); }
        }
        #endregion Properties

        #region Public Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _inf = Template.FindName("xINF", this) as Message;
            _err = Template.FindName("xERR", this) as Message;
            _wrn = Template.FindName("xWRN", this) as Message;
        }
        #endregion

        #region IMessagesControl Support
        public void ShowInfo(string message)
        {
            if (NotReady(_inf)) return;

            _inf.Content = message;
            _inf.Visibility = Visibility.Visible;

            // TODO: Start timer.
        }

        public void ShowError(string message)
        {
            if (NotReady(_err)) return;

            _err.Content = message;
            _err.Visibility = Visibility.Visible;

            // TODO: Start timer.
        }

        public void ShowWarning(string message)
        {
            if (NotReady(_wrn)) return;

            _wrn.Content = message;
            _wrn.Visibility = Visibility.Visible;

            // TODO: Start timer.
        }
        #endregion IMessagesControl Support

        private Boolean NotReady(Message msg)
        {
            // We can't show message if other is shown.
            if (_inf != null)
                _inf.Visibility = Visibility.Collapsed;
            if (_err != null)
                _err.Visibility = Visibility.Collapsed;
            if (_wrn != null)
                _wrn.Visibility = Visibility.Collapsed;

            // Check whether message control initialized.
            return msg == null;
        }
    }
}
