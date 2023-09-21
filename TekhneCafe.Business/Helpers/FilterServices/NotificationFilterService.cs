using TekhneCafe.Core.Filters;
using TekhneCafe.Core.Filters.Notification;
using TekhneCafe.Core.ResponseHeaders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Helpers.FilterServices
{
    public class NotificationFilterService
    {
        public NotificationResponseFilter<List<Notification>> FilterNotifications(IQueryable<Notification> notifications, NotificationRequestFilter filters)
        {
            var filteredNotifications = notifications
                .OrderByDescending(_ => _.CreatedDate)
                .Skip(filters.Page * filters.Size)
                .Take(filters.Size)
                .ToList();
            Metadata metadata = new(filters.Page, filters.Size, notifications.Count(), notifications.Count() / filters.Size + 1);
            var header = new CustomHeaders().AddPaginationHeader(metadata);

            return new()
            {
                ResponseValue = filteredNotifications,
                Headers = header
            };
        }
    }
}
