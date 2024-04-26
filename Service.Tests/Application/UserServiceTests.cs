using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Domain.Interfaces.Services.Application;
using Service.Tests.MockBases;
using Services.Application;
using Shouldly;

namespace Service.Tests.Application
{
    public class UserServiceTests : ServiceBaseTests<User>
    {
        private readonly IUserService _userService;
        private readonly IPasswordHashService _passwordHashService;

        public UserServiceTests()
        {
            var userRepo = MockRepositoryBase.MockRepo<IUserRepository, User>(new List<User>() { });

            _userService = new UserService(userRepo.Object);

            _passwordHashService = new PasswordHashService();
        }

        [Test]
        public void Can_Add_New_User_With_Hashed_Password()
        {
            // Arrange
            var password = "Password";
            var user = new User { Id = Guid.NewGuid(), Email = "Add@gmail.com", Password = password, PhoneNumber = "55912345678"};

            //Act
            var result = _userService.Add(user);

            // Assert
            result.Password.ShouldNotBe(password);
            result.Password.ShouldBe(_passwordHashService.CreateHash(password));
            _passwordHashService.VerifyPassword(password, result.Password!).ShouldBeTrue();
        }

        [Test]
        public void GetByEmail_Should_Return_User()
        {
            // Arrange
            var email = "New@gmail.com";
            var password = "Password";
            var user = new User { Id = Guid.NewGuid(), Email = email, Password = password, PhoneNumber = "55912345678"};
            var add = _userService.Add(user);

            // Act
            var result = _userService.GetByEmail(email);

            // Assert
            result.ShouldNotBeNull();
            result.Email.ShouldNotBeNull();
            result.Email.ShouldBe(email);
        }

        [Test]
        public void GetByEmail_Should_Not_Return_User()
        {
            // Arrange
            var email = "Old@gmail.com";

            // Act
            var result = _userService.GetByEmail(email);

            // Assert
            result.ShouldBeNull();
        }
    }
}
