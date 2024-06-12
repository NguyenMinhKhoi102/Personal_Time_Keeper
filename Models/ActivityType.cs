namespace MyProject.Models
{
    public class ActivityType : AuditableEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<LogTime>? LogTimes { get; set; }
    }
}

