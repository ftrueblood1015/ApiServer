using Domain.Entities.Application;

namespace Domain.Interfaces.Services.Application
{
    public interface IUserService : IServiceBase<User>
    {
        public User? GetByEmail(string email);
    }
}
