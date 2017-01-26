﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimeSheet.Views.Pages
{
    using Commands;
    using Services;
    using Services.Models;
    using Shared;
    using System;

    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard(GoogleSheetsServiceWrapper sheets)
        {
            InitializeComponent();
            DataContext = new DashboardViewModel(sheets);
        }
    }

    class DashboardViewModel : INotifyPropertyChanged
    {
        private const string DefaultSpreadSheetId = "1U8bBQtr4kFQkOeLoLlOrryFflDPzOb30ECDr8mCIDHo";
        private const string DefaultSheedName     = "Alex Malik";

        public DashboardViewModel(GoogleSheetsServiceWrapper sheets)
        {
            Sheets = sheets;
            Sheets.Init();

            SpreadSheetId = DefaultSpreadSheetId;
            SheetName     = DefaultSheedName;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // IoC Properties
        public GoogleSheetsServiceWrapper Sheets { get; }

        // Commands
        public ICommand RefreshCommand => CommandFactory.CreateFor(Refresh);
        public ICommand SettingsCommand => CommandFactory.CreateFor(Settings);
        public ICommand UpdateCommand { get; }
        public ICommand InsertCommand => CommandFactory.CreateFor(Insert);

        // Bindable Properties
        public string SpreadSheetId { get; set; }
        public string SheetName { get; set; }
        public IEnumerable<RecordModel> Records { get; private set; }

        private void Refresh()
        {
            Records = Sheets.Get(SpreadSheetId, SheetName);
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