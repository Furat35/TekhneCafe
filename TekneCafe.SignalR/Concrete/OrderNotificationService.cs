using Microsoft.AspNetCore.SignalR;
using TekhneCafe.Core.DTOs.Order;
using TekneCafe.SignalR.Abstract;
using TekneCafe.SignalR.Hubs;

namespace TekneCafe.SignalR.Concrete
{
    public class OrderNotificationService : IOrderNotificationService
    {
        private readonly IHubContext<OrderNoficationHub> _context;
        public OrderNotificationService(IHubContext<OrderNoficationHub> context)
        {
            _context = context;
        }

        public async Task SendOrderNotificationAsync(OrderListDto order)
        {
            await _context.Clients.All.SendAsync("receiveOrder", order);
        }
    }
}
