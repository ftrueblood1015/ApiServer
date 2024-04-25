using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Domain.Interfaces.Services.Application;

namespace Services.Application
{
    public class RoleService : ServiceBase<Role, IRoleRepository>, IRoleService
    {
        public RoleService(IRoleRepository repo) : base(repo)
        {
        }
    }
}
