namespace Domain.Configuration
{
    public class ConfigOptions
    {
        public string CorsPolicy { get; set; } = "DefaultCorsPolicy";

        public string? Environment { get; set; } = null;

        public Token Token { get; set; } = null!;

        public string AuthPrefix { get; set; } = string.Empty;
    }
}
