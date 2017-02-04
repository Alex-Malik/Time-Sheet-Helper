using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Settings
{
    //using Imps;
    using Interfaces;

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
