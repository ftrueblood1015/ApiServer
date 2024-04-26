using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Domain.Interfaces.Services.Application;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Service.Tests.MockBases;
using Services.Application;
using Shouldly;

namespace Service.Tests.Application
{
    public class UserRoleServiceTests : ServiceBaseTests<UserRole>
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleServiceTests()
        {
            var userRoleRepo = MockRepositoryBase.MockRepo<IUserRoleRepository, UserRole>(new List<UserRole>()
            {
                new UserRole() { UserId = new Guid("bf420b8e-dfd4-4339-9492-f39d88c21ce0"), RoleId = Guid.NewGuid() },
                new UserRole() { UserId = new Guid("bf420b8e-dfd4-4339-9492-f39d88c21ce0"), RoleId = Guid.NewGuid() },
                new UserRole() { UserId = new Guid("87ebfef7-9c59-4423-8ac8-c1d1ce78f495"), RoleId = Guid.NewGuid() },
                new UserRole() { UserId = Guid.NewGuid(), RoleId = Guid.NewGuid() },
            });

            _userRoleService = new UserRoleService(userRoleRepo.Object);
        }

        [TestCase("bf420b8e-dfd4-4339-9492-f39d88c21ce0", 2)]
        [TestCase("87ebfef7-9c59-4423-8ac8-c1d1ce78f495", 1)]
        [TestCase("87ebfef7-9c59-4423-8ac8-c2d1ce78f495", 0)]
        public void Can_Get_Roles_By_User_Id(string id, int expectedCount)
        {
            // Act
            var result = _userRoleService.GetAllUserRolesForUser(new Guid(id));

            // Assert
            result.Count().ShouldBe(expectedCount);
        }
    }
}
