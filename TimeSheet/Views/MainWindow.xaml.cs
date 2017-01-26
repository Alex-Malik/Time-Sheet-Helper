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
using TimeSheet.Services;
using TimeSheet.Shared;
using TimeSheet.Views.Pages;

namespace TimeSheet.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;

        }

        // TODO: Consider init MainWindow and do initial navigation
        // from App.OnStartup() method.
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.GoTo<Dashboard>();
        }
    }

    public class MainWindowViewModel
    {
    }
}
