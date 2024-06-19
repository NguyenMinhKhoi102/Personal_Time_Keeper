using System;
namespace MyProject.Payload.Request
{
    public class LogTimeRequest
    {
        public required DateOnly LogDate { get; set; }
        public required float Duration { get; set; }
        public string? Description { get; set; }
        public required int ActivityTypeId { get; set; }
    }
}

