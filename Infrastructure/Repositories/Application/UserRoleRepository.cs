using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Application
{
    public class UserRoleRepository : RepositoryBase<UserRole, ApplicationDbContext>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<UserRole> GetAllExpanded()
        {
            return Context.Set<UserRole>().Include(u => u.User).Include(r => r.Role);
        }
    }
}
