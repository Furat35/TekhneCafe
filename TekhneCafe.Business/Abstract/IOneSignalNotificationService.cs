using TekhneCafe.Business.Models;

namespace TekhneCafe.Business.Abstract
{
    public interface IOneSignalNotificationService
    {
        Task SendToGivenUserAsync(CreateNotificationModel notificationModel, string userId);
    }
}
