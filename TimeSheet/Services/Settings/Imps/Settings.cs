using System;
using System.Collections.Generic;

namespace TimeSheet.Services.Settings.Imps
{
    using Interfaces;

    internal class Settings : IInsertSettings, ISheetSettings
    {
        #region IInsertSettings
        // View specific values.
        Int32   IInsertSettings.StepInHours    { get; set; } = 1;
        Int32   IInsertSettings.StepInMinutes  { get; set; } = 15;
        Boolean IInsertSettings.AllowDataMerge { get; set; } = true;

        // Data specific values.
        IEnumerable<String> IInsertSettings.Projects { get; set; } = new List<String>();
        #endregion IInsertSettings

        #region ISheetsSettings
        String ISheetSettings.SpreadSheetID { get; set; } = String.Empty;
        String ISheetSettings.SheetName     { get; set; } = String.Empty;
        #endregion ISheetsSettings
    }
}
