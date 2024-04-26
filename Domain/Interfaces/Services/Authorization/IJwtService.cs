using System.Security.Claims;

namespace Domain.Interfaces.Services.Authorization
{
    public interface IJwtService
    {
        public string CreateEncodedJwtToken(string email, string ip, DateTime now, List<Claim> claims);
        public IEnumerable<string> DecodeRolesFromJwtToken(string token);
        public string DecodeEmailFromJwtToken(string token);
    }
}
