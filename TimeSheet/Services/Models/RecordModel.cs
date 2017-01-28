using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Models
{
    public class RecordModel
    {
        public DateTime? CreatedAt { get; set; }
        public String    Content { get; set; }
        public String    Project { get; set; }
        public Double?   Hours { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public String FormatedCreatedAt => CreatedAt.HasValue ? CreatedAt.Value.ToString("yyyy-MM-dd") : "<empty>";
        public String FormatedHours => Hours.HasValue ? $"{(int)Math.Round(Hours.Value)} hours" : "<empty>";
    }
}
