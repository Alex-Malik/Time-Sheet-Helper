using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface ISettings
    {

    }

    public interface IInsertSettings : ISettings
    {
        // View specific values.
        Int32   StepInHours    { get; set; }
        Int32   StepInMinutes  { get; set; }
        Boolean AllowDataMerge { get; set; }

        // Data specific values.
        IEnumerable<String> Projects { get; set; }
    }

    public interface ISheetsSettings : ISettings
    {
        // TODO: Consider how to architect it in generic way.

        String SpreadSheetID { get; set; }
        String SheetName     { get; set; }
    }

    public interface IApplicationSettings : ISettings
    {
        String SettingsFilePath { get; set; }
    }

    public interface ISettingsService<TSettings> : IService where TSettings : ISettings
    {
        void Save(TSettings settings);
        Task SaveAsync(TSettings settings);
        TSettings Load();
        Task<TSettings> LoadAsync();
    }
}
