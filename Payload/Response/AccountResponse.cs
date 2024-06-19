namespace MyProject.Payload.Response
{
    public class AccountResponse
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public string? Phone { get; set; }
    }
}

