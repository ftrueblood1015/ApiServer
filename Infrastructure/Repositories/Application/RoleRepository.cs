using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;

namespace Infrastructure.Repositories.Application
{
    public class RoleRepository : RepositoryBase<Role, ApplicationDbContext>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
