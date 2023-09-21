using TekhneCafe.Core.DTOs.Order;

namespace TekneCafe.SignalR.Abstract
{
    public interface IOrderNotificationService
    {
        Task SendOrderNotificationAsync(OrderListDto order);
    }
}
