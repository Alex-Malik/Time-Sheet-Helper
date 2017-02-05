using System;
using System.Threading.Tasks;

namespace TimeSheet.Services.Insert
{
    using Imps;
    using Interfaces;

    internal class InsertService : IInsertService
    {
        // TODO: Move this constants to the settings.
        private const string DefaultSpreadSheetId = "1U8bBQtr4kFQkOeLoLlOrryFflDPzOb30ECDr8mCIDHo";
        private const string DefaultSheetName     = "Alex Malik";
        private readonly ISheetService   _sheets;
        private readonly ISettingsService _settings;

        public InsertService(ISettingsService settings, ISheetService sheets)
        {
            _sheets   = sheets;
            _settings = settings;
        }

        public void Save(IData data)
        {
            ISheetSettings settings = _settings.Load<ISheetSettings>();

            //ISheet sheet = _sheets.GetSheet(settings.SpreadSheetID, settings.SheetName);
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
            // TODO: Implement after SheetsService become async.
            throw new NotImplementedException();
        }

        public IInsertSettings LoadSettings() => _settings.Load<IInsertSettings>();
        public Task<IInsertSettings> LoadSettingsAsync() => _settings.LoadAsync<IInsertSettings>();

        public void SaveSettings(IInsertSettings settings) => _settings.Save(settings);
        public Task SaveSettingsAsync(IInsertSettings settings) => _settings.SaveAsync(settings);
    }
}
