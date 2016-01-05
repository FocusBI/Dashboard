using System;

namespace FoucsBI.Dashboard.Models
{
    public class Executable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PackageName { get; set; }
        public string PackagePath { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
        public int? ExecutionResult { get; set; }
        public string ExecutionValue { get; set; }
        public long ExecutionId { get; set; }
        
        public string RunDateString
        {
            get
            {
                return this.StartTime.HasValue ? this.StartTime.Value.ToShortTimeString() : string.Empty;
            }
        }
        public string StartTimeString
        {
            get
            {
                return this.StartTime.HasValue ? this.StartTime.Value.ToShortTimeString() : string.Empty;
            }
        }
        public string EndTimeString
        {
            get
            {
                return this.EndTime.HasValue ? this.EndTime.Value.ToShortTimeString() : string.Empty;
            }
        }
    }
}


