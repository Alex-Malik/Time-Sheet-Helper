using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TimeSheet.Services;

namespace TimeSheet.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private const string DefaultSpreadSheetId = "1U8bBQtr4kFQkOeLoLlOrryFflDPzOb30ECDr8mCIDHo";
        private const int DefaultSheedIndex = 0;

        public Dashboard()
        {
            InitializeComponent();

            // TODO: Redesign with resolving by Autofac.
            Sheets = new GoogleSheetsServiceWrapper();
            Sheets.Init();

            DataContext = new DashboardViewModel
            {
                SpreadSheetId = DefaultSpreadSheetId,
                SheetIndex = DefaultSheedIndex,
                Records = Sheets.Get(DefaultSpreadSheetId, DefaultSheedIndex)
            };
        }

        // TODO: Should be setted with Autofac.
        public GoogleSheetsServiceWrapper Sheets { get; }
    }

    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string SpreadSheetId { get; set; }
        public int SheetIndex { get; set; }
        public string Records { get; set; }
    }
}
