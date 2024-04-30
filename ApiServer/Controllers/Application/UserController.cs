using Domain.Entities.Application;
using Domain.Interfaces.Services.Application;

namespace ApiServer.Controllers.Application
{
    public class UserController : ApplicationControllerBase<User, IUserService>
    {
        public UserController(IUserService service) : base(service)
        {
        }
    }
}
