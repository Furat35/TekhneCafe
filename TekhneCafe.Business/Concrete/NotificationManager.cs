using AutoMapper;
using Microsoft.AspNetCore.Http;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Helpers.FilterServices;
using TekhneCafe.Business.Helpers.HeaderServices;
using TekhneCafe.Core.DTOs.Notification;
using TekhneCafe.Core.Extensions;
using TekhneCafe.Core.Filters.Notification;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDal _notificationDal;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public NotificationManager(INotificationDal notificationDal, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _notificationDal = notificationDal;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task CreateNotificationAsync(string title, string message, string userId)
        {
            var notification = new Notification()
            {
                AppUserId = Guid.Parse(userId),
                Message = message,
                Title = title
            };
            await _notificationDal.AddAsync(notification);
        }

        public List<NotificationListDto> GetNotifications(NotificationRequestFilter filters = null)
        {
            Guid userId = Guid.Parse(_httpContext.HttpContext.User.ActiveUserId());
            var filteredResult = new NotificationFilterService().FilterNotifications(_notificationDal.GetAll(_ => _.AppUserId == userId), filters);
            new HeaderService(_httpContext).AddToHeaders(filteredResult.Headers);
            return _mapper.Map<List<NotificationListDto>>(filteredResult.ResponseValue);
        }
    }
}
