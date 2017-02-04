using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Models.Google
{
    using Interfaces;

    internal class SheetWrapper : ISheet
    {
        internal SheetWrapper(String spreadSheetID, String nameAndID, Int32 index)
        {
            SpreadSheetID = spreadSheetID;

            ID    = nameAndID;
            Name  = nameAndID;
            Index = index;
        }

        public String SpreadSheetID { get; }

        public String ID    { get; }
        public String Name  { get; }
        public Int32  Index { get; }
    }

    internal class SheetInfo : SheetWrapper, ISheetInfo
    {
        internal SheetInfo(String spreadSheetID, String nameAndID, Int32 index, Int32 rows, Int32 cols)
            : base (spreadSheetID, nameAndID, index)
        {
            Rows = rows;
            Cols = cols;
        }

        internal SheetInfo(SheetWrapper sheet, Int32 rows, Int32 cols)
            : base(sheet.SpreadSheetID, sheet.Name, sheet.Index)
        {
            Rows = rows;
            Cols = cols;
        }

        public Int32  Rows { get; }
        public Int32  Cols { get; }
        public String AvaliableRows  => $"{Name}!{1}:{Rows}";
        public String AvaliableCols  => $"{Name}!{1}:{Cols}";
        public String AvaliableRange => $"";
    }
}
