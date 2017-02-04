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
    using Converters;
    using Interfaces;
    using Models;

    public class GoogleService_Old
    {
        #region Private Fields

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string   _applicationName = "Time Sheet .NET Client ID";

        private readonly SheetsService _service;
        private readonly ICellBuilder<CellData> _builder;
        private readonly ISheetConverter _converter;

        #endregion

        #region Init

        public GoogleService_Old()
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

            _builder = new GoogleCellBuilder();
            _converter = new GoogleConverter();
        }
        
        #endregion

        #region Public Methods

        public IEnumerable<ISheetInfo> GetSheets(String spreadSheetId)
        {
            return GetSpreadSheet(spreadSheetId)
                .Sheets
                .Select(s => new GoogleSheetInfo(s));
        }

        public async Task<IEnumerable<ISheetInfo>> GetSheetsAsync(String spreadSheetId)
        {
            Spreadsheet spreadsheet = await GetSpreadSheetAsync(spreadSheetId);
            return spreadsheet
                .Sheets
                .Select(s => new GoogleSheetInfo(s));
        }

        public ISheetInfo GetSheetInfo(String spreadSheetId, Int32 sheetIndex)
        {
            throw new NotImplementedException();
        }

        public async Task<ISheetInfo> GetSheetInfoAsync(String spreadSheetId, Int32 sheetIndex)
        {
            throw new NotImplementedException();
        }

        public ISheetInfo GetSheetInfo(String spreadSheetId, String sheetName)
        {
            Spreadsheet spreadsheet = GetSpreadSheet(spreadSheetId, sheetName);
            Sheet sheet = spreadsheet.Sheets.First();

            return new GoogleSheetInfo(sheet);
        }

        public async Task<ISheetInfo> GetSheetInfoAsync(String spreadSheetId, String sheetName)
        {
            Spreadsheet spreadsheet = await GetSpreadSheetAsync(spreadSheetId, sheetName);
            Sheet sheet = spreadsheet.Sheets.First();

            return new GoogleSheetInfo(sheet);
        }
        
        public IEnumerable<IData> GetData(String spreadSheetId, String sheetName)
        {
            // TODO: Optimize records loading.
            //        + Remove full loading of the spreadsheet.
            //        + Remove full loading of the sheet.
            //        - Optimize cells converting (from CellData to a model).

            // Load sheet info.
            ISheetInfo info = GetSheetInfo(spreadSheetId, sheetName);

            // Load sheet data.
            ISheetData data = GetSheetData(spreadSheetId, info);

            // Option I
            //// Convert loaded sheet with data to a list of IData instances.
            //return _converter.Convert(data);

            // TODO: Find best solution between this two options.

            // Option II
            return data.Rows
                .Select(x => x as GoogleRow)
                .Select(x => x.Source)
                .Select(x => new Data
                {
                    CreatedAt = GetDate(GetValue(x, 0)),
                    Content   = GetString(GetValue(x, 1)),
                    Hours     = GetDouble(GetValue(x, 2)),
                    Project   = GetString(GetValue(x, 3)),
                    StartedAt = GetDate(GetValue(x, 4)),
                    EndedAt   = GetDate(GetValue(x, 5))
                })
                .Where(r => r.CreatedAt.HasValue
                     && !String.IsNullOrEmpty(r.Content)
                     && !String.IsNullOrEmpty(r.Project)
                     && r.Hours.HasValue)
                .OrderByDescending(r => r.CreatedAt.Value);

            // OLD CODE:
            //SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(spreadSheetId);
            //request.IncludeGridData = true;
            //request.Ranges = sheetName;

            //Spreadsheet spreadsheet = request.Execute();
            //if (spreadsheet == null)
            //    return Enumerable.Empty<Data>();
            //if (spreadsheet.Sheets == null)
            //    return Enumerable.Empty<Data>();
            //if (spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName)?.Data == null)
            //    return Enumerable.Empty<Data>();

            //GridData griddata = spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == sheetName).Data[0];
            //List<Data> records = new List<Data>();
            //foreach (var row in griddata.RowData.Skip(1)) // TODO: Move skip count to settings.
            //{
            //    // TODO: Implement cells merge.

            //    records.Add(new Data
            //    {
            //        CreatedAt = GetDate(GetValue(row, 0)),
            //        Content = GetString(GetValue(row, 1)),
            //        Hours = GetDouble(GetValue(row, 2)),
            //        Project = GetString(GetValue(row, 3)),
            //        StartedAt = GetDate(GetValue(row, 4)),
            //        EndedAt = GetDate(GetValue(row, 5))
            //    });
            //}

            //return records
            //    .Where(r => r.CreatedAt.HasValue
            //         && !String.IsNullOrEmpty(r.Content)
            //         && !String.IsNullOrEmpty(r.Project)
            //         && r.Hours.HasValue)
            //    .OrderByDescending(r => r.CreatedAt.Value);
        }

        public Task<IEnumerable<Data>> GetAsync(String spreadSheetId, int sheetIndex = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates and inserts new time record with wollowing data to the timesheet.
        /// </summary>
        public void Insert(String spreadSheetId, String sheetName, DateTime date, String message, String project, Double hours, DateTime startAt, DateTime endAt)
        {
            // TODO: Consider using record model for input values.
            // TODO: Consider using local cache for the table values.

            // Row data to be appended (note: data must be declared as entered by user).
            RowData appendingRow = new RowData
            {
                Values = new[] {
                    GoogleCellBuilder.New.FromDate(date).Build(),
                    GoogleCellBuilder.New.FromString(message).Build(),
                    GoogleCellBuilder.New.FromDouble(hours).Build(),
                    GoogleCellBuilder.New.FromString(project).Build(),
                    GoogleCellBuilder.New.FromTime(startAt).Build(),
                    GoogleCellBuilder.New.FromTime(endAt).Build()
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
        
        private Spreadsheet GetSpreadSheet(String spreadSheetId, String range = null, Boolean includeData = false)
        {
            SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(spreadSheetId);
            request.Ranges = range;
            request.IncludeGridData = includeData;
            // TODO: Consider throw an exception is spreadsheet wasn't returned.

            return request.Execute();
        }

        private Task<Spreadsheet> GetSpreadSheetAsync(String spreadSheetId, String range = null, Boolean includeData = false)
        {
            SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(spreadSheetId);
            request.Ranges = range;
            request.IncludeGridData = includeData;

            return request.ExecuteAsync();
        }

        private ISheetData GetSheetData(String spreadSheetId, ISheetInfo info)
        {
            Spreadsheet spreadSheet = GetSpreadSheet(spreadSheetId, info.AvaliableRows, true);  // TODO: Consider get avaliable range.
            Sheet sheet = spreadSheet.Sheets.First();

            return new GoogleSheetData(sheet);
        }

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