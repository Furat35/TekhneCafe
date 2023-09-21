using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using TekhneCafe.Api.Controllers;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.DTOs.Transaction;
using TekhneCafe.Core.Filters.Transaction;

namespace TekhneCafe.Test.ControllerTests
{
    public class TransactionsControllerApiTest
    {
        [Fact]
        public async Task GetUserTransactions_ReturnsOkResult_WithTransactions()
        {
            var userId = "testUserId";
            var list = new List<TransactionHistoryListDto> { };
            var mockTransactionHistoryService = new Mock<ITransactionHistoryService>();
            mockTransactionHistoryService
                .Setup(repo => repo.GetAllTransactionHistoriesByUserIdAsync(It.IsAny<TransactionHistoryRequestFilter>(), userId))
                .ReturnsAsync(list);
            var controller = new TransactionsController(mockTransactionHistoryService.Object);
            var result = await controller.GetUserTransactions(new TransactionHistoryRequestFilter(), userId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var transactions = Assert.IsAssignableFrom<List<TransactionHistoryListDto>>(okResult.Value);
            Assert.Empty(transactions);
        }

        [Fact]
        public async Task Transactions_ReturnsOkResult_WithTransactions()
        {
            var list = new List<TransactionHistoryListDto> { };
            var mockTransactionHistoryService = new Mock<ITransactionHistoryService>();
            mockTransactionHistoryService
                .Setup(repo => repo.GetActiveUsersTransactionHistoriesAsync(It.IsAny<TransactionHistoryRequestFilter>()))
                .ReturnsAsync(list);
            var controller = new TransactionsController(mockTransactionHistoryService.Object);
            var result = await controller.Transactions(new TransactionHistoryRequestFilter());
            var okResult = Assert.IsType<OkObjectResult>(result);
            var transactions = Assert.IsAssignableFrom<List<TransactionHistoryListDto>>(okResult.Value);
            Assert.Empty(transactions);
        }

        [Fact]
        public async Task GetUserTransactions_Returns_OkResult()
        {
            var mockTransactionHistoryService = new Mock<ITransactionHistoryService>();
            var controller = new TransactionsController(mockTransactionHistoryService.Object);

            var filters = new TransactionHistoryRequestFilter();
            var userId = "testUserId";
            var result = await controller.GetUserTransactions(filters, userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Transactions_Returns_OkResult()
        {
            var mockTransactionHistoryService = new Mock<ITransactionHistoryService>();
            var controller = new TransactionsController(mockTransactionHistoryService.Object);
            var filters = new TransactionHistoryRequestFilter();
            var result = await controller.Transactions(filters);
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUserTransactions_Returns_OkResult_ForAuthorizedUser()
        {
            var mockTransactionHistoryService = new Mock<ITransactionHistoryService>();
            var controller = new TransactionsController(mockTransactionHistoryService.Object);
            var filters = new TransactionHistoryRequestFilter();
            var userId = "testUserId";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.Role, RoleConsts.CafeService)
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            var result = await controller.GetUserTransactions(filters, userId);
            var okResult = Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetUserTransactions_Returns_EmptyList_ForNonExistentUser()
        {
            var mockTransactionHistoryService = new Mock<ITransactionHistoryService>();
            mockTransactionHistoryService
                .Setup(repo => repo.GetAllTransactionHistoriesByUserIdAsync(It.IsAny<TransactionHistoryRequestFilter>(), It.IsAny<string>()))
                .ReturnsAsync(new List<TransactionHistoryListDto>());
            var controller = new TransactionsController(mockTransactionHistoryService.Object);
            var filters = new TransactionHistoryRequestFilter();
            var userId = "nonExistentUserId";

            var result = await controller.GetUserTransactions(filters, userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var transactions = Assert.IsAssignableFrom<List<TransactionHistoryListDto>>(okResult.Value);
            Assert.Empty(transactions);
        }
    }
}

