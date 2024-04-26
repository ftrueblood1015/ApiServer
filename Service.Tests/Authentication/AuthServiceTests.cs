using Domain.Configuration;
using Domain.Entities.Application;
using Domain.Interfaces.Repositories.Application;
using Domain.Interfaces.Services.Application;
using Domain.Interfaces.Services.Authorization;
using Domain.Models;
using Service.Tests.MockBases;
using Services.Application;
using Services.Authentication;
using Shouldly;

namespace Service.Tests.Authentication
{
    public class AuthServiceTests
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            _passwordHashService = new PasswordHashService();

            var configOptions = new ConfigOptions();

            var ip = "http://localhost:7081";
            var key = "9842476D7A8E8048026F4F5ECB900BF6A1076F57";
            _jwtService = new JwtService(new ConfigOptions() { Token = new Token() { Key = key, Issuer = ip, Audience = ip, TokenAccessExpiry = "1440" } });

            var userRepo = MockRepositoryBase.MockRepo<IUserRepository, User>(new List<User>() {
                new User() {
                    Id = new Guid("37160c9d-e647-42ed-b9d5-2a33dd080f5c"),
                    Password = "008c70392e3abfbd0fa47bbc2ed96aa99bd49e159727fcba0f2e6abeb3a9d601",
                    Email = "Test@gmail.com",
                    PhoneNumber = "5597218521",
                    DisplayName = "Tests"
                }
            });

            var roleRepo = MockRepositoryBase.MockRepo<IRoleRepository, Role>(new List<Role>() {
                new Role() { Id = new Guid("12ba829c-d9a6-4271-9bf2-904227345587"), Name = "SuperUser", Description = "SUPERUSER!"}
            });
            var userRoleRepo = MockRepositoryBase.MockRepo<IUserRoleRepository, UserRole>(new List<UserRole>() {
                new UserRole() {
                    Id = new Guid("e3a92377-815a-4c57-baf5-203ea49e5922"),
                    UserId = new Guid("37160c9d-e647-42ed-b9d5-2a33dd080f5c"),
                    RoleId = new Guid("12ba829c-d9a6-4271-9bf2-904227345587"),
                    Role = new Role() { Id = new Guid("12ba829c-d9a6-4271-9bf2-904227345587"), Name = "SuperUser", Description = "SUPERUSER!"},
                    User = new User() {
                        Id = new Guid("37160c9d-e647-42ed-b9d5-2a33dd080f5c"),
                        Password = "008c70392e3abfbd0fa47bbc2ed96aa99bd49e159727fcba0f2e6abeb3a9d601",
                        Email = "Test@gmail.com",
                        PhoneNumber = "5597218521",
                        DisplayName = "Tests"
                    }
                }
            });

            _userService = new UserService(userRepo.Object);
            _roleService = new RoleService(roleRepo.Object);
            _userRoleService = new UserRoleService(userRoleRepo.Object);

