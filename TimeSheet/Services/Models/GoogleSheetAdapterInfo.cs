using System;

namespace TimeSheet.Services.Models
{
    using Interfaces;

    internal class GoogleSheetAdapterInfo : ISheetInfo
    {
        public GoogleSheetAdapterInfo(String name, Int32 index, Int32 rows, Int32 cols)
        {
            Name    = name;
            Index   = index;
            Rows    = rows;
            Cols = cols;
        }

        /// <summary>
        /// Gets the name (title) of the sheet.
        /// </summary>
        public String Name    { get; }

        /// <summary>
        /// Gets the index of the sheet.
        /// </summary>
        public Int32  Index   { get; }

        /// <summary>
        /// Gets the number of rows in the sheet.
        /// </summary>
        public Int32  Rows    { get; }

        /// <summary>
        /// Gets the number of columns int the sheet.
        /// </summary>
        public Int32  Cols    { get; }

        /// <summary>
        /// Get avaliable range of rows in A1 notation. 
        /// This is a string like Sheet1!A1:B2, that refers to 
        /// the first two cells in the top two rows of Sheet1.
        /// </summary>
        public String AvaliableRange
        {
            get
            {
                String sCol = "A";
                String eCol = "";
                Int32  sRow = 1;
                Int32  eRow = Rows;

                // TODO: Implement columns converting. (from numbers to A-Z)
                
                return $"{Name}!{sCol}{sRow}:{eCol}{eRow}";
            }
        }

        /// <summary>
        /// Get avaliable range of rows in A1 notation. 
        /// This is a string like Sheet1!1:2, that refers to 
        /// the all cells in the first two rows of Sheet1.
        /// </summary>
        public string AvaliableRows => $"{Name}!{1}:{Rows}";

        /// <summary>
        /// Get avaliable range of rows in A1 notation. 
        /// This is a string like Sheet1!A:A, that refers to 
        /// the all cells in the first column of Sheet1.
        /// </summary>
        public string AvaliableCols => $"{Name}!{1}:{Cols}";
    }
}
