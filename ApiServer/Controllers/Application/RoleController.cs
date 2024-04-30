using Domain.Entities.Application;
using Domain.Interfaces.Services.Application;

namespace ApiServer.Controllers.Application
{
    public class RoleController : ApplicationControllerBase<Role, IRoleService>
    {
        public RoleController(IRoleService service) : base(service)
        {
        }
    }
}
