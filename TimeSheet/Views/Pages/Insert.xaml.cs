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

namespace TimeSheet.Views.Pages
{
    using Commands;
    using Shared;
    using Services;

    /// <summary>
    /// Interaction logic for Insert.xaml
    /// </summary>
    public partial class Insert : Page
    {
        public Insert(GoogleService sheets)
        {
            InitializeComponent();
            DataContext = new InsertViewModel(sheets);
        }
    }

    class InsertViewModel : INotifyPropertyChanged
    {
        private const string DefaultSpreadSheetId = "1U8bBQtr4kFQkOeLoLlOrryFflDPzOb30ECDr8mCIDHo";
        private const string DefaultSheedName = "Alex Malik";

        public InsertViewModel(GoogleService sheets)
        {
            Sheets = sheets;

            Project   = String.Empty;
            Message   = String.Empty;
            CreatedAt = DateTime.Now;
            StartedAtHours   = 10;
            StartedAtMinutes = 0;
            EndedAtHours     = 18;
            EndedAtMinutes   = 0;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // IoC Properties
        public GoogleService Sheets { get; }

        // Commands
        public ICommand SaveCommand   => CommandFactory.CreateFor(Save);
        public ICommand GoBackCommand => CommandFactory.CreateFor(GoBack);

        // Bindable Properties
        public String   Project          { get; set; }
        public String   Message          { get; set; }
        public DateTime CreatedAt        { get; set; }
        public Int32    StartedAtHours   { get; set; }
        public Int32    StartedAtMinutes { get; set; }
        public Int32    EndedAtHours     { get; set; }
        public Int32    EndedAtMinutes   { get; set; }
        public String   FormattedCreatedAt => CreatedAt.ToString("yyyy-MM-dd");

        private void Save()
        {
            Sheets.Insert(DefaultSpreadSheetId, DefaultSheedName, CreatedAt, Message, Project, 0, DateTime.Now, DateTime.Now);
        }
        
        private void GoBack()
        {
            NavigationManager.Instance.GoBack();
        }
    }
}
