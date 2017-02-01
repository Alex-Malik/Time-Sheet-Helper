using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TimeSheet.Views.Pages
{
    using Commands;
    using Services;
    using Shared;

    /// <summary>
    /// Interaction logic for Insert.xaml
    /// </summary>
    public partial class Insert : Page
    {
        public Insert(GoogleService sheets)
        {
            InitializeComponent();
            DataContext = new InsertViewModel(sheets);

            // Setup initial bindings.
            xStartedAtIncrement.Command = (DataContext as InsertViewModel)?.IncrementStartedAtMinutesCommand;
            xStartedAtDecrement.Command = (DataContext as InsertViewModel)?.DecrementStartedAtMinutesCommand;
            xEndedAtIncrement.Command   = (DataContext as InsertViewModel)?.IncrementEndedAtMinutesCommand;
            xEndedAtDecrement.Command   = (DataContext as InsertViewModel)?.DecrementEndedAtMinutesCommand;
        }

        private void OnKeyboardFocusedChangedForStartedAtHours(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false) return;
            xStartedAtIncrement.Command = (DataContext as InsertViewModel)?.IncrementStartedAtHoursCommand;
            xStartedAtDecrement.Command = (DataContext as InsertViewModel)?.DecrementStartedAtHoursCommand;
        }

        private void OnKeyboardFocusedChangedForStartedAtMinutes(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false) return;
            xStartedAtIncrement.Command = (DataContext as InsertViewModel)?.IncrementStartedAtMinutesCommand;
            xStartedAtDecrement.Command = (DataContext as InsertViewModel)?.DecrementStartedAtMinutesCommand;
        }

        private void OnKeyboardFocusedChangedForEndedAtHours(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false) return;
            xEndedAtIncrement.Command = (DataContext as InsertViewModel)?.IncrementEndedAtHoursCommand;
            xEndedAtDecrement.Command = (DataContext as InsertViewModel)?.DecrementEndedAtHoursCommand;
        }

        private void OnKeyboardFocusedChangedForEndedAtMinutes(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false) return;
            xEndedAtIncrement.Command = (DataContext as InsertViewModel)?.IncrementEndedAtMinutesCommand;
            xEndedAtDecrement.Command = (DataContext as InsertViewModel)?.DecrementEndedAtMinutesCommand;
        }
    }

    class InsertViewModel : INotifyPropertyChanged
    {
        private const string DefaultSpreadSheetId = "1U8bBQtr4kFQkOeLoLlOrryFflDPzOb30ECDr8mCIDHo";
        private const string DefaultSheedName     = "Alex Malik";
        private readonly Timer _timer;

        public InsertViewModel(GoogleService sheets)
        {
            Sheets = sheets;

            DateTime     now = DateTime.Now;
            Project          = String.Empty;
            Message          = String.Empty;
            StartedAtHours   = now.Hour >= 4  && now.Hour < 14 ? now.Hour   : 10;
            StartedAtMinutes = now.Hour >= 4  && now.Hour < 14 ? now.Minute : 0;
            EndedAtHours     = now.Hour >= 14 || now.Hour < 4  ? now.Hour   : 18;
            EndedAtMinutes   = now.Hour >= 14 || now.Hour < 4  ? now.Minute : 0;
            CreatedAt        = DateTime.Now;
            CurrentTime      = DateTime.Now;
            CurrentDate      = DateTime.Now;

            _timer = new Timer(OnTimerCallback, this, 0, 1000);
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // IoC Properties
        public GoogleService     Sheets     { get; }
        public NavigationManager Navigator  { get; }

        // Commands
        public ICommand SaveCommand          => CommandFactory.CreateFor(Save);
        public ICommand GoToTimetableCommand => CommandFactory.CreateFor(GoToTimetable);
        public ICommand GoToSettingsCommand  => CommandFactory.CreateFor(GoToSettings);
        public ICommand IncrementStartedAtHoursCommand   => CommandFactory.CreateFor(IncrementStartedAtHours);
        public ICommand IncrementStartedAtMinutesCommand => CommandFactory.CreateFor(IncrementStartedAtMinutes);
        public ICommand DecrementStartedAtHoursCommand   => CommandFactory.CreateFor(DecrementStartedAtHours);
        public ICommand DecrementStartedAtMinutesCommand => CommandFactory.CreateFor(DecrementStartedAtMinutes);
        public ICommand IncrementEndedAtHoursCommand     => CommandFactory.CreateFor(IncrementEndedAtHours);
        public ICommand IncrementEndedAtMinutesCommand   => CommandFactory.CreateFor(IncrementEndedAtMinutes);
        public ICommand DecrementEndedAtHoursCommand     => CommandFactory.CreateFor(DecrementEndedAtHours);
        public ICommand DecrementEndedAtMinutesCommand   => CommandFactory.CreateFor(DecrementEndedAtMinutes);

        // Bindable Properties
        public String   Project          { get; set; }
        public String   Message          { get; set; }
        public Int32    StartedAtHours   { get; private set; }
        public Int32    StartedAtMinutes { get; private set; }
        public Int32    EndedAtHours     { get; private set; }
        public Int32    EndedAtMinutes   { get; private set; }
        public DateTime CreatedAt        { get; set; }
        public DateTime CurrentTime      { get; private set; }
        public DateTime CurrentDate      { get; private set; }

        public String   FormattedStartedAtHours   => $"{StartedAtHours:00}";
        public String   FormattedStartedAtMinutes => $"{StartedAtMinutes:00}";
        public String   FormattedEndedAtHours     => $"{EndedAtHours:00}";
        public String   FormattedEndedAtMinutes   => $"{EndedAtMinutes:00}";
        public String   FormattedCurrentTime      => $"{CurrentTime.Hour:00}:{CurrentTime.Minute:00}:{CurrentTime.Second:00}";
        public String   FormattedCurrentDate      => CurrentDate.ToString("dddd, MMMM MM, yyyy");

        private void Save()
        {
            Double hours = (EndedAtHours + (EndedAtMinutes / 60.0)) - (StartedAtHours + (StartedAtMinutes / 60.0));
            DateTime startedAt = DateTime.Now.Date.AddHours(StartedAtHours).AddMinutes(StartedAtMinutes);
            DateTime endedAt   = DateTime.Now.Date.AddHours(EndedAtHours).AddMinutes(EndedAtMinutes);

            Sheets.Insert(DefaultSpreadSheetId, DefaultSheedName, CreatedAt, Message, Project, hours, startedAt, endedAt);
        }
        
        private void GoToTimetable()
        {
            // TODO: Rename Dashboard to TimeTable.
            Navigator.GoTo<Dashboard>();
        }

        private void GoToSettings()
        {
            Navigator.GoTo<Settings>();
        }

        private void OnTimerCallback(object state)
        {
            // Update current time.
            CurrentTime = DateTime.Now;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTime)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedCurrentTime)));

            // Update current date.
            CurrentDate = DateTime.Now;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDate)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedCurrentDate)));
        }

        private void IncrementStartedAtHours()
        {
            StartedAtHours++;
            if (StartedAtHours >= 24) StartedAtHours = 0;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(StartedAtHours)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedStartedAtHours)));
        }

        private void IncrementStartedAtMinutes()
        {
            StartedAtMinutes += 5;
            if (StartedAtMinutes >= 60) {
                StartedAtMinutes = 0;
                IncrementStartedAtHours();
            }
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(StartedAtMinutes)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedStartedAtMinutes)));
        }

        private void DecrementStartedAtHours()
        {
            StartedAtHours--;
            if (StartedAtHours < 0) StartedAtHours = 23;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(StartedAtHours)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedStartedAtHours)));
        }

        private void DecrementStartedAtMinutes()
        {
            StartedAtMinutes -= 5;
            if (StartedAtMinutes < 0) {
                StartedAtMinutes = 55;
                DecrementStartedAtHours();
            }
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(StartedAtMinutes)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedStartedAtMinutes)));
        }

        private void IncrementEndedAtHours()
        {
            EndedAtHours++;
            if (EndedAtHours >= 24) EndedAtHours = 0;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(EndedAtHours)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedEndedAtHours)));
        }

        private void IncrementEndedAtMinutes()
        {
            EndedAtMinutes += 5;
            if (EndedAtMinutes >= 60) {
                EndedAtMinutes = 0;
                IncrementEndedAtHours();
            }
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(EndedAtMinutes)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedEndedAtMinutes)));
        }

        private void DecrementEndedAtHours()
        {
            EndedAtHours--;
            if (EndedAtHours < 0) EndedAtHours = 23;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(EndedAtHours)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedEndedAtHours)));
        }

        private void DecrementEndedAtMinutes()
        {
            EndedAtMinutes -= 5;
            if (EndedAtMinutes < 0) {
                EndedAtMinutes = 55;
                DecrementEndedAtHours();
            }
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(EndedAtMinutes)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedEndedAtMinutes)));
        }
    }
}
