using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Domain.Interfaces.Services.Application;

namespace Services.Application
{
    public class UserService : ServiceBase<User, IUserRepository>, IUserService
    {
        public UserService(IUserRepository repo) : base(repo)
        {
        }
    }
}
