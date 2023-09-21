using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Security.Claims;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Concrete;
using TekhneCafe.Core.DTOs.AppUser;
using TekhneCafe.Core.DTOs.Payment;
using TekhneCafe.Core.Exceptions.AppUser;
using TekhneCafe.DataAccess.Helpers.Transaction;

namespace TekhneCafe.Test.ServiceTests
{
    public class PaymentServiceTest
    {
        private Mock<IWalletService> _walletService = new();
        private Mock<ITransactionManagement> _transactionManagement = new();
        private Mock<ITransactionHistoryService> _transactionHistoryService = new();
        private Mock<IOneSignalNotificationService> _oneSignalNotificationService = new();
        private Mock<IAppUserService> _appUserService = new();
        private Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private Mock<INotificationService> _notificationService = new();
        private Mock<IDbContextTransaction> _dbContextTransaction = new();
        private ClaimsPrincipal _principal;

        public PaymentServiceTest()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };
            var identity = new ClaimsIdentity(claims, "Bearer");
            _principal = new ClaimsPrincipal(identity);
            var mockHttpContext = new DefaultHttpContext();
            mockHttpContext.User = _principal;
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext);
        }

        [Fact]
        public async Task Pay_WithValidCredentials_ReturnsTask()
        {
            // Arrange 
            AppUserListDto user = new() { FullName = "Name", };
            PaymentDto paymentDto = new()
            {
                UserId = Guid.NewGuid().ToString(),
                Amount = 2222,
                Description = "Test"
            };
            _appUserService.Setup(_ => _.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _transactionManagement.Setup(_ => _.BeginTransactionAsync()).ReturnsAsync(_dbContextTransaction.Object);
            PaymentManager paymentManager = new PaymentManager(_walletService.Object, _transactionManagement.Object, _transactionHistoryService.Object, _httpContextAccessor.Object,
                _oneSignalNotificationService.Object, _appUserService.Object, _notificationService.Object);

            // Act
            await paymentManager.PayAsync(paymentDto);

            // Assert
            //No need for assertion
        }


        [Fact]
        public async Task Pay_WithInvalidUserId_ThrowsAppUserNotFoundException()
        {
            // Arrange 
            PaymentDto paymentDto = new()
            {
                UserId = Guid.NewGuid().ToString(),
                Amount = 2222,
                Description = "Test"
            };
            _appUserService.Setup(_ => _.GetUserByIdAsync(It.IsAny<string>())).ThrowsAsync(new AppUserNotFoundException());
            PaymentManager paymentManager = new PaymentManager(_walletService.Object, _transactionManagement.Object, _transactionHistoryService.Object, _httpContextAccessor.Object,
                _oneSignalNotificationService.Object, _appUserService.Object, _notificationService.Object);

            // Act
            var result = async () => await paymentManager.PayAsync(paymentDto);

            //Assert
            await Assert.ThrowsAsync(typeof(AppUserNotFoundException), result);
        }
    }
}
