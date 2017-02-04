using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;

namespace TimeSheet.Services.Models
{
    using Interfaces;

    internal class GoogleRow : IRow
    {
        public GoogleRow(RowData row)
        {
            Source = row;
            
            List<ICell> cells = new List<ICell>();
            foreach (CellData cellData in row.Values)
            {
                ICell cell = Convert(cellData);
                if (cell != null)
                {
                    cells.Add(cell);
                }
            }

            Cells = cells;
        }

        public RowData Source { get; }

        public IEnumerable<ICell> Cells { get; }

        // TODO: Consider better solution.
        private ICell Convert(CellData cell)
        {
            if (cell == null) return null;
            if (cell.EffectiveValue == null) return null;
            if (cell.EffectiveValue.NumberValue.HasValue)
            {
                if (cell.EffectiveFormat?.NumberFormat != null)
                {
                    // TODO: Choose cell time (Date/Time/DateTime)
                    return new GoogleCell<DateTime>(cell, DateTime.FromOADate(cell.EffectiveValue.NumberValue.Value));
                }
                else
                {
                    return new GoogleCell<Double>(cell, cell.EffectiveValue.NumberValue.Value);
                }
            }
            else if (cell.EffectiveValue.BoolValue.HasValue)
            {
                return new GoogleCell<Boolean>(cell, cell.EffectiveValue.BoolValue.Value);
            }
            else
            {
                return new GoogleCell<String>(cell, cell.EffectiveValue.StringValue);
            }
        }
    }
}
