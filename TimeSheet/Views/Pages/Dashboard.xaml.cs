using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimeSheet.Views.Pages
{
    using Commands;
    using Services;
    using Services.Models;
    using Shared;

    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        // TODO: Remove after Autofac implementation.
        public Dashboard()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel(new GoogleSheetsServiceWrapper());
        }

        public Dashboard(GoogleSheetsServiceWrapper sheets)
        {
            InitializeComponent();
            DataContext = new DashboardViewModel(sheets);
        }
    }

    public class DashboardViewModel : INotifyPropertyChanged
    {
        private const string DefaultSpreadSheetId = "1U8bBQtr4kFQkOeLoLlOrryFflDPzOb30ECDr8mCIDHo";
        private const int DefaultSheedIndex = 0;

        public DashboardViewModel(GoogleSheetsServiceWrapper sheets)
        {
            Sheets = sheets;
            Sheets.Init();

            SpreadSheetId = DefaultSpreadSheetId;
            SheetIndex = DefaultSheedIndex;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // IoC Properties
        public GoogleSheetsServiceWrapper Sheets { get; }

        // Commands
        public ICommand RefreshCommand => CommandsFactory.CreateFor(Refresh);
        public ICommand SettingsCommand => CommandsFactory.CreateFor(Settings);
        public ICommand UpdateCommand { get; }
        public ICommand InsertCommand { get; }

        // Bindable Properties
        public string SpreadSheetId { get; set; }

        public int SheetIndex { get; set; }
        public IEnumerable<RecordModel> Records { get; private set; }

        private void Refresh()
        {
            Records = Sheets.Get(SpreadSheetId, SheetIndex);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));
        }

        private void Settings()
        {
            NavigationManager.Instance.GoTo<Settings>();
        }
    }
}