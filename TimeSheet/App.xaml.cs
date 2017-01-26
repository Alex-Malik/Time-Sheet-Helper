using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace TimeSheet
{
    using TimeSheet.Services;
    using TimeSheet.Shared;

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

        public const string AppScope = "app";
        public const string UserScope = "user";
        public const string PageScope = "page";

        private ILifetimeScope _app;
        private ILifetimeScope _user;
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

        public ILifetimeScope BeginUserScope()
        {
            EndUserScope();
            _user = AppplicationScope.BeginLifetimeScope(UserScope);
            return _user;
        }

        public void EndUserScope()
        {
            if (_user != null)
            {
                _user.Dispose();
                _user = null;
            }
        }

        public ILifetimeScope BeginPageScope()
        {
            EndPageScope();
            if (_user == null)
                _user = BeginUserScope();
            _page = _user.BeginLifetimeScope(PageScope);
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
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container = CreateContainer();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_page != null)
                _page.Dispose();
            if (_user != null)
                _user.Dispose();
            if (_app != null)
                _app.Dispose();

            Container = null;

            base.OnExit(e);
        }

        private IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<SharedModule>();
            builder.RegisterModule<ServicesModule>();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            return builder.Build();
        }
    }
}
