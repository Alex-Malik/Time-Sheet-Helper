using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Insert
{
    using Imps;
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
            _sheets   = sheets;
            _settings = settings;
        }

        public void Save(IData data)
        {
            ISheet sheet = _sheets.GetSheet(DefaultSpreadSheetId, DefaultSheetName);
            IRow   row   = new InsertDataRow
                          (new[] {
                              new InsertDataCell(data.CreatedAt, CellDataType.Date),
                              new InsertDataCell(data.Content),
                              new InsertDataCell(data.Hours),
                              new InsertDataCell(data.Project),
                              new InsertDataCell(data.StartedAt, CellDataType.Time),
                              new InsertDataCell(data.EndedAt,   CellDataType.Time)
                          });

            _sheets.Append(sheet, row);
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
