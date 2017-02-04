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

namespace TimeSheet.Services.Google
{
    using Imps;
    using Interfaces;

    internal class GoogleService : ISheetsService
    {
        #region Private Fields
        private const string NumberFormatText       = "TEXT";
        private const string NumberFormatNumber     = "NUMBER";
        private const string NumberFormatPercent    = "PERCENT";
        private const string NumberFormatCurrency   = "CURRENCY";
        private const string NumberFormatDate       = "DATE";
        private const string NumberFormatTime       = "TIME";
        private const string NumberFormatDateTime   = "DATE_TIME";
        private const string NumberFormatScientific = "SCIENTIFIC";

        private const string NumberFormatDatePattern     = "dd.mm.yyyy";
        private const string NumberFormatTimePattern     = "hh:mm";
        private const string NumberFormatDateTimePattern = "";

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string   _applicationName = "Time Sheet .NET Client ID";

        private SheetsService _service;
        #endregion

        #region Initialize
        public GoogleService()
        {

        }
        #endregion Initialize

        public ISheet GetSheet(String spreadSheetId, String sheetName)
        {
            if (String.IsNullOrEmpty(spreadSheetId))
                throw new ArgumentNullException(nameof(spreadSheetId));
            if (String.IsNullOrEmpty(sheetName))
                throw new ArgumentNullException(nameof(sheetName));

            Initialize();

            Spreadsheet spreadSheet = LoadSpreadSheet(spreadSheetId, sheetName);
            Sheet sheet = spreadSheet.Sheets.First();
            // TODO: Consider throw an exception if sheet wasn't loaded.

            return new GoogleSheetAdapter(spreadSheet, sheet);
        }

        public ISheet GetSheet(String spreadSheetId, Int32 sheetIndex)
        {
            throw new NotImplementedException();
        }


        public ISheetInfo GetSheetInfo(ISheet sheet)
        {
            throw new NotImplementedException();
        }

        public ISheetInfo GetSheetInfo(String spreadSheetId, String sheetName)
        {
            throw new NotImplementedException();
        }

        public ISheetInfo GetSheetInfo(String spreadSheetId, Int32  sheetIndex)
        {
            throw new NotImplementedException();
        }


        public ISheetData GetSheetData(ISheet sheet)
        {
            throw new NotImplementedException();
        }

        public ISheetData GetSheetData(String spreadSheetId, String sheetName)
        {
            throw new NotImplementedException();
        }

        public ISheetData GetSheetData(String spreadSheetId, Int32  sheetIndex)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ISheetInfo> GetSheets(String spreadSheetId)
        {
            throw new NotImplementedException();
        }


        public void Append(ISheet sheet, IRow row)
        {
            // TODO: Consider implement GoogleAPIWrapper for requests building.

            RowData newRow = new RowData();
            newRow.Values = new List<CellData>();

            foreach (ICell cell in row.Cells)
            {
                CellData newCell = Convert(cell);
                if (newCell != null)
                    newRow.Values.Add(newCell);
            }

            // Create 'append cells' request for the current sheet.
            AppendCellsRequest appendRequest = new AppendCellsRequest();
            appendRequest.SheetId = 0;
            appendRequest.Rows = new[] { newRow };
            appendRequest.Fields = "*";

            // Wrap it into request.
            Request request = new Request();
            request.AppendCells = appendRequest;

            // Wrap it into batch update request.
            BatchUpdateSpreadsheetRequest batchRequst = new BatchUpdateSpreadsheetRequest();
            batchRequst.Requests = new[] { request };

            // Finally update the sheet.
            _service.Spreadsheets
                .BatchUpdate(batchRequst, sheet.SpreadSheetID)
                .Execute();
        }

        public void Update(ISheet sheet, IRow row)
        {
            throw new NotImplementedException();
        }

        public void Delete(ISheet sheet, IRow row)
        {
            throw new NotImplementedException();
        }

        public void Insert(ISheet sheet, IRow row, Int32 index)
        {
            throw new NotImplementedException();
        }


        private void Initialize(bool forced = false)
        {
            // Return back if service already initialized.
            if (!forced && _service != null) return;

            UserCredential credential;

            using (var stream = new FileStream
                ("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, 
                    ".credentials/sheets.googleapis.timesheet.json");

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

        private Spreadsheet LoadSpreadSheet
            (String  spreadSheetId, 
             String  range = null, 
             Boolean includeData = false)
        {
            SpreadsheetsResource.GetRequest request = 
                _service.Spreadsheets.Get(spreadSheetId);
            request.Ranges = range;
            request.IncludeGridData = includeData;
            // TODO: Consider throw an exception is spreadsheet wasn't returned.

            return request.Execute();
        }

        private CellData Convert(ICell cell)
        {
            if (cell == null)
                return null;
            else if (cell.Value is String || cell.Value is Char)
                return ConvertToString(cell);
            else if (cell.Value is Int16)
                return ConvertToInt16(cell);
            else if (cell.Value is Int32)
                return ConvertToInt32(cell);
            else if (cell.Value is Int64)
                return ConvertToInt64(cell);
            else if (cell.Value is Single)
                return ConvertToSingle(cell);
            else if (cell.Value is Double)
                return ConvertToDouble(cell);
            else if (cell.Value is DateTime && cell.Format?.DataType == CellDataType.Date)
                return ConvertToDate(cell);
            else if (cell.Value is DateTime && cell.Format?.DataType == CellDataType.Time)
                return ConvertToTime(cell);
            else if (cell.Value is DateTime)
                return ConvertToDateTime(cell);
            else
                return ConvertToString(cell);
        }

        private CellData ConvertToString(ICell cell)
        {
            return new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = (String)cell.Value
                },
                UserEnteredFormat = new CellFormat
                {
                    
                }
            };
        }

        private CellData ConvertToInt16(ICell cell)  => ConvertToInt32(cell);

        private CellData ConvertToInt32(ICell cell)  => ConvertToInt64(cell);

        private CellData ConvertToInt64(ICell cell)  => ConvertToSingle(cell);

        private CellData ConvertToSingle(ICell cell) => ConvertToDouble(cell);

        private CellData ConvertToDouble(ICell cell)
        {
            return new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    NumberValue = (Double)cell.Value
                },
                UserEnteredFormat = new CellFormat
                {

                }
            };
        }

        private CellData ConvertToDate(ICell cell)
        {
            return new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    NumberValue = ((DateTime)cell.Value).ToOADate()
                },
                UserEnteredFormat = new CellFormat
                {
                    NumberFormat = new NumberFormat
                    {
                        Type = NumberFormatDate,
                        Pattern = cell.Format?.DataFormat as String ?? NumberFormatDatePattern
                    }
                }
            };
        }

        private CellData ConvertToTime(ICell cell)
        {
            return new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    NumberValue = ((DateTime)cell.Value).ToOADate()
                },
                UserEnteredFormat = new CellFormat
                {
                    NumberFormat = new NumberFormat
                    {
                        Type = NumberFormatTime,
                        Pattern = cell.Format?.DataFormat as String ?? NumberFormatTimePattern
                    }
                }
            };
        }

        private CellData ConvertToDateTime(ICell cell)
        {
            return new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    NumberValue = ((DateTime)cell.Value).ToOADate()
                },
                UserEnteredFormat = new CellFormat
                {
                    NumberFormat = new NumberFormat
                    {
                        Type = NumberFormatDateTime,
                        Pattern = cell.Format?.DataFormat as String ?? NumberFormatDateTimePattern
                    }
                }
            };
        }
    }
}
