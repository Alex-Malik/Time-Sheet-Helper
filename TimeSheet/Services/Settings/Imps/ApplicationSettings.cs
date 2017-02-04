using System;

namespace TimeSheet.Services.Settings.Imps
{
    using Interfaces;

    internal class ApplicationSettings : IApplicationSettings
    {
        public String SettingsFilePath { get; set; }
    }
}
