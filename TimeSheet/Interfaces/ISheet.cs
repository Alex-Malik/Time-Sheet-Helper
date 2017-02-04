using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface ISheet
    {
        String SpreadSheetID { get; }

        String ID    { get; }
        String Name  { get; }
        Int32  Index { get; }
    }

    public interface ISheetInfo : ISheet
    {
        Int32  RowsNumber { get; }
        Int32  ColsNumber { get; }
        String AvaliableRows  { get; }
        String AvaliableCols  { get; }
        String AvaliableRange { get; }
    }

    public interface ISheetData : ISheet
    {
        IEnumerable<IRow> Rows { get; }
        IEnumerable<ICol> Cols { get; }
    }

    public interface IRow
    {
        IEnumerable<ICell> Cells { get; }
    }

    public interface ICol
    {
        IEnumerable<ICell> Cells { get; }
    }

    public interface ICell
    {
        Object      Value  { get; }
        ICellFormat Format { get; }
    }

    public interface ICellFormat
    {
        CellDataType DataType   { get; }
        Object       DataFormat { get; }

        // TODO: Consider ICellFormat architecture.
    }

    public enum CellDataType
    {
        String,
        Int16,
        Int32,
        Int64,
        Single,
        Double,
        Date,
        Time,
        DateTime
    }
}
