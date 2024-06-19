namespace MyProject.Payload.Request
{
    public class RegisterRequest
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirmation { get; set; }
        public string? Phone { get; set; }
    }
}

