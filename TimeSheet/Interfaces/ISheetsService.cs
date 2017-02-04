using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface ISheetsService
    {
        ISheet     GetSheet(String spreadSheetId, String sheetName);
        ISheet     GetSheet(String spreadSheetId, Int32  sheetIndex);

        ISheetInfo GetSheetInfo(ISheet sheet);
        ISheetInfo GetSheetInfo(String spreadSheetId, String sheetName);
        ISheetInfo GetSheetInfo(String spreadSheetId, Int32  sheetIndex);

        ISheetData GetSheetData(ISheet sheet);
        ISheetData GetSheetData(String spreadSheetId, String sheetName);
        ISheetData GetSheetData(String spreadSheetId, Int32  sheetIndex);

        IEnumerable<ISheetInfo> GetSheets(String spreadSheetId);

        void Append(ISheet sheet, IRow row);
        void Update(ISheet sheet, IRow row);
        void Delete(ISheet sheet, IRow row);
        void Insert(ISheet sheet, IRow row, Int32 index);

        // TODO: Add async methods for each of above.
    }
}