            _authService = new AuthService(_userService, _userRoleService, _jwtService, _passwordHashService);
        }

        [Test]
        public void Can_Authenticate_User_Login_Successfully()
        {
            // Arrange
            UserLogin credentials = new UserLogin() { Username = "Test@gmail.com", Password = "Password123" };
            var remoteIp = "127.0.0.1";

            // Act
            var result = _authService.AuthenticateUser(credentials, remoteIp);

            // Assert
            result.AccessToken.ShouldNotBeNull();

            result.Username.ShouldBe("Test@gmail.com");

            result.Roles.ShouldNotBeNull();
            result.Roles!.Count().ShouldBe(1);
            result.Roles!.First().ShouldBe("SuperUser");

            result.ApiMessage.ShouldBe("Successfully Authenticated");
        }

        [Test]
        public void Cannot_Authenticate_With_Null_UserLogin()
        {
            // Arrange
            UserLogin? credentials = null;
            var remoteIp = "127.0.0.1";

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("User Login Cannot Be Null");
        }

        [Test]
        public void Cannot_Authenticate_With_Null_Email()
        {
            // Arrange
            UserLogin? credentials = new UserLogin() { Username = null, Password = "Password123" };
            var remoteIp = "127.0.0.1";

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("Username and Password Cannot Be Blank");
        }

        [Test]
        public void Cannot_Authenticate_With_Null_Password()
        {
            // Arrange
            UserLogin? credentials = new UserLogin() { Username = "Test@gmail.com", Password = null };
            var remoteIp = "127.0.0.1";

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("Username and Password Cannot Be Blank");
        }

        [Test]
        public void Cannot_Authenticate_With_Empty_Password()
        {
            // Arrange
            UserLogin? credentials = new UserLogin() { Username = "Test@gmail.com", Password = string.Empty };
            var remoteIp = "127.0.0.1";

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("Username and Password Cannot Be Blank");
        }

        [Test]
        public void Cannot_Authenticate_With_Empty_Email()
        {
            // Arrange
            UserLogin? credentials = new UserLogin() { Username = string.Empty, Password = "Password123" };
            var remoteIp = "127.0.0.1";

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("Username and Password Cannot Be Blank");
        }

        [Test]
        public void Cannot_Authenticate_With_Empty_RemoteIp()
        {
            // Arrange
            UserLogin? credentials = new UserLogin() { Username = "Test@gmail.com", Password = "Password123" };
            var remoteIp = string.Empty;

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("Must Supply Ip");
        }

        [Test]
        public void Cannot_Authenticate_With_Null_RemoteIp()
        {
            // Arrange
            UserLogin? credentials = new UserLogin() { Username = "Test@gmail.com", Password = "Password123" };
            string? remoteIp = null;

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("Must Supply Ip");
        }

        [Test]
        public void Cannot_Authenticate_With_Invalid_Email()
        {
            // Arrange
            UserLogin? credentials = new UserLogin() { Username = "INVALID", Password = "Password123" };
            string? remoteIp = "10.0.0.127";

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("Incorrect Email Or Password");
        }

        [Test]
        public void Cannot_Authenticate_With_Invalid_Password()
        {
            // Arrange
            UserLogin? credentials = new UserLogin() { Username = "Test@gmail.com", Password = "Invalid" };
            string? remoteIp = "10.0.0.127";

            // Act
            var result = _authService.AuthenticateUser(credentials!, remoteIp);

            // Assert
            result.ApiMessage.ShouldBe("Incorrect Email Or Password");
        }

        [Test]
        public void GetUserByEmail_Should_Return_User()
        {
            // Arrange
            var email = "Test@gmail.com";

            // Act
            var result = _authService.GetUserByEmail(email);

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetUserByEmail_Should_Not_Return_User()
        {
            // Arrange
            var email = "INVALID@gmail.com";

            // Act
            var result = _authService.GetUserByEmail(email);

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void GetUserRoles_Should_Return_Non_Empty_List()
        {
            // Arrange
            var guid = new Guid("37160c9d-e647-42ed-b9d5-2a33dd080f5c");

            // Act
            var result = _authService.GetUserRoles(guid);

            // Assert
            result.Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public void GetUserRoles_Should_Return_Empty_List()
        {
            // Arrange
            var guid = new Guid("49160c9d-e647-42ed-b9d5-2a33dd080f5c");

            // Act
            var result = _authService.GetUserRoles(guid);

            // Assert
            result.Count().ShouldBe(0);
        }

        [Test]
        public void GetAccessToken_Should_Not_Be_Null()
        {
            // Arrange
            var email = "Test@gmail.com";
            var ip = "127.0.0.1";
            var roles = _roleService.GetAll().ToList();

            // Act
            var result = _authService.GetAccessToken(email, ip, roles);

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void AuthenticatePassword_Should_Be_True()
        {
            // Arrange
            var suppliedPassword = "Password123";
            var storedPassword = "008c70392e3abfbd0fa47bbc2ed96aa99bd49e159727fcba0f2e6abeb3a9d601";

            // Act
            var result = _authService.AuthenticatePassword(suppliedPassword, storedPassword);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void AuthenticatePassword_Should_Be_False()
        {
            // Arrange
            var suppliedPassword = "Invalid";
            var storedPassword = "008c70392e3abfbd0fa47bbc2ed96aa99bd49e159727fcba0f2e6abeb3a9d601";

            // Act
            var result = _authService.AuthenticatePassword(suppliedPassword, storedPassword);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
