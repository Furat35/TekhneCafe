using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.Filters.Notification;

namespace TekhneCafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Get Notifications
        /// </summary>
        /// <param name="filters">Notification filters</param>
        /// <returns>Notifications</returns>
        /// <response code="200">Returns notifications</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<IActionResult> Notifications([FromQuery] NotificationRequestFilter filters)
        {
            var notifications = _notificationService.GetNotifications(filters);
            return Ok(notifications);
        }
    }
}
