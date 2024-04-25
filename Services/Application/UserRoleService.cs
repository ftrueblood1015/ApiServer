using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Domain.Interfaces.Services.Application;

namespace Services.Application
{
    public class UserRoleService : ServiceBase<UserRole, IUserRoleRepository>, IUserRoleService
    {
        public UserRoleService(IUserRoleRepository repo) : base(repo)
        {
        }
    }
}
