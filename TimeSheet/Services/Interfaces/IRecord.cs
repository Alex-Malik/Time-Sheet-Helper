using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Interfaces
{
    public interface IRecord
    {
        DateTime? CreatedAt { get; set; }
        String    Content   { get; set; }
        String    Project   { get; set; }
        Double?   Hours     { get; set; }
        DateTime? StartedAt { get; set; }
        DateTime? EndedAt   { get; set; }

        String FormatedCreatedAt { get; }
        String FormatedHours     { get; }
    }
}
