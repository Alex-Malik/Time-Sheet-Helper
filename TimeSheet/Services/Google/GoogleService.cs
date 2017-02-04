using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
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
    using global::Google.Apis.Sheets.v4.Data;
    using Interfaces;

    internal class GoogleService : ISheetsService
    {
        #region Private Fields

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string   _applicationName = "Time Sheet .NET Client ID";

        private SheetsService _service;

        #endregion

        public ISheet GetSheet(String spreadSheetId, String sheetName)
        {
            Initialize();
            Spreadsheet spreadsheet = LoadSpreadSheet(spreadSheetId, sheetName);
            Sheet sheet = spreadsheet.Sheets.First();

            return new GoogleSheetAdapter(spreadsheet, sheet);
        }

        public ISheet GetSheet(String spreadSheetId, Int32  sheetIndex)
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
            throw new NotImplementedException();
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


        private void Initialize()
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
    }

    // TODO: Move to the models folder.
    public class GoogleSheetAdapter : ISheetInfo, ISheetData
    {
        public GoogleSheetAdapter(Spreadsheet spreadsheet, Sheet sheet)
        {
            throw new NotImplementedException();
        }

        #region ISheet Support
        public String SpreadSheetID { get; }

        public String ID    { get; }
        public String Name  { get; }
        public Int32  Index { get; }
        #endregion ISheet Support

        #region ISheetInfo Support
        public Int32  RowsNumber { get; }
        public Int32  ColsNumber { get; }
        public String AvaliableRows  { get; }
        public String AvaliableCols  { get; }
        public String AvaliableRange { get; }
        #endregion ISheetInfo Support

        #region ISheetData Support
        public IEnumerable<IRow> Rows { get; }
        public IEnumerable<ICol> Cols { get; }
        #endregion ISheetData Support
    }
}
