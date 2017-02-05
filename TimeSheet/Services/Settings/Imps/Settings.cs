using System;
using System.Collections.Generic;

namespace TimeSheet.Services.Settings.Imps
{
    using Interfaces;

    internal class Settings : IInsertSettings, ISheetSettings
    {
        #region IInsertSettings
        // View specific values.
        public Int32   StepInHours    { get; set; } = 1;
        public Int32   StepInMinutes  { get; set; } = 15;
        public Boolean AllowDataMerge { get; set; } = true;

        // Data specific values.
        public IEnumerable<String> Projects { get; set; } = new List<String>();
        #endregion IInsertSettings

        #region ISheetsSettings
        public String SpreadSheetID { get; set; } = String.Empty;
        public String SheetName     { get; set; } = String.Empty;
        #endregion ISheetsSettings
    }
}
