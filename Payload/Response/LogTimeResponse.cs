using System;
namespace MyProject.Payload.Response
{
    public class LogTimeResponse
    {
        public int Id { get; set; }
        public DateOnly LogDate { get; set; }
        public float Duration { get; set; }
        public int ActivityTypeId { get; set; }
        public required string ActivityTypeName { get; set; }
        public string? Description { get; set; }
    }
}

