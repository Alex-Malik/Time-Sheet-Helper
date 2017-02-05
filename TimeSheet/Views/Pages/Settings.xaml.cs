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
    using Interfaces;
    using Shared;

    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings(SettingsViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }

    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly NavigationManager _navigator;
        private readonly ISettingsService  _service;

        public SettingsViewModel(NavigationManager navigator, ISettingsService service)
        {
            _navigator = navigator;
            _service   = service;

            SheetSettings = _service.Load<ISheetSettings>();
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Commands
        public ICommand SaveCommand   => CommandFactory.CreateFor(Save);
        public ICommand CancelCommand => CommandFactory.CreateFor(Cancel);

        // Bindable Properties
        public ISheetSettings SheetSettings { get; }

        private void Save()
        {
            _service.Save(SheetSettings);
        }

        private void Cancel()
        {
            _navigator.GoBack();
        }
    }
}
