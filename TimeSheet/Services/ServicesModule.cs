using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterType<GoogleSheetsServiceWrapper>()
                .As<GoogleSheetsServiceWrapper>()
                .InstancePerMatchingLifetimeScope(App.PageScope);
        }
    }
}
