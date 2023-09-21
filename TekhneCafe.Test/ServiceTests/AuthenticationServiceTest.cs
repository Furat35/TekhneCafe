using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Concrete;
using TekhneCafe.Core.DTOs.Authentication;
using TekhneCafe.Core.Exceptions.Authentication;

namespace TekhneCafe.Test.ServiceTests
{
    public class AuthenticationServiceTest
    {
        private Mock<IConfiguration> _configuration = new();
        private Mock<ITokenService> _tokenService = new();
        private Mock<IAppUserService> _appUserService = new();
        private readonly string validUsername = "ad.soyad";
        private readonly string validUserPassword = "Tkl2023.";
        public AuthenticationServiceTest()
        {
            _configuration.Setup(_ => _.GetSection("LdapSettings:Username").Value).Returns("ldapconnect.user");
            _configuration.Setup(_ => _.GetSection("LdapSettings:Password").Value).Returns("Tekhne2023*");
            _configuration.Setup(_ => _.GetSection("LdapSettings:Path").Value).Returns("LDAP://10.0.0.201:389");
            _tokenService.Setup(_ => _.GenerateToken(It.IsAny<List<Claim>>())).Returns(new JwtSecurityToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDYWZlQWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRsSxcmF0IE9ydGHDpyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiOGJmMzc2ZjctNjUzOS00ZWRjLTE5Y2QtMDhkYmEzMDJjYWM3IiwiZXhwIjoxNjk0MTYyNjA5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDEzIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAxMyJ9.bJT5JWhI9-WFz5NiFDYkwMCuuOSQmEUYFBHj8TElG1Y"));
            _appUserService.Setup(_ => _.GetUserByLdapIdAsync(It.IsAny<string>())).ReturnsAsync(new Entity.Concrete.AppUser()
            {
                Id = Guid.NewGuid(),
                Department = "Department",
                Email = "Email",
                FullName = "FullName",
                Username = "Username",
                Wallet = 243,
                ImagePath = "ImagePath"
            });
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsJwtResponse()
        {
            // Arrange
            LdapAuthenticationManager authenticationManager = new(_configuration.Object, _tokenService.Object, _appUserService.Object);

            // Act
            var response = await authenticationManager.Login(new() { Username = validUsername, Password = validUserPassword });

            // Assert
            Assert.IsType<JwtResponse>(response);
        }

        [Fact]
        public async Task Login_WithInvalidUsername_ThrowsUserNotFoundException()
        {
            // Arrange
            LdapAuthenticationManager authenticationManager = new(_configuration.Object, _tokenService.Object, _appUserService.Object);

            // Act
            var response = async () => await authenticationManager.Login(new() { Username = "invaliUser", Password = validUserPassword });

            // Assert
            await Assert.ThrowsAsync(typeof(UserNotFoundException), response);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ThrowsUserBadRequestException()
        {
            // Arrange
            LdapAuthenticationManager authenticationManager = new(_configuration.Object, _tokenService.Object, _appUserService.Object);

            // Act
            var response = async () => await authenticationManager.Login(new() { Username = validUsername, Password = "invalidPassword" });

            // Assert
            await Assert.ThrowsAsync(typeof(UserBadRequestException), response);
        }

        [Fact]
        public async Task Login_WithInvalidLdapConfigurations_ThrowsUserBadRequestException()
        {
            // Arrange
            _configuration.Setup(_ => _.GetSection("LdapSettings:Username").Value).Returns("invalidUser");
            _configuration.Setup(_ => _.GetSection("LdapSettings:Password").Value).Returns("invalidPassword");
            _configuration.Setup(_ => _.GetSection("LdapSettings:Path").Value).Returns("invalidPath");
            LdapAuthenticationManager authenticationManager = new(_configuration.Object, _tokenService.Object, _appUserService.Object);

            // Act
            var response = async () => await authenticationManager.Login(new() { Username = validUsername, Password = validUserPassword });

            // Assert
            await Assert.ThrowsAsync(typeof(UserBadRequestException), response);
        }
    }
}
