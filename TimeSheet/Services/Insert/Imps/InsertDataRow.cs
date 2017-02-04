using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
