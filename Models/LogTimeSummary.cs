using System;
namespace MyProject.Models
{
    public class LogTimeSummary : AuditableEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public required Account Account { get; set; }

        public ICollection<LogTime>? LogTimes { get; set; }
    }
}

