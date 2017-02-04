using System;

namespace TimeSheet.Interfaces
{
    public interface IApplicationSettings : ISettings
    {
        String SettingsFilePath { get; set; }
    }
}
