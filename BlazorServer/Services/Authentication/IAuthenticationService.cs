using Domain.Models;

namespace BlazorServer.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task<AuthenticatedUser> AuthenticateUser(UserLogin userLogin);
        Task Logout();
    }
}
