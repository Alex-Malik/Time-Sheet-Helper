using System;

namespace TimeSheet.Interfaces
{
    public interface IData
    {
        DateTime CreatedAt { get; set; }
        String   Content   { get; set; }
        String   Project   { get; set; }
        Double   Hours     { get; set; }
        DateTime StartedAt { get; set; }
        DateTime EndedAt   { get; set; }

        String FormatedCreatedAt { get; }
        String FormatedHours     { get; }
    }
}
