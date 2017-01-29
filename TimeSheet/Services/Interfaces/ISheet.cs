using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Interfaces
{
    public interface ISheet
    {
        String Name  { get; }
        Int32  Index { get; }
    }

    public interface ISheetInfo : ISheet
    {
        Int32  Rows    { get; }
        Int32  Cols    { get; }
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

    }

    public interface ICol
    {

    }
}
