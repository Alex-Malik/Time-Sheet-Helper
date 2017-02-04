using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface IService
    {

    }

    public interface IService<TSettings> where TSettings : ISettings
    {
        TSettings LoadSettings();
        Task<TSettings> LoadSettingsAsync();
        void SaveSettings(TSettings settings);
        Task SaveSettingsAsync(TSettings settings);
    }


    public interface IInsertService : IService<IInsertSettings>
    {
        void Save(IData data);
        Task SaveAsync(IData data);
    }
}
