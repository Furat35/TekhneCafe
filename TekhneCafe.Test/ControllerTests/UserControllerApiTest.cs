using Microsoft.AspNetCore.Mvc;
using Moq;
using TekhneCafe.Api.Controllers;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.DTOs.AppUser;
using TekhneCafe.Core.DTOs.Authentication;
using TekhneCafe.Core.Exceptions.AppUser;
using TekhneCafe.Core.Exceptions.Authentication;
using TekhneCafe.Core.Filters.AppUser;


namespace TekhneCafe.Test.ControllerTests
{
    public class UserControllerApiTest
    {

        [Fact]
        public void GetUsers_ReturnsOkResult_WithUserList()
        {
            var userServiceMock = new Mock<IAppUserService>();
            var filters = new AppUserRequestFilter();

            var users = new List<AppUserListDto>();
            userServiceMock.Setup(service => service.GetUserList(filters)).Returns(users);

            var controller = new UsersController(userServiceMock.Object);

            var result = controller.GetUsers(filters);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WithUser()
        {
            var userServiceMock = new Mock<IAppUserService>();
            var userId = "someUserId";

            var user = new AppUserListDto();
            userServiceMock.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var controller = new UsersController(userServiceMock.Object);

            var result = await controller.GetUsers(userId);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(user, okResult.Value);

        }
        [Fact]
        public void GetUsers_ReturnsInternalServerError_OnServiceError()
        {
            var userServiceMock = new Mock<IAppUserService>();
            var filters = new AppUserRequestFilter();

            userServiceMock.Setup(service => service.GetUserList(filters)).Throws(new Exception("Service error"));

            var controller = new UsersController(userServiceMock.Object);
            Assert.Throws<Exception>(() => controller.GetUsers(filters));
        }

        [Fact]
        public async Task GetUserById_Returns_BadRequestResult_When_ServiceThrowsException()
        {
            var mockUserService = new Mock<IAppUserService>();
            var userId = Guid.NewGuid();
            mockUserService.Setup(service => service.GetUserByIdAsync(userId.ToString())).ThrowsAsync(new AppUserNotFoundException());

            var controller = new UsersController(mockUserService.Object);
            var result = async delegate ()
            {
                await controller.GetUsers(userId.ToString());
            };
            await Assert.ThrowsAsync(typeof(AppUserNotFoundException), result);
        }

        [Fact]
        public void GetUsers_Returns_OkResult_With_UserList()
        {
            var mockUserService = new Mock<IAppUserService>();
            var users = new List<AppUserListDto>
            {
                new AppUserListDto { Id = Guid.NewGuid(), FullName = "User 1" },
                new AppUserListDto { Id = Guid.NewGuid(), FullName = "User 2" }
            };
            mockUserService.Setup(service => service.GetUserList(It.IsAny<AppUserRequestFilter>())).Returns(users);

            var controller = new UsersController(mockUserService.Object);

            var result = controller.GetUsers(new AppUserRequestFilter());

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AppUserListDto>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }
        [Fact]
        public async Task GetUserById_Returns_User()
        {
            var mockUserService = new Mock<IAppUserService>();
            var userId = Guid.NewGuid();
            var userDto = new AppUserListDto { Id = userId, FullName = "User" };
            mockUserService.Setup(service => service.GetUserByIdAsync(userId.ToString())).ReturnsAsync(userDto);

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.GetUsers(userId.ToString());

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<AppUserListDto>(okResult.Value);
            Assert.Equal(userId, model.Id);
        }
        [Fact]
        public async Task Login_Returns_BadRequestResult_When_InvalidUserCredentials()
        {
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            var invalidUserDto = new UserLoginDto { Username = "invalid", Password = "invalidpass" };
            mockAuthenticationService.Setup(service => service.Login(invalidUserDto)).ThrowsAsync(new UserBadRequestException());

            var controller = new AuthenticationController(mockAuthenticationService.Object);

            await Assert.ThrowsAsync<UserBadRequestException>(() => controller.Login(invalidUserDto));

        }

        [Fact]
        public async Task Login_UsernameOrPasswordTooLong_ReturnsBadRequest()
        {
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            var controller = new AuthenticationController(mockAuthenticationService.Object);

            var excessivelyLongString = new string('a', 301);
            var userLoginDto = new UserLoginDto
            {
                Username = excessivelyLongString,
                Password = excessivelyLongString
            };

            mockAuthenticationService.Setup(service => service.Login(userLoginDto))
                                      .ThrowsAsync(new UserBadRequestException("Geçersiz kullanıcı bilgileri!"));
            var result = async delegate ()
            {
                var result = await controller.Login(userLoginDto);
            };
            await Assert.ThrowsAsync(typeof(UserBadRequestException), result);

        }
        [Fact]
        public async Task Login_ReturnsJwtResponse_ForValidUserCredentials()
        {
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            var validUserDto = new UserLoginDto { Username = "valid-username", Password = "valid-password" };

            var jwtResponse = new JwtResponse { Token = "valid-token", User = new UserResponse() };
            mockAuthenticationService.Setup(service => service.Login(validUserDto)).ReturnsAsync(jwtResponse);

            var controller = new AuthenticationController(mockAuthenticationService.Object);

            var result = await controller.Login(validUserDto);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var response = okResult.Value as JwtResponse;
            Assert.NotNull(response);
            Assert.Equal(jwtResponse.Token, response.Token);
        }
    }
}


