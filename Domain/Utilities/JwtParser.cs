using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Domain.Utilities
{
    public static class JwtParser
    {
        public static IEnumerable<string> GetValidatedUserRoles(string jwt, TokenValidationParameters validationParameters)
        {
            new JwtSecurityTokenHandler()
                .ValidateToken(jwt, validationParameters, out var tokenSecure);

            if (tokenSecure == null)
            {
                return new List<string>();
            }

            var userRoles = ExtractUserRoles((JwtSecurityToken)tokenSecure);

            return userRoles;
        }

        private static List<string> ExtractUserRoles(JwtSecurityToken token)
        {
            var data = token.Payload["roles"];

            if (data?.ToString()?.IndexOf(",", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                return JsonConvert.DeserializeObject<List<string>>(data!.ToString()!)! ?? new List<string>();
            }
            else
            {
                return new List<string>() { data?.ToString()! };
            }
        }

        public static bool IsValid(string jwt)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(jwt);
            }
            catch (Exception)
            {
                return false;
            }

            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
        }
    }
}
