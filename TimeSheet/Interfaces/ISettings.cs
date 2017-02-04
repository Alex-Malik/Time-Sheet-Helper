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
        Int32   StepInHours    { get; }
        Int32   StepInMinutes  { get; }
        Boolean AllowDataMerge { get; }

        // Data specific values.
        IEnumerable<String> Projects { get; }
    }

    public interface ISettingsService<TSettings> where TSettings : ISettings
    {
        void Save(TSettings settings);
        Task SaveAsync(TSettings settings);
        TSettings Load();
        Task<TSettings> LoadAsync();
    }
}
