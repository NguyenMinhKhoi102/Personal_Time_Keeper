using System;
namespace MyProject.Models
{
    public class LogTime : AuditableEntity
    {
        public int Id { get; set; }
        public DateOnly LogDate { get; set; }
        public float Duration { get; set; }

        public int ActivityTypeId { get; set; }
        public required ActivityType ActivityType { get; set; }

        public int LogTimeSummaryId { get; set; }
        public required LogTimeSummary LogTimeSummary { get; set; }
    }
}

