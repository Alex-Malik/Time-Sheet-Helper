using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.ExceptionServices;
using System.Windows.Threading;

namespace TimeSheet
{
    using Services;
    using Shared;
    using Views;
    using Views.Pages;
    using Interfaces;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Singleton Implementation

        public static App Instance { get; private set; }

        // Note! That only one instance of the System.Windows.Application 
        // class can be created per System.AppDomain.
        public App()
        {
            Instance = this;
        }

        #endregion

        #region Container Implementation
        public const string AppScope = "app";
        public const string PageScope = "page";

        private ILifetimeScope _app;
        private ILifetimeScope _page;

        public IContainer Container { get; private set; }

        public ILifetimeScope AppplicationScope
        {
            get
            {
                if (_app == null)
                    _app = Container.BeginLifetimeScope(AppScope);
                return _app;
            }
        }

        public ILifetimeScope BeginPageScope()
        {
            EndPageScope();
            _page = AppplicationScope.BeginLifetimeScope(PageScope);
            return _page;
        }

        public void EndPageScope()
        {
            if (_page != null)
            {
                _page.Dispose();
                _page = null;
            }
        }
        #endregion Container Implementation

        #region App Event Overrides
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeContainer();
            InitializeExceptionsHandling();
            InitializeMainWindow();
            
            NavigationManager.Instance.GoTo<Insert>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DeinitializeContainer();
            base.OnExit(e);
        }
        #endregion App Event Overrides

        #region Private Methods
        private void InitializeContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            builder.RegisterModule<SharedModule>();
            builder.RegisterModule<ServicesModule>();

            Container = builder.Build();
        }

        private void InitializeExceptionsHandling()
        {
            AppDomain.CurrentDomain.FirstChanceException += OnDomainException;
            DispatcherUnhandledException += OnApplicationException;
        }

        private void InitializeMainWindow()
        {
            MainWindow = Container.Resolve<MainWindow>();
            MainWindow.Show();
        }

        private void DeinitializeContainer()
        {
            if (_page != null)
                _page.Dispose();
            if (_app != null)
                _app.Dispose();

            Container.Dispose();
            Container = null;
        }

        private void OnDomainException(object sender, FirstChanceExceptionEventArgs e)
        {
            // Try to get messages control and show error on the screen, but
            // if control will not be found then we try to navigate to the 
            // error screen, but if navigation impossible then we just log e.

            IMessageControl messages = ControlsRouter.Instance.Get<IMessageControl>();

            // If messages control found then show error
            // message; otherwise navigate to error page.
            if (messages != null)
            {
                messages.ShowError(e.Exception.Message);
            }
            else if (NavigationManager.Instance.NavigationPossible())
            {
                NavigationManager.Instance.GoTo<Error>(e.Exception);
            }
            else
            {
                // TODO: Implement logger, log exception, exit application.
                App.Instance.Shutdown();
            }
        }

        private void OnApplicationException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Try to get messages control and show error on the screen, but
            // if control will not be found then we try to navigate to the 
            // error screen, but if navigation impossible then we just log e.

            IMessageControl messages = ControlsRouter.Instance.Get<IMessageControl>();

            // If messages control found then show error
            // message; otherwise navigate to error page.
            if (messages != null)
            {
                messages.ShowError(e.Exception.Message);
            }
            else if (NavigationManager.Instance.NavigationPossible())
            {
                NavigationManager.Instance.GoTo<Error>(e.Exception);
            }
            else
            {
                // TODO: Implement logger, log exception, exit application.
                App.Instance.Shutdown();
            }

            e.Handled = true;
        }
        #endregion Private Methods
    }
}
