using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Services.Interfaces;

namespace TimeSheet.Services
{
    // TODO: Move all interfaces to corresponding folders.
    
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

    public class InsertService : IInsertService
    {
        private readonly ISheetsService _sheets;
        private readonly ISettingsService<IInsertSettings> _settings;

        public InsertService(ISettingsService<IInsertSettings> settings, ISheetsService sheets)
        {
            _sheets = sheets;
            _settings = settings;
        }

        public void Save(IData data)
        {
            // TODO: View should implement ViewDataAdapter : IData which will be put here.
            // TODO: Here IData should be transformed to the IRow and sent to the ISheetsService.

            throw new NotImplementedException();
        }

        public Task SaveAsync(IData data)
        {
            // TODO: View should implement ViewDataAdapter : IData which will be put here.
            // TODO: IData should be transformed to the IRow and sent to the ISheetsService asynchronously.
            throw new NotImplementedException();
        }

        public IInsertSettings LoadSettings() => _settings.Load();

        public Task<IInsertSettings> LoadSettingsAsync() => _settings.LoadAsync();

        public void SaveSettings(IInsertSettings settings) => _settings.Save(settings);

        public Task SaveSettingsAsync(IInsertSettings settings) => _settings.SaveAsync(settings);
    }
}
