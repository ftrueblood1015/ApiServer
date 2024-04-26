using Domain.Configuration;
using Domain.Interfaces.Services.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services.Authentication
{
    public class JwtService : IJwtService
    {
        private Token _token;
        private TokenValidationParameters _tokenValidationParameters;

        public JwtService(ConfigOptions options)
        {
            _token = options.Token;
            _tokenValidationParameters = options.Token.CreateValidationParameters();
        }

        public string CreateEncodedJwtToken(string email, string ip, DateTime now, List<Claim> claims)
        {
            claims.Add(new Claim(ClaimTypes.Name, email));
            claims.Add(new Claim("ip_address", ip));

            var token = new JwtSecurityToken(
                _token.Issuer,
                _token.Audience,
                claims,
                expires: now.AddMinutes(_token.GetAccessTokenExpiry()),
                signingCredentials: _token.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string DecodeEmailFromJwtToken(string token)
        {
            new JwtSecurityTokenHandler().ValidateToken(token, _tokenValidationParameters, out var tokenSecure);

            if (tokenSecure == null)
            {
                return string.Empty;
            }

            var result = ((JwtSecurityToken)tokenSecure).Payload[ClaimTypes.Name].ToString();

            return result ?? string.Empty;
        }

        public IEnumerable<string> DecodeRolesFromJwtToken(string token)
        {
            new JwtSecurityTokenHandler().ValidateToken(token, _tokenValidationParameters, out var tokenSecure);

            if (tokenSecure == null)
            {
                return null!;
            }

            var data = ((JwtSecurityToken)tokenSecure).Payload["roles"];

            if (data?.ToString()?.IndexOf(",", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                return JsonConvert.DeserializeObject<List<string>>(data!.ToString()!)! ?? new List<string>();
            }
            else
            {
                return new List<string>() { data?.ToString()! };
            }
        }
    }
}
