using Domain.Interfaces.Services.Application;
using Services.Application;
using Shouldly;

namespace Service.Tests.Application
{
    public class PasswordHashServiceTests
    {
        private readonly IPasswordHashService PasswordHashService;

        public PasswordHashServiceTests()
        {
            PasswordHashService = new PasswordHashService();
        }

        [Test]
        public void Can_Hash_Plain_Text_Password()
        {
            // Arrange
            var plainTextPassword = "Password123";

            // Act
            var result = PasswordHashService.CreateHash(plainTextPassword);

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void Hash_Should_Not_Contain_Dash()
        {
            // Arrange
            var plainTextPassword = "Password123";

            // Act
            var result = PasswordHashService.CreateHash(plainTextPassword);

            // Assert
            result.ShouldNotContain("-");
        }

        [Test]
        public void Hash_Should_Be_Lower_Case()
        {
            // Arrange
            var plainTextPassword = "Password123";

            // Act
            var result = PasswordHashService.CreateHash(plainTextPassword);

            // Assert
            result.ShouldBe(result.ToLower());
        }

        [Test]
        public void Cannot_Hash_Null_Password()
        {
            // Arrange
            string? plainTextPassword = null;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => PasswordHashService.CreateHash(plainTextPassword));
        }

        [Test]
        public void Cannot_Hash_Empty_Password()
        {
            // Arrange
            string? plainTextPassword = String.Empty;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => PasswordHashService.CreateHash(plainTextPassword));
        }

        [Test]
        public void Passwords_Match()
        {
            // Arrange
            var plainTextPassword = "Password123";
            var hashedPassword = PasswordHashService.CreateHash(plainTextPassword);

            // Act
            var result = PasswordHashService.VerifyPassword(plainTextPassword, hashedPassword);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void Passwords_Do_Not_Match()
        {
            // Arrange
            var correctPassword = "Password123";
            var incorrectPassword = "Password124";
            var hashedPassword = PasswordHashService.CreateHash(correctPassword);

            // Act
            var result = PasswordHashService.VerifyPassword(incorrectPassword, hashedPassword);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
