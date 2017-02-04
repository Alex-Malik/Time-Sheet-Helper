using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Interfaces;

namespace TimeSheet.Services
{
    using Interfaces;

    public class InsertService : IInsertService
    {
        // TODO: Move this constants to the settings.
        private const string DefaultSpreadSheetId = "1U8bBQtr4kFQkOeLoLlOrryFflDPzOb30ECDr8mCIDHo";
        private const string DefaultSheetName     = "Alex Malik";
        private readonly ISheetsService _sheets;
        private readonly ISettingsService<IInsertSettings> _settings;

        public InsertService(ISettingsService<IInsertSettings> settings, ISheetsService sheets)
        {
            _sheets = sheets;
            _settings = settings;
        }

        public void Save(IData data)
        {
            // TODO: Here IData should be transformed to the IRow and sent to the ISheetsService.
            throw new NotImplementedException();
        }

        public Task SaveAsync(IData data)
        {
            // TODO: IData should be transformed to the IRow and sent to the ISheetsService asynchronously.
            throw new NotImplementedException();
        }

        public IInsertSettings LoadSettings() => _settings.Load();

        public Task<IInsertSettings> LoadSettingsAsync() => _settings.LoadAsync();

        public void SaveSettings(IInsertSettings settings) => _settings.Save(settings);

        public Task SaveSettingsAsync(IInsertSettings settings) => _settings.SaveAsync(settings);
    }
}
