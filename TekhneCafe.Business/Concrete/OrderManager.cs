using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Extensions;
using TekhneCafe.Business.Helpers.FilterServices;
using TekhneCafe.Business.Helpers.HeaderServices;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.DTOs.Order;
using TekhneCafe.Core.Exceptions;
using TekhneCafe.Core.Exceptions.Order;
using TekhneCafe.Core.Extensions;
using TekhneCafe.Core.Filters.Order;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Helpers.Transaction;
using TekhneCafe.Entity.Concrete;
using TekhneCafe.Entity.Enums;
using TekneCafe.SignalR.Abstract;

namespace TekhneCafe.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderHistoryService _orderHistoryService;
        private readonly IWalletService _walletService;
        private readonly IOrderProductService _orderProductService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly ITransactionManagement _transactionManagement;
        private readonly IOrderNotificationService _orderNotificationService;
        private readonly IOneSignalNotificationService _oneSignalNotificationService;
        private readonly INotificationService _notificationService;
        private readonly HttpContext _httpContext;

        public OrderManager(IOrderDal orderDal, IMapper mapper, IHttpContextAccessor httpContextAccessor, IOrderHistoryService orderHistoryService,
            IWalletService walletService, IOrderProductService orderProductService, ITransactionHistoryService transactionHistoryService,
            ITransactionManagement transactionManagement, IOrderNotificationService orderNotificationService, IOneSignalNotificationService oneSignalNotificationService,
            INotificationService notificationService)
        {
            _orderDal = orderDal;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = httpContextAccessor.HttpContext;
            _orderHistoryService = orderHistoryService;
            _walletService = walletService;
            _orderProductService = orderProductService;
            _transactionHistoryService = transactionHistoryService;
            _transactionManagement = transactionManagement;
            _orderNotificationService = orderNotificationService;
            _oneSignalNotificationService = oneSignalNotificationService;
            _notificationService = notificationService;
        }

        public async Task CreateOrderAsync(OrderAddDto orderAddDto)
        {
            Order order = _mapper.Map<Order>(orderAddDto);
            var validOrder = await GetValidOrderAsync(order);
            using (var result = await _transactionManagement.BeginTransactionAsync())
            {
                try
                {
                    await CreateOrderWhenValidAsync(order);
                    _orderHistoryService.SetOrderHistoryForOrder(order, OrderStatus.Ordered);
                    await SendOrderNotificationWithSignalRAsync(order.Id);
                    result.Commit();
                }
                catch
                {
                    throw new InternalServerErrorException();
                }
            }
        }

        public async Task ConfirmOrderAsync(string id)
        {
            var order = await _orderDal.GetByIdAsync(Guid.Parse(id));
            ThrowErrorIfOrderNotFound(order);
            if (order.OrderStatus == OrderStatus.OrderConfirmed)
                return;
            order.OrderStatus = OrderStatus.OrderConfirmed;
            _orderHistoryService.SetOrderHistoryForOrder(order, OrderStatus.OrderConfirmed);
            string title = "Sipariş Alındı!",
                content = "Siparişiniz alınmıştır. Afiyet olsun :)";
            using (var result = await _transactionManagement.BeginTransactionAsync())
            {
                try
                {
                    await _walletService.WithdrawFromWalletAsync(order.AppUserId, order.TotalPrice);
                    await _orderDal.UpdateAsync(order);
                    await _oneSignalNotificationService.SendToGivenUserAsync(new() { Title = title, Content = content }, order.AppUserId.ToString());
                    await _notificationService.CreateNotificationAsync(title, content, order.AppUserId.ToString());
                    result.Commit();
                }
                catch
                {
                    throw new InternalServerErrorException();
                }
            }
        }

        public async Task<OrderDetailDto> GetOrderDetailByIdAsync(string id)
        {
            Order order = await _orderDal.GetOrderIncludeProductsAsync(id);
            ThrowErrorIfOrderNotFound(order);
            if (!IsActiveUsersOrder(order) && !_httpContext.User.IsInAnyRoles(RoleConsts.CafeService, RoleConsts.CafeAdmin))
                throw new ForbiddenException();
            return _mapper.Map<OrderDetailDto>(order);
        }

        public List<OrderListDto> GetOrders(OrderRequestFilter filters)
        {
            var query = _orderDal.GetAll()
                .Include(_ => _.TransactionHistories)
                .ThenInclude(_ => _.AppUser)
                .Include(_ => _.OrderProducts)
                .AsNoTracking()
                .AsSplitQuery();
            var filteredResult = new OrderFilterService().FilterOrders(query, filters);
            new HeaderService(_httpContextAccessor).AddToHeaders(filteredResult.Headers);
            return OrderListDtoMapper(filteredResult.ResponseValue);
        }

        private async Task<Order> GetValidOrderAsync(Order order)
        {
            if (order is null || order.OrderProducts is null)
                throw new OrderBadRequestException();
            await _orderProductService.ValidateOrderProductsAsync(order);
            return order.OrderProducts.Count > 0
                ? order
                : throw new OrderBadRequestException();
        }

        private List<OrderListDto> OrderListDtoMapper(List<Order> orders)
        {
            var orderList = new List<OrderListDto>();
            foreach (var order in orders)
            {
                var products = new Dictionary<string, int>();
                foreach (var orderProduct in order.OrderProducts)
                    products.Add(orderProduct.Name, orderProduct.Quantity);
                var orderDto = new OrderListDto()
                {
                    Id = order.Id.ToString(),
                    FullName = order.TransactionHistories.Count > 0 ? order.TransactionHistories.First().AppUser.FullName : null,
                    Amount = order.TotalPrice,
                    Description = order.Description,
                    CreatedDate = order.TransactionHistories.Count > 0 ? order.TransactionHistories.First().CreatedDate : null,
                    Products = products,
                    OrderStatus = order.OrderStatus.ToString(),
                };
                orderList.Add(orderDto);
            }

            return orderList;
        }

        private async Task CreateOrderWhenValidAsync(Order order)
        {
            float orderTotalPrice = GetOrderTotalPrice(order);
            order.AppUserId = Guid.Parse(_httpContext.User.ActiveUserId());
            order.TotalPrice = orderTotalPrice;
            _transactionHistoryService.SetTransactionHistoryForOrder(order, orderTotalPrice, $"Sipariş verildi.", order.AppUserId);
            _orderHistoryService.SetOrderHistoryForOrder(order, OrderStatus.Ordered);
            await _orderDal.AddAsync(order);
        }

        private float GetOrderTotalPrice(Order order)
        {
            float totalPrice = 0;
            if (order.OrderProducts != null)
                foreach (var orderProduct in order.OrderProducts)
                {
                    totalPrice += orderProduct.Price * orderProduct.Quantity;
                    if (orderProduct.OrderProductAttributes != null)
                        foreach (var orderProductAttribute in orderProduct.OrderProductAttributes)
                            totalPrice += orderProductAttribute.Price * orderProductAttribute.Quantity;
                }

            return totalPrice;
        }

        private bool IsActiveUsersOrder(Order order)
            => order.AppUserId == Guid.Parse(_httpContext.User.ActiveUserId());

        private void ThrowErrorIfOrderNotFound(Order order)
        {
            if (order is null)
                throw new OrderNotFoundException();
        }

        private async Task SendOrderNotificationWithSignalRAsync(Guid orderId)
        {
            Order order = _orderDal.GetAll(_ => _.Id == orderId)
                .Include(_ => _.TransactionHistories)
                .ThenInclude(_ => _.AppUser)
                .Include(_ => _.OrderProducts)
                .AsNoTracking()
                .AsSplitQuery()
                .First();
            await _orderNotificationService.SendOrderNotificationAsync(OrderListDtoMapper(new List<Order>() { order }).First());
        }
    }
}
