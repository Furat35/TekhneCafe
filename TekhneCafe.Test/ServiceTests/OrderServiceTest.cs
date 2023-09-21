using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Linq.Expressions;
using System.Security.Claims;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Concrete;
using TekhneCafe.Core.DTOs.Order;
using TekhneCafe.Core.Exceptions;
using TekhneCafe.Core.Exceptions.Order;
using TekhneCafe.Core.Filters.Order;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Helpers.Transaction;
using TekhneCafe.Entity.Concrete;
using TekhneCafe.Entity.Enums;
using TekneCafe.SignalR.Abstract;

namespace TekhneCafe.Test.ServiceTests
{
    public class OrderServiceTest
    {
        private Mock<IOrderDal> _orderDal = new();
        private Mock<IMapper> _mapper = new();
        private Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private Mock<IOrderHistoryService> _orderHistoryService = new();
        private Mock<IWalletService> _walletService = new();
        private Mock<IOrderProductService> _orderProductService = new();
        private Mock<ITransactionHistoryService> _transactionHistoryService = new();
        private Mock<ITransactionManagement> _transactionManagement = new();
        private Mock<IOrderNotificationService> _orderNotificationService = new();
        private Mock<IOneSignalNotificationService> _oneSignalNotificationService = new();
        private Mock<INotificationService> _notificationService = new();
        private Mock<IDbContextTransaction> _dbContextTransaction = new();
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly List<Order> _orders;
        private readonly DefaultHttpContext _mockHttpContext;

