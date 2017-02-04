using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services
{
    // TODO: Move all interfaces to corresponding folders.

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
        void            Save(TSettings settings);
        Task            SaveAsync(TSettings settings);
        TSettings       Load();
        Task<TSettings> LoadAsync();
    }

    // TODO: Implementation of ISettingsService should implement explicetly
    // different instances of the ISettingsService<T>.

    public class SettingsService
        : ISettingsService<IInsertSettings>
    {
        #region IInsertSettings Support
        IInsertSettings ISettingsService<IInsertSettings>.Load()
        {
            throw new NotImplementedException();
        }

        Task<IInsertSettings> ISettingsService<IInsertSettings>.LoadAsync()
        {
            throw new NotImplementedException();
        }

        void ISettingsService<IInsertSettings>.Save(IInsertSettings settings)
        {
            throw new NotImplementedException();
        }

        Task ISettingsService<IInsertSettings>.SaveAsync(IInsertSettings settings)
        {
            throw new NotImplementedException();
        }
        #endregion IInsertSettings Support
    }
}
