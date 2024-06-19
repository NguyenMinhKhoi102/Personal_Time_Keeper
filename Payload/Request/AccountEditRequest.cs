using System;
namespace MyProject.Payload.Request
{
    public class AccountEditRequest
    {
        public required string FullName { get; set; }
        public string? Phone { get; set; }
    }
}

