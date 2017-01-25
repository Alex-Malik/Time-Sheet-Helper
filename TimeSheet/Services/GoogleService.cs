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
        public IEnumerable<RecordModel> Get(String spreadSheetId, int sheetIndex = 0)
        {
            SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(spreadSheetId);
            request.IncludeGridData = true;
            
            Spreadsheet spreadsheet = request.Execute();
            if (spreadsheet == null)
                return Enumerable.Empty<RecordModel>();

            GridData data = spreadsheet.Sheets[sheetIndex].Data[0];

            if (data == null)
                return Enumerable.Empty<RecordModel>();

            List<RecordModel> records = new List<RecordModel>();
            foreach (var row in data.RowData)
            {
                records.Add(new RecordModel
                {
                    Content = row.Values[1]?.EffectiveValue?.StringValue
                });
            }

            return records;
        }
    }
}