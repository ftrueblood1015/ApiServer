using Domain.Entities.Application;

namespace Domain.Interfaces.Services.Application
{
    public interface IUserRoleService : IServiceBase<UserRole>
    {
        IEnumerable<UserRole> GetAllUserRolesForUser(Guid Id);
    }
}
