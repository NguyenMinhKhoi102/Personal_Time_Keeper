namespace MyProject.Models
{
    public class LogTime : AuditableEntity
    {
        public int Id { get; set; }
        public DateOnly LogDate { get; set; }
        public float Duration { get; set; }
        public string? Description { get; set; }

        public int ActivityTypeId { get; set; }
        public required ActivityType ActivityType { get; set; }

        public int AccountId { get; set; }
        public required Account Account { get; set; }

    }
}

