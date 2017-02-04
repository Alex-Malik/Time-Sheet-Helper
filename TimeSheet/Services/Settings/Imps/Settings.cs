using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Settings.Imps
{
    using Interfaces;

    internal class Settings : IInsertSettings, ISheetsSettings
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
        String ISheetsSettings.SpreadSheetID { get; set; } = String.Empty;
        String ISheetsSettings.SheetName     { get; set; } = String.Empty;
        #endregion ISheetsSettings
    }
}
