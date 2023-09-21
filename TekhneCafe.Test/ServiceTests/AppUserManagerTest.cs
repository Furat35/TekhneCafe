using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration; 
using Moq;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Concrete;
using TekhneCafe.Core.Exceptions.AppUser;
using TekhneCafe.Core.Filters.AppUser;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;
using System;
using System.Linq;
using Xunit;
using System.Security.Claims;
using TekhneCafe.Business.Models;
using TekhneCafe.Core.DTOs.AppUser;

namespace TekhneCafe.Test
{
    public class AppUserManagerTest
    {
        private readonly Mock<IConfiguration> configurationMock;

        public AppUserManagerTest()
        {
            configurationMock = new Mock<IConfiguration>();
        }

        [Fact]
        public void GetUserByIdAsync_Returns_User()
        {
            // Arrange
            var userId = Guid.NewGuid(); 
            var userDto = new AppUserListDto { Id = userId, FullName = "Test User" };
            var appUser = new AppUser { Id = userId, FullName = "Test User" }; 

            var userDalMock = new Mock<IAppUserDal>();
            userDalMock.Setup(dal => dal.GetByIdAsync(userId)).ReturnsAsync(appUser);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<AppUserListDto>(appUser)).Returns(userDto);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var configurationMock = new Mock<IConfiguration>();
            var imageServiceMock = new Mock<IImageService>();

            var userManager = new AppUserManager(
                userDalMock.Object,
                mapperMock.Object,
                httpContextAccessorMock.Object,
                imageServiceMock.Object,
                configurationMock.Object
            );

            // Act
            var result = userManager.GetUserByIdAsync(userId.ToString()).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Id, result.Id);
            Assert.Equal(userDto.FullName, result.FullName);
        }

      
       
        [Fact]
        public void GetUserByIdAsync_Throws_AppUserNotFoundException_When_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var userDalMock = new Mock<IAppUserDal>();
            userDalMock.Setup(dal => dal.GetByIdAsync(Guid.Parse(userId))).ReturnsAsync((AppUser)null);

            var mapperMock = new Mock<IMapper>();

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var configurationMock = new Mock<IConfiguration>();
            var imageServiceMock = new Mock<IImageService>();

            var userManager = new AppUserManager(
                userDalMock.Object,
                mapperMock.Object,
                httpContextAccessorMock.Object,
                imageServiceMock.Object,
                configurationMock.Object
            );

            // Act & Assert
            Assert.ThrowsAsync<AppUserNotFoundException>(() => userManager.GetUserByIdAsync(userId));
        }
    }
}
