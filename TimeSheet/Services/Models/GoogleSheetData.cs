using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;

namespace TimeSheet.Services.Models
{
    using Interfaces;

    internal class GoogleSheetData : ISheetData
    {
        public GoogleSheetData(Sheet sheet)
        {
            Name  = sheet.Properties.Title;
            Index = sheet.Properties.Index.Value;

            List<ICol> cols = new List<ICol>();
            List<IRow> rows = new List<IRow>();

            if (sheet.Data.Any())
            {
                GridData data = sheet.Data.First();
                foreach (DimensionProperties col in data.ColumnMetadata)
                {
                    // TODO: Consider best solution of how to use col info.
                    cols.Add(new GoogleCol());
                }
                foreach (RowData row in data.RowData)
                {
                    rows.Add(new GoogleRow(row));
                }
            }

            Cols = cols;
            Rows = rows;
        }

        public String Name { get; }

        public Int32  Index { get; }

        public IEnumerable<ICol> Cols { get; }

        public IEnumerable<IRow> Rows { get; }
    }
}
