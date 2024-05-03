using Domain.Interfaces.Services.Authorization;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthService _authService;
        private IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<AuthenticatedUser> Login(UserLogin userLogin)
        {
            var remoteIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString()!;

            var result = _authService.AuthenticateUser(userLogin, remoteIp);

            return result;
        }
    }
}
