using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace TimeSheet.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using TimeSheet.Services.Models;

    public class GoogleSheetsServiceWrapper
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        private static string[] _scopes = { SheetsService.Scope.SpreadsheetsReadonly };

        private static string _applicationName = "Time Sheet .NET Client ID";

        private SheetsService _service;

        public GoogleSheetsServiceWrapper()
        {
        }

        public void Init()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.timesheet.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }

        // TODO: Redesign with return an objects list of time sheet records.
        public IEnumerable<RecordModel> Get(String spreadSheetId, String sheetName)
        {
            Spreadsheet spreadsheet = LoadSpreadSheet(spreadSheetId, sheetName);
            if (spreadsheet == null)
                return Enumerable.Empty<RecordModel>();
            if (spreadsheet.Sheets == null)
                return Enumerable.Empty<RecordModel>();
            if (spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName)?.Data == null)
                return Enumerable.Empty<RecordModel>();

            GridData data = spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName).Data[0];
            List<RecordModel> records = new List<RecordModel>();
            foreach (var row in data.RowData.Skip(1)) // TODO: Move skip count to settings.
            {
                // TODO: Implement cells merge.

                records.Add(new RecordModel
                {
                    CreatedAt = GetDateTime(GetValue(row, 0)),
                    Content   = GetString(GetValue(row, 1)),
                    Hours     = GetDouble(GetValue(row, 2)),
                    Project   = GetString(GetValue(row, 3)),
                    StartedAt = GetDateTime(GetValue(row, 4)),
                    EndedAt   = GetDateTime(GetValue(row, 5))
                });
            }

            return records
                .Where(r => r.CreatedAt.HasValue
                     && !String.IsNullOrEmpty(r.Content)
                     && !String.IsNullOrEmpty(r.Project)
                     && r.Hours.HasValue)
                .OrderByDescending(r => r.CreatedAt.Value);
        }

        public Task<IEnumerable<RecordModel>> GetAsync(String spreadSheetId, int sheetIndex = 0)
        {
            throw new NotImplementedException();
        }

        private Spreadsheet LoadSpreadSheet(String spreadSheetId, String sheetName)
        {
            SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(spreadSheetId);
            request.IncludeGridData = true;
            request.Ranges = sheetName;

            return request.Execute();
        }

        // TODO: Consider using extension for next actions.

        private CellData GetValue(RowData row, int index)
        {
            if (row == null) return null;
            if (row.Values == null) return null;
            if (row.Values.Count < index + 1) return null;

            return row.Values[index];
        }

        private DateTime? GetDateTime(CellData cell)
        {
            if (cell == null) return null;
            if (cell.EffectiveFormat == null) return null;
            if (cell.EffectiveFormat.NumberFormat == null) return null;
            if (cell.EffectiveFormat.NumberFormat.Type != "DATE") return null;
            if (cell.EffectiveValue == null) return null;
            if (cell.EffectiveValue.NumberValue.HasValue == false) return null;

            return DateTime.FromOADate(cell.EffectiveValue.NumberValue.Value);
        }

        private String GetString(CellData cell)
        {
            return cell?.EffectiveValue?.StringValue;
        }

        private Double? GetDouble(CellData cell)
        {
            return cell?.EffectiveValue?.NumberValue;
        }
    }
}