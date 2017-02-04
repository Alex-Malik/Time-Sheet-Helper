using Autofac;

namespace TimeSheet.Services
{
    using Interfaces;

    using Google;
    using Insert;
    using Settings;

    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SettingsService>().AsImplementedInterfaces()
                .InstancePerMatchingLifetimeScope(App.AppScope);

            builder.RegisterType<InsertService>().As<IInsertService>()
                .InstancePerMatchingLifetimeScope(App.PageScope);
            builder.RegisterType<GoogleService>().As<ISheetsService>()
                .InstancePerMatchingLifetimeScope(App.PageScope);
        }
    }
}
