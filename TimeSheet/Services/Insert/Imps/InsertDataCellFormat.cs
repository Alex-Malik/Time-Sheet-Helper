using System;

namespace TimeSheet.Services.Insert.Imps
{
    using Interfaces;

    internal class InsertDataCellFormat : ICellFormat
    {
        public InsertDataCellFormat(CellDataType type, String format = null)
        {
            DataType = type;
            DataFormat = format;
        }

        public CellDataType DataType   { get; }
        public Object       DataFormat { get; }
    }
}
