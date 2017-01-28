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
        public Insert(GoogleSheetsServiceWrapper sheets)
        {
            InitializeComponent();
            DataContext = new InsertViewModel(sheets);
        }
    }

    class InsertViewModel : INotifyPropertyChanged
    {
        public InsertViewModel(GoogleSheetsServiceWrapper sheets)
        {
            Sheets = sheets;
            Sheets.Init();

            Project = String.Empty;
            Message = String.Empty;
            CreatedAt = DateTime.Now;
            StartedAtHours = 10;
            StartedAtMinutes = 0;
            EndedAtHours = 18;
            EndedAtMinutes = 0;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // IoC Properties
        public GoogleSheetsServiceWrapper Sheets { get; }

        // Commands
        public ICommand SaveCommand => CommandFactory.CreateFor(Save);
        public ICommand GoBackCommand => CommandFactory.CreateFor(GoBack);

        // Bindable Properties
        public String   Project { get; set; }
        public String   Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public String   FormattedCreatedAt => CreatedAt.ToString("yyyy-MM-dd");
        public Int32    StartedAtHours { get; set; }
        public Int32    StartedAtMinutes { get; set; }
        public Int32    EndedAtHours { get; set; }
        public Int32    EndedAtMinutes { get; set; }

        private void Save()
        {
            throw new NotImplementedException();
        }
        
        private void GoBack()
        {
            NavigationManager.Instance.GoBack();
        }
    }
}
