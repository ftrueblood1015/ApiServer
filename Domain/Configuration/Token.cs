using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Configuration
{
    public class Token
    {
        public string Audience { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public SymmetricSecurityKey SecurityKey => new(Encoding.UTF8.GetBytes(Key));

        public SigningCredentials SigningCredentials => new(SecurityKey, SecurityAlgorithms.HmacSha256);

        public string TokenAccessExpiry { get; set; } = string.Empty;

        public TokenValidationParameters CreateValidationParameters() => new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKey,
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            RequireExpirationTime = true,
            ValidateLifetime = true,
        };

        public double GetAccessTokenExpiry()
        {
            if (!double.TryParse(TokenAccessExpiry, out double expiryMinutes))
            {
                expiryMinutes = 1440;
            }

            return expiryMinutes;
        }
    }
}
