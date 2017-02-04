using System;
using System.Collections.Generic;

namespace TimeSheet.Interfaces
{
    public interface IInsertSettings : ISettings
    {
        // View specific values.
        Int32   StepInHours    { get; set; }
        Int32   StepInMinutes  { get; set; }
        Boolean AllowDataMerge { get; set; }

        // Data specific values.
        IEnumerable<String> Projects { get; set; }
    }
}
