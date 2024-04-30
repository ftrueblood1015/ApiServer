using Domain.Entities.Application;
using Domain.Interfaces.Services.Application;

namespace ApiServer.Controllers.Application
{
    public class UserRoleController : ApplicationControllerBase<UserRole, IUserRoleService>
    {
        public UserRoleController(IUserRoleService service) : base(service)
        {
        }
    }
}
