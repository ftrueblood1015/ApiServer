using Domain.Entities.Application;
using Domain.Interfaces.Services.Application;

namespace ApiServer.Controllers.Application
{
    public class UserRoleController : ControllerBase<UserRole, IUserRoleService>
    {
        public UserRoleController(IUserRoleService service) : base(service)
        {
        }
    }
}
