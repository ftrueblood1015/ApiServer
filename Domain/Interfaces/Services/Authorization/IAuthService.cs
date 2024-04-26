using Domain.Entities.Application;
using Domain.Models;

namespace Domain.Interfaces.Services.Authorization
{
    public interface IAuthService
    {
        AuthenticatedUser AuthenticateUser(UserLogin userLogin, string remoteIp);
        public User? GetUserByEmail(string email);
        public List<Role> GetUserRoles(Guid id);
        public string GetAccessToken(string email, string ip, List<Role> roles);
        public bool AuthenticatePassword(string suppliedPassword, string storedPassword);
    }
}
