namespace MyProject.Models
{
    public class Account : AuditableEntity
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }

        public ICollection<LogTimeSummary>? LogTimeSummaries { get; set; }
    }
}

