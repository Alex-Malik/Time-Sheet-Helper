using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services
{
    using Interfaces;

    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //builder
            //    .RegisterType<GoogleService>()
            //    .As<GoogleService>()  // TODO: Change on ISheetsService
            //    .InstancePerMatchingLifetimeScope(App.PageScope);
            //builder
            //    .RegisterType<SettingsService>()
            //    .AsSelf()
            //    .InstancePerMatchingLifetimeScope(App.PageScope);

            builder.RegisterType<SettingsService>().AsImplementedInterfaces()
                .InstancePerMatchingLifetimeScope(App.AppScope);

            builder.RegisterType<InsertService>().As<IInsertService>()
                .InstancePerMatchingLifetimeScope(App.PageScope);
        }
    }
}
