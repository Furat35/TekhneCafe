using TekhneCafe.Core.DTOs.Order;
using TekhneCafe.Core.Filters.Order;

namespace TekhneCafe.Business.Abstract
{
    public interface IOrderService
    {
        Task<OrderDetailDto> GetOrderDetailByIdAsync(string id);
        Task CreateOrderAsync(OrderAddDto orderAddDto);
        Task ConfirmOrderAsync(string id);
        List<OrderListDto> GetOrders(OrderRequestFilter filters);
    }
}
