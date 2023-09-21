using Microsoft.AspNetCore.Mvc;
using Moq;
using TekhneCafe.Api.Controllers;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.DTOs.Authentication;
using TekhneCafe.Core.Exceptions.Authentication;

namespace TekhneCafe.Test.ControllerTests
{
    public class AuthenticationControllerApiTest
    {
        Mock<IAuthenticationService> _authenticationService = new();
        public AuthenticationControllerApiTest()
        {

        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnJwtResponse()
        {
            // Arrange 
            _authenticationService.Setup(_ => _.Login(It.IsAny<UserLoginDto>())).ReturnsAsync(new JwtResponse()
            {
                Token = "validToken",
                User = new()
            });
            AuthenticationController controller = new(_authenticationService.Object);

            // Act
            var result = await controller.Login(new());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithInvalidUsername_ThrowsUserNotFoundException()
        {
            // Arrange 
            _authenticationService.Setup(_ => _.Login(It.IsAny<UserLoginDto>())).ThrowsAsync(new UserNotFoundException());
            AuthenticationController controller = new(_authenticationService.Object);

            // Act
            var result = async () => await controller.Login(new());

            // Assert
            await Assert.ThrowsAsync(typeof(UserNotFoundException), result);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ThrowsUserBadRequestException()
        {
            // Arrange 
            _authenticationService.Setup(_ => _.Login(It.IsAny<UserLoginDto>())).ThrowsAsync(new UserBadRequestException());
            AuthenticationController controller = new(_authenticationService.Object);

            // Act
            var result = async () => await controller.Login(new());

            // Assert
            await Assert.ThrowsAsync(typeof(UserBadRequestException), result);
        }
    }
}
