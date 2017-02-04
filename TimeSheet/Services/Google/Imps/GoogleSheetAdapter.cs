using System;
using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;

namespace TimeSheet.Services.Google.Imps
{
    using Interfaces;

    internal class GoogleSheetAdapter : ISheetInfo, ISheetData
    {
        private readonly Int32 _rowsOffset;
        private readonly Int32 _colsOffset;

        public GoogleSheetAdapter(Spreadsheet spreadSheet, Sheet sheet)
        {
            if (spreadSheet == null)
                throw new ArgumentNullException(nameof(spreadSheet));
            if (sheet == null)
                throw new ArgumentNullException(nameof(sheet));

            // Base ISheet implementation.
            SpreadSheetID = spreadSheet.SpreadsheetId;

            ID    = sheet.Properties.SheetId.Value.ToString();
            Name  = sheet.Properties.Title;
            Index = sheet.Properties.Index.Value;

            // The ISheetInfo implementation is optional.
            RowsNumber  = sheet.Properties.GridProperties?.RowCount ?? -1;
            ColsNumber  = sheet.Properties.GridProperties?.ColumnCount ?? -1;
            _rowsOffset = sheet.Properties.GridProperties?.FrozenRowCount ?? 0;
            _colsOffset = sheet.Properties.GridProperties?.FrozenColumnCount ?? 0;
        }

        public GoogleSheetAdapter
            (Spreadsheet spreadSheet, 
             Sheet sheet, 
             IEnumerable<IRow> rows, 
             IEnumerable<ICol> cols) 
            : this (spreadSheet, sheet)
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
        public String AvaliableRows  => RowsNumber >= 0 ? $"{Name}!{_rowsOffset + 1}:{_rowsOffset + RowsNumber}" : String.Empty;
        public String AvaliableCols  => ColsNumber >= 0 ? $"{Name}!{(char)('A' + _colsOffset + 1)}:{(char)('A' + _colsOffset + ColsNumber)}" : String.Empty;
        public String AvaliableRange => $"";
        #endregion ISheetInfo Support

        #region ISheetData Support
        public IEnumerable<IRow> Rows { get; }
        public IEnumerable<ICol> Cols { get; }
        #endregion ISheetData Support
    }
}
