using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;

namespace Infrastructure.Repositories.Application
{
    public class UserRepository : RepositoryBase<User, ApplicationDbContext>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
