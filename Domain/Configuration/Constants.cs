using Microsoft.IdentityModel.Tokens;

namespace Domain.Configuration
{
    public static class Constants
    {
        public static Dictionary<string, TokenValidationParameters> ValidationParameters { get; set; } = new();

        public static class Authentication
        {
            public const string BearerId = "Bearer";
            public const string IpAddress = "ip_address";
            public static readonly KeyValuePair<string, string> GrantType = new KeyValuePair<string, string>("grant_type", "password");
        }

        public static void AddTokenValidation(string name, TokenValidationParameters tokenValidation)
        {
            ValidationParameters.Add(name, tokenValidation);
        }
    }
}
