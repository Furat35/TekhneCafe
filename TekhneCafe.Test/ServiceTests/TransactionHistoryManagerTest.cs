using Microsoft.AspNetCore.Http;
using Moq;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Concrete;
using TekhneCafe.Core.Filters.Transaction;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;
using TekhneCafe.Entity.Enums;

namespace TekhneCafe.Test.ServiceTests
{
    public class TransactionHistoryManagerTest
    {
        [Fact]
        public void SetTransactionHistoryForOrder_Should_Set_TransactionHistory()
        {
            // Arrange
            var order = new Order();
            var amount = 100.0f;
            var description = "Test transaction";
            var userId = Guid.NewGuid();

            var transactionHistoryDalMock = new Mock<ITransactionHistoryDal>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var appUserServiceMock = new Mock<IAppUserService>();

            var manager = new TransactionHistoryManager(transactionHistoryDalMock.Object, httpContextAccessorMock.Object, appUserServiceMock.Object);

            // Act
            manager.SetTransactionHistoryForOrder(order, amount, description, userId);

            // Assert
            Assert.NotNull(order.TransactionHistories);
            Assert.Single(order.TransactionHistories);
            Assert.Equal(amount, order.TransactionHistories.First().Amount);
            Assert.Equal(description, order.TransactionHistories.First().Description);
            Assert.Equal(userId, order.TransactionHistories.First().AppUserId);
        }
        [Fact]
        public async Task CreateTransactionHistoryAsync_Should_Add_TransactionHistory()
        {
            // Arrange
            var amount = 100.0f;
            var transactionType = TransactionType.Order;
            var description = "Test transaction";
            var userId = Guid.NewGuid();

            var transactionHistoryDalMock = new Mock<ITransactionHistoryDal>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var appUserServiceMock = new Mock<IAppUserService>();

            var manager = new TransactionHistoryManager(transactionHistoryDalMock.Object, httpContextAccessorMock.Object, appUserServiceMock.Object);

            // Act
            await manager.CreateTransactionHistoryAsync(amount, transactionType, description, userId);

            // Assert
            transactionHistoryDalMock.Verify(repo => repo.AddAsync(It.IsAny<TransactionHistory>()), Times.Once);
        }

    }
}
