using Domain.Entities.Application;
using Domain.Interfaces.Services.Application;
using Domain.Interfaces.Services.Authorization;
using Domain.Models;
using System.Security.Claims;

namespace Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHashService _passwordHashService;

        public AuthService(IUserService userService, IUserRoleService userRoleService, IJwtService jwtService, IPasswordHashService passwordHashService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _jwtService = jwtService;
            _passwordHashService = passwordHashService;

        }

        public AuthenticatedUser AuthenticateUser(UserLogin userLogin, string remoteIp)
        {
            var test = userLogin;
            if (userLogin == null)
            {
                return new AuthenticatedUser() { ApiMessage = "User Login Cannot Be Null" };
            }

            if (string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password))
            {
                return new AuthenticatedUser() { ApiMessage = "Username and Password Cannot Be Blank" };
            }

            if (string.IsNullOrEmpty(remoteIp))
            {
                return new AuthenticatedUser() { ApiMessage = "Must Supply Ip" };
            }

            var user = GetUserByEmail(userLogin.Username);

            if (user == null)
            {
                return new AuthenticatedUser() { ApiMessage = "Incorrect Email Or Password" };
            }

            var authenticate = AuthenticatePassword(userLogin.Password, user.Password!);

            if (!authenticate)
            {
                return new AuthenticatedUser() { ApiMessage = "Incorrect Email Or Password" };
            }

            var roles = GetUserRoles(user.Id);

            var accessToken = GetAccessToken(user.Email!, remoteIp, roles);

            return new AuthenticatedUser() {
                AccessToken = accessToken,
                ApiMessage = "Successfully Authenticated",
                Roles = roles.Select(x => x.Name)!,
                Username = user.Email
            };
        }

        public User? GetUserByEmail(string email)
        {
            return _userService.GetByEmail(email);
        }

        public List<Role> GetUserRoles(Guid id)
        {
            var userRoles = _userRoleService.GetAllUserRolesForUser(id).ToList();
            var roles = new List<Role>();
            userRoles.ForEach(x => roles.Add(x.Role!));
            return roles;
        }

        public string GetAccessToken(string email, string ip, List<Role> roles)
        {
            List<Claim> claims = new List<Claim>();

            roles.ForEach(x => { claims.Add(new Claim("roles", x.Name!)); });

            return _jwtService.CreateEncodedJwtToken(email, ip, DateTime.Now, claims);
        }

        public bool AuthenticatePassword(string suppliedPassword, string storedPassword)
        {
            return _passwordHashService.VerifyPassword(suppliedPassword, storedPassword);
        }
    }
}
