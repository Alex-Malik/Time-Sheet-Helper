using System;

namespace TimeSheet.Interfaces
{
    public interface ISheetsSettings : ISettings
    {
        // TODO: Consider how to architect it in generic way.

        String SpreadSheetID { get; set; }
        String SheetName     { get; set; }
    }
}
