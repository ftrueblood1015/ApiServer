using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Domain.Interfaces.Services.Application;

namespace Services.Application
{
    public class UserService : ServiceBase<User, IUserRepository>, IUserService
    {
        private readonly IPasswordHashService _passwordHashService;

        public UserService(IUserRepository repo) : base(repo)
        {
            _passwordHashService = new PasswordHashService();
        }

        public override User Add(User entity)
        {
            try
            {
                var hashedPassword = _passwordHashService.CreateHash(entity.Password!);
                entity.Password = hashedPassword;
                return Repo.Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User? GetByEmail(string email)
        {
            return Filter(x => x.Email == email).FirstOrDefault();
        }
    }
}
