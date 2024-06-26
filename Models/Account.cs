﻿namespace MyProject.Models
{
    public class Account : AuditableEntity
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string Password { get; set; }
        public string? Phone { get; set; }

        public ICollection<LogTime>? LogTimes { get; set; }
    }
}

