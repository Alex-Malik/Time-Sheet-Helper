using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Settings.Imps
{
    using Interfaces;

    internal class ApplicationSettings : IApplicationSettings
    {
        public String SettingsFilePath { get; set; }
    }
}
