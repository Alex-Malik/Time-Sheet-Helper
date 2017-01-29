using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeSheet.Services
{
    using Builders;
    using Interfaces;
    using Models;

    public class GoogleService
    {
        #region Private Fields

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string   _applicationName = "Time Sheet .NET Client ID";

        private readonly SheetsService _service;
        private readonly ICellBuilder<CellData> _cellBuilder;

        #endregion

        #region Init

        public GoogleService()
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

            _cellBuilder = new CellBuilder();
        }
        
        #endregion

        #region Public Methods

        public IEnumerable<ISheetInfo> GetSheets(String spreadSheetId)
        {
            Spreadsheet spreadsheet = _service.Spreadsheets.Get(spreadSheetId).Execute();
            // TODO: Consider throw an exception is spreadsheet wasn't returned.

            return spreadsheet.Sheets
                .Select(s => new GoogleSheetAdapterInfo(
                    s.Properties.Title,
                    s.Properties.Index.Value,
                    s.Properties.GridProperties.RowCount.Value,
                    s.Properties.GridProperties.ColumnCount.Value));
        }

        public async Task<IEnumerable<ISheetInfo>> GetSheetsAsync(String spreadSheetId)
        {
            Spreadsheet spreadsheet = await _service.Spreadsheets.Get(spreadSheetId).ExecuteAsync();
            // TODO: Consider throw an exception is spreadsheet wasn't returned.

            return spreadsheet.Sheets
                .Select(s => new GoogleSheetAdapterInfo(
                    s.Properties.Title, 
                    s.Properties.Index.Value,
                    s.Properties.GridProperties.RowCount.Value,
                    s.Properties.GridProperties.ColumnCount.Value));
        }

        public ISheetInfo GetSheet(String spreadSheetId, Int32 sheetIndex)
        {
            throw new NotImplementedException();
        }

        public async Task<ISheetInfo> GetSheetAsync(String spreadSheetId, Int32 sheetIndex)
        {
            throw new NotImplementedException();
        }

        public ISheetInfo GetSheet(String spreadSheetId, String sheetName)
        {
            SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(spreadSheetId);
            request.Ranges = sheetName;

            Spreadsheet spreadsheet = _service.Spreadsheets.Get(spreadSheetId).Execute();
            // TODO: Consider throw an exception is spreadsheet wasn't returned.

            Sheet sheet = spreadsheet.Sheets[0];

            return new GoogleSheetAdapterInfo(
                    sheet.Properties.Title,
                    sheet.Properties.Index.Value,
                    sheet.Properties.GridProperties.RowCount.Value,
                    sheet.Properties.GridProperties.ColumnCount.Value);
        }

        public async Task<ISheetInfo> GetSheetAsync(String spreadSheetId, String sheetName)
        {
            SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(spreadSheetId);
            request.Ranges = sheetName;

            Spreadsheet spreadsheet = await _service.Spreadsheets.Get(spreadSheetId).ExecuteAsync();
            // TODO: Consider throw an exception is spreadsheet wasn't returned.

            Sheet sheet = spreadsheet.Sheets[0];

            return new GoogleSheetAdapterInfo(
                    sheet.Properties.Title,
                    sheet.Properties.Index.Value,
                    sheet.Properties.GridProperties.RowCount.Value,
                    sheet.Properties.GridProperties.ColumnCount.Value);
        }

        public IEnumerable<IRecord> Get(String spreadSheetId, String sheetName)
        {
            // TODO: Optimize records loading.
            //        - Remove full loading of the spreadsheet.
            //        - Remove full loading of the sheet.
            //        - Optimize cells converting (from CellData to a model).

            #region New Implementation

            // Load sheet info.
            ISheetInfo info = GetSheet(spreadSheetId, sheetName);

            // TODO: Load sheet data.
            ISheetData data = null;

            // TODO: Convert loaded data to IRecords.

            #endregion

            SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(spreadSheetId);
            //request.IncludeGridData = true;
            //request.Ranges = sheetName;

            Spreadsheet spreadsheet = request.Execute();
            if (spreadsheet == null)
                return Enumerable.Empty<RecordModel>();
            if (spreadsheet.Sheets == null)
                return Enumerable.Empty<RecordModel>();
            if (spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName)?.Data == null)
                return Enumerable.Empty<RecordModel>();

            GridData griddata = spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName).Data[0];
            List<RecordModel> records = new List<RecordModel>();
            foreach (var row in griddata.RowData.Skip(1)) // TODO: Move skip count to settings.
            {
                // TODO: Implement cells merge.

                records.Add(new RecordModel
                {
                    CreatedAt = GetDate(GetValue(row, 0)),
                    Content = GetString(GetValue(row, 1)),
                    Hours = GetDouble(GetValue(row, 2)),
                    Project = GetString(GetValue(row, 3)),
                    StartedAt = GetDate(GetValue(row, 4)),
                    EndedAt = GetDate(GetValue(row, 5))
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

        /// <summary>
        /// Creates and inserts new time record with wollowing data to the timesheet.
        /// </summary>
        /// <param name="date">The date mark of the record.</param>
        /// <param name="message"></param>
        /// <param name="project"></param>
        /// <param name="startAt"></param>
        /// <param name="endAt"></param>
        public void Insert(String spreadSheetId, String sheetName, DateTime date, String message, String project, Double hours, DateTime startAt, DateTime endAt)
        {
            // TODO: Consider using record model for input values.
            // TODO: Consider using local cache for the table values.

            // Row data to be appended (note: data must be declared as entered by user).
            RowData appendingRow = new RowData
            {
                Values = new[] {
                    CellBuilder.New.FromDate(date).Build(),
                    CellBuilder.New.FromString(message).Build(),
                    CellBuilder.New.FromDouble(hours).Build(),
                    CellBuilder.New.FromString(project).Build(),
                    CellBuilder.New.FromTime(startAt).Build(),
                    CellBuilder.New.FromTime(endAt).Build()
                }
            };

            // Create append cells request for the current sheet.
            AppendCellsRequest appendRequest = new AppendCellsRequest();
            appendRequest.SheetId = 0;
            appendRequest.Rows = new[] { appendingRow };
            appendRequest.Fields = "*";

            Request request = new Request();
            request.AppendCells = appendRequest;

            BatchUpdateSpreadsheetRequest batchRequst = new BatchUpdateSpreadsheetRequest();
            batchRequst.Requests = new[] { request };

            _service.Spreadsheets.BatchUpdate(batchRequst, spreadSheetId).Execute();
        }

        #endregion

        #region Private Methods
        
        // TODO: Consider using extension for next actions.

        private CellData GetValue(RowData row, int index)
        {
            if (row == null) return null;
            if (row.Values == null) return null;
            if (row.Values.Count < index + 1) return null;

            return row.Values[index];
        }

        private DateTime? GetDate(CellData cell)
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
                
        #endregion
    }
}