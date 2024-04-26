using Domain.Entities.Application;
using Domain.Interfaces.Services.Application;

namespace ApiServer.Controllers.Application
{
    public class UserController : ControllerBase<User, IUserService>
    {
        public UserController(IUserService service) : base(service)
        {
        }
    }
}
