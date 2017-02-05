using System;

namespace TimeSheet.Interfaces
{
    public interface ISheetSettings : ISettings
    {
        // TODO: Consider how to architect it in generic way.

        String SpreadSheetID { get; set; }
        String SheetName     { get; set; }
    }
}
