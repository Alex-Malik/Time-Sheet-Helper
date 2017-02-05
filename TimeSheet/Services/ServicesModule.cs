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

            builder.RegisterType<SettingsService>().As<ISettingsService>()
                .InstancePerMatchingLifetimeScope(App.PageScope);
            builder.RegisterType<InsertService>().As<IInsertService>()
                .InstancePerMatchingLifetimeScope(App.PageScope);
            builder.RegisterType<GoogleService>().As<ISheetService>()
                .InstancePerMatchingLifetimeScope(App.PageScope);
        }
    }
}
