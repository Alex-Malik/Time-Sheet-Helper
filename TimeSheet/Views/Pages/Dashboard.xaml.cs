using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimeSheet.Views.Pages
{
    using Commands;
    using Services;
    using Services.Interfaces;
    using Shared;

    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard(GoogleService sheets)
        {
            InitializeComponent();
            DataContext = new DashboardViewModel(sheets);
        }
    }

    class DashboardViewModel : INotifyPropertyChanged
    {
        private const string DefaultSpreadSheetId = "1U8bBQtr4kFQkOeLoLlOrryFflDPzOb30ECDr8mCIDHo";
        private const string DefaultSheetName     = "Alex Malik";

        public DashboardViewModel(GoogleService sheets)
        {
            Sheets = sheets;

            SpreadSheetId = DefaultSpreadSheetId;
            SheetName     = DefaultSheetName;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // IoC Properties
        public GoogleService Sheets { get; }

        // Commands
        public ICommand RefreshCommand  => CommandFactory.CreateFor(Refresh);
        public ICommand SettingsCommand => CommandFactory.CreateFor(Settings);
        public ICommand UpdateCommand   { get; }
        public ICommand InsertCommand   => CommandFactory.CreateFor(Insert);

        // Bindable Properties
        public string SpreadSheetId { get; set; }
        public string SheetName     { get; set; }
        public IEnumerable<IData> Records { get; private set; }

        private void Refresh()
        {
            Records = Sheets.GetData(SpreadSheetId, SheetName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Records)));
        }

        private void Settings()
        {
            NavigationManager.Instance.GoTo<Settings>();
        }

        private void Insert()
        {
            NavigationManager.Instance.GoTo<Insert>();
        }
    }
}