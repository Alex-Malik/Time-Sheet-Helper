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
        public string Content { get; set; }
        public string Project { get; set; }
        public double? Hours { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public string FormatedCreatedAt => CreatedAt.HasValue ? CreatedAt.Value.ToString("yyyy-MM-dd") : "<empty>";
        public string FormatedHours => Hours.HasValue ? $"{(int)Math.Round(Hours.Value)} hours" : "<empty>";
    }
}