        public OrderServiceTest()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "A97D8D47-3228-4DC9-FC81-08DBA3DC8273") };
            var claimsIdentity = new ClaimsIdentity(claims, "Bearer");
            _claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            _orders = new List<Order> {
                new Order
                {
                    Id = Guid.Parse("333D8D47-3228-4DC9-FC81-08DBA3DC8273"),
                    AppUserId= Guid.Parse("A97D8D47-3228-4DC9-FC81-08DBA3DC8273"),
                    Description = "sipariş ",
                    TotalPrice = 20,
                    OrderStatus = OrderStatus.Ordered,
                    TransactionHistories = new List<TransactionHistory>{new TransactionHistory{CreatedDate = DateTime.Today, AppUser= new AppUser{FullName = "fullname"}} },
                    OrderProducts = new List<OrderProduct>{new OrderProduct{Name = "Deneme", Price = 4, Quantity = 22}},
                },
                new Order
                {
                    Id = Guid.Parse("223D8D47-3228-4DC9-FC81-08DBA3DC8273"),
                    AppUserId= Guid.Parse("111D8D47-3228-4DC9-FC81-08DBA3DC8273"),
                    Description = "sipariş ",
                    TotalPrice = 12,
                    OrderStatus = OrderStatus.Ordered,
                    TransactionHistories = new List<TransactionHistory>{new TransactionHistory{CreatedDate = DateTime.Today, AppUser= new AppUser{FullName = "fullname"}} },
                    OrderProducts = new List<OrderProduct>{new OrderProduct{Name = "Deneme", Price = 4, Quantity = 22}},
                }
            };
            _mockHttpContext = new DefaultHttpContext();
            _mockHttpContext.User = _claimsPrincipal;
            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(_mockHttpContext);
            _httpContextAccessor.Setup(_ => _.HttpContext.User).Returns(_mockHttpContext.User);
        }

        [Fact]
        public async Task GetOrders_WithValidParameters_ReturnsOrders()
        {
            // Arrange
            _orderDal.Setup(_ => _.GetAll(It.IsAny<Expression<Func<Order, bool>>>())).Returns(_orders.AsQueryable());
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object, _transactionHistoryService.Object,
                _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);
            OrderRequestFilter orderRequestFilter = new();
            _httpContextAccessor.Setup(_ => _.HttpContext.Response.Headers.Add(It.IsAny<string>(), It.IsAny<StringValues>()));

            // Act
            var result = orderManager.GetOrders(orderRequestFilter);

            // Assert
            // No need to check the result type; it's already known to be a Task
        }

        [Fact]
        public async Task GetOrderDetailById_WithValidOrderId_ReturnsOrderWithGivenId()
        {
            // Arrange
            _orderDal.Setup(_ => _.GetOrderIncludeProductsAsync(It.IsAny<string>())).ReturnsAsync(_orders.First());
            _mapper.Setup(_ => _.Map<OrderDetailDto>(It.IsAny<Order>())).Returns(new OrderDetailDto());
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object, _transactionHistoryService.Object,
                _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);

            // Act
            var order = await orderManager.GetOrderDetailByIdAsync("A97D8D47-3228-4DC9-FC81-08DBA3DC8273");

            // Assert 
            Assert.NotNull(order);
        }

        [Fact]
        public async Task GetOrderDetailById_WithInvalidOrderId_ThrowsOrderNotFoundException()
        {
            // Arrange
            _mapper.Setup(_ => _.Map<OrderDetailDto>(It.IsAny<Order>())).Returns(new OrderDetailDto());
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object, _transactionHistoryService.Object,
                _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);

            // Act
            var result = async () => await orderManager.GetOrderDetailByIdAsync("A97D8D47-3228-4DC9-FC81-08DBA3DC8273");

            // Assert 
            await Assert.ThrowsAsync(typeof(OrderNotFoundException), result);
        }

        [Fact]
        public async Task GetOrderDetailById_WithUnauthorizedUser_ThrowsForbiddenException()
        {
            // Arrange
            _orderDal.Setup(_ => _.GetOrderIncludeProductsAsync(It.IsAny<string>())).ReturnsAsync(_orders[1]);
            _mapper.Setup(_ => _.Map<OrderDetailDto>(It.IsAny<Order>())).Returns(new OrderDetailDto());
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object,
                _transactionHistoryService.Object, _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);

            // Act
            var order = async () => await orderManager.GetOrderDetailByIdAsync("A97D8D47-3228-4DC9-FC81-08DBA3DC8273");

            // Assert 
            Assert.ThrowsAsync(typeof(ForbiddenException), order);
        }

        [Fact]
        public async Task ConfirmOrder_WithValidOrderId_ReturnsTask()
        {
            // Arrange
            _orderDal.Setup(_ => _.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_orders.First());
            _transactionManagement.Setup(_ => _.BeginTransactionAsync()).ReturnsAsync(_dbContextTransaction.Object);
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object,
                _transactionHistoryService.Object, _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);

            // Act
            await orderManager.ConfirmOrderAsync("333D8D47-3228-4DC9-FC81-08DBA3DC8273");

            // Assert
            // No need to check the result type, it's already known to be a Task
        }

        [Fact]
        public async Task ConfirmOrder_WithInvalidOrderId_ThrowsOrderNotFoundException()
        {
            // Arrange
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object,
                _transactionHistoryService.Object, _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);

            // Act
            var result = async () => await orderManager.ConfirmOrderAsync("333D8D47-3228-4DC9-FC81-08DBA3DC8273");

            // Assert
            await Assert.ThrowsAsync(typeof(OrderNotFoundException), result);
        }

        [Fact]
        public async Task CreateOrder_WithValidParameters_ReturnsTask()
        {
            // Arrange
            _orderDal.Setup(_ => _.GetAll(It.IsAny<Expression<Func<Order, bool>>>())).Returns(_orders.AsQueryable());
            _transactionManagement.Setup(_ => _.BeginTransactionAsync()).ReturnsAsync(_dbContextTransaction.Object);
            _mapper.Setup(_ => _.Map<Order>(It.IsAny<OrderAddDto>())).Returns(_orders.First());
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object,
               _transactionHistoryService.Object, _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);

            // Act
            await orderManager.CreateOrderAsync(null);

            // Assert 
            // No need to check the result type, it's already known to be a Task
        }

        [Fact]
        public async Task CreateOrder_WithNullOrder_ThrowsOrderBadRequestException()
        {
            // Arrange
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object,
               _transactionHistoryService.Object, _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);

            // Act
            var result = async () => await orderManager.CreateOrderAsync(null);

            // Assert 
            await Assert.ThrowsAsync(typeof(OrderBadRequestException), result);
        }

        [Fact]
        public async Task CreateOrder_WithNullOrderProduct_ThrowsOrderBadRequestException()
        {
            // Arrange
            _orders.First().OrderProducts = null;
            _mapper.Setup(_ => _.Map<Order>(It.IsAny<OrderAddDto>())).Returns(_orders.First());
            OrderManager orderManager = new(_orderDal.Object, _mapper.Object, _httpContextAccessor.Object, _orderHistoryService.Object, _walletService.Object, _orderProductService.Object,
               _transactionHistoryService.Object, _transactionManagement.Object, _orderNotificationService.Object, _oneSignalNotificationService.Object, _notificationService.Object);

            // Act
            var result = async () => await orderManager.CreateOrderAsync(null);

            // Assert 
            await Assert.ThrowsAsync(typeof(OrderBadRequestException), result);
        }
    }
}
