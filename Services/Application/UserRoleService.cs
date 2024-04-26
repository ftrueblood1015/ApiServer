using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Domain.Interfaces.Services.Application;
using System;

namespace Services.Application
{
    public class UserRoleService : ServiceBase<UserRole, IUserRoleRepository>, IUserRoleService
    {
        public UserRoleService(IUserRoleRepository repo) : base(repo)
        {
        }

        public IEnumerable<UserRole> GetAllUserRolesForUser(Guid Id)
        {
            return GetAllExpanded().Where(x => x.UserId == Id);
        }
    }
}
