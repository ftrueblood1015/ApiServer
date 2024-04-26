namespace Domain.Models
{
    public class AuthenticatedUser
    {
        public string? AccessToken { get; set; }
        public string? ApiMessage { get; set; } = string.Empty;
        public IEnumerable<string>? Roles { get; set; }
        public string? Username { get; set; }
    }
}
