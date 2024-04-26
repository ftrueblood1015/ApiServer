using Domain.Configuration;
using Domain.Interfaces.Services.Authorization;
using Services.Authentication;
using Shouldly;
using System.Security.Claims;

namespace Service.Tests.Authentication
{
    public class JwtServiceTests
    {
        private readonly IJwtService _jwtService;

        public JwtServiceTests()
        {
            var ip = "http://localhost:7081";
            var key = "9842476D7A8E8048026F4F5ECB900BF6A1076F57";
            _jwtService = new JwtService(new ConfigOptions() { Token = new Token() { Key = key, Issuer = ip, Audience = ip, TokenAccessExpiry = "1440" } });
        }

        [Test]
        public void Can_Create_Token()
        {
            // Arrange
            var claim = new Claim("roles","SuperUser");
            var token = _jwtService.CreateEncodedJwtToken("Test@gmail.com", "10.0.0.127", DateTime.Now, new List<Claim>() { claim });

            token.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Can_Create_and_Decode_Email()
        {
            var claim = new Claim("roles","SuperUser");
            var token = _jwtService.CreateEncodedJwtToken("Test@gmail.com", "10.0.0.127", DateTime.Now, new List<Claim>() { claim });

            var email = _jwtService.DecodeEmailFromJwtToken(token);

            email.ShouldBe("Test@gmail.com");
        }

        [Test]
        public void Can_Create_and_Decode_Role()
        {
            var claim = new Claim("roles","SuperUser");
            var token = _jwtService.CreateEncodedJwtToken("Test@gmail.com", "10.0.0.127", DateTime.Now, new List<Claim>() { claim });

            var roles = _jwtService.DecodeRolesFromJwtToken(token);

            roles.Count().ShouldBe(1);
            roles.First().ShouldBe("SuperUser");
        }

        [Test]
        public void Can_Create_and_Decode_Multiple_Roles()
        {
            var claim1 = new Claim("roles","SuperUser");
            var claim2 = new Claim("roles","Admin");
            var token = _jwtService.CreateEncodedJwtToken("Test@gmail.com", "10.0.0.127", DateTime.Now, new List<Claim>() { claim1, claim2 });

            var roles = _jwtService.DecodeRolesFromJwtToken(token);

            roles.Count().ShouldBe(2);
            roles.First().ShouldBe("SuperUser");
            roles.Last().ShouldBe("Admin");
        }
    }
}
