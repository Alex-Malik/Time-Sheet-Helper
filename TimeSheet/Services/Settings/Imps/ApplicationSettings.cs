using System;

namespace TimeSheet.Services.Settings.Imps
{
    using Interfaces;

    internal class AppSettings : IApplicationSettings
    {
        public AppSettings(String settingsFilePath)
        {
            SettingsFilePath = settingsFilePath;
        }

        public String SettingsFilePath { get; set; }
    }
}
