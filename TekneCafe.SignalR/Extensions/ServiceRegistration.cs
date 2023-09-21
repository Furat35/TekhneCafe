using Microsoft.Extensions.DependencyInjection;
using TekneCafe.SignalR.Abstract;
using TekneCafe.SignalR.Concrete;

namespace TekneCafe.SignalR.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IOrderNotificationService, OrderNotificationService>();
        }
    }
}
