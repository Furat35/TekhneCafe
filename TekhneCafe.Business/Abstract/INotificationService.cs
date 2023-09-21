using TekhneCafe.Core.DTOs.Notification;
using TekhneCafe.Core.Filters.Notification;

namespace TekhneCafe.Business.Abstract
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string title, string message, string userId);
        List<NotificationListDto> GetNotifications(NotificationRequestFilter filters = null);
    }
}
