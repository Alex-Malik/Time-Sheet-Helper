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
        public static IContainer Container;

        public const string ApplicationScope = "app";
        public const string UserScope = "user";
        public const string LocalScope = "page";

        private ILifetimeScope _app;
        private ILifetimeScope _user;
        private ILifetimeScope _page;

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

            base.OnExit(e);
        }

        public ILifetimeScope AppplicationScope
        {
            get
            {
                if (_app == null)
                    _app = Container.BeginLifetimeScope(ApplicationScope);
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
            _page = _user.BeginLifetimeScope(LocalScope);
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
