using System.Collections.Generic;

namespace TimeSheet.Services.Insert.Imps
{
    using Interfaces;

    internal class InsertDataRow : IRow
    {
        public InsertDataRow(IEnumerable<ICell> cells)
        {
            Cells = cells;
        }

        public IEnumerable<ICell> Cells { get; }
    }
}
