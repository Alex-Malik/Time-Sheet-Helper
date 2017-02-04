using System;

namespace TimeSheet.Services.Models
{
    using Interfaces;

    public class Data : IData
    {
        public DateTime CreatedAt { get; set; }
        public String   Content   { get; set; }
        public String   Project   { get; set; }
        public Double   Hours     { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt   { get; set; }

        public String FormatedCreatedAt => CreatedAt.ToString("yyyy-MM-dd");
        public String FormatedHours     => $"{(int)Math.Round(Hours)} hours";
    }
}
