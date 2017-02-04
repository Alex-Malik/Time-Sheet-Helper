using System;

namespace TimeSheet.Services.Insert.Imps
{
    using Interfaces;

    internal class InsertDataCell : ICell
    {
        public InsertDataCell(Object value)
        {
            Value = value;
        }

        public InsertDataCell(Object value, CellDataType type, String format = null)
            : this(value)
        {
            Format = new InsertDataCellFormat(type, format);
        }

        public Object      Value  { get; }
        public ICellFormat Format { get; }
    }
}
