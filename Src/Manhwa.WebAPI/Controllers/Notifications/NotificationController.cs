using Manhwa.Application.Features.Notifications.Queries.GetNotifications;
using Manhwa.Application.Features.Notifications.Queries.GetUnreadCount;
using Manhwa.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manhwa.WebAPI.Controllers.Notifications
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyNotifications([FromQuery] GetNotificationsRequest request)
        {
            var userId = User.GetUserId(); 
            if (userId == null) return Unauthorized();

            var result = await _mediator.Send(new GetNotificationsQuery
            {
                UserId = (long)userId,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            });

            return Ok(result);
        }
        [HttpGet("unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.GetUserId(); 
            if (userId == null) return Unauthorized();
            var count = await _mediator.Send(new GetUnreadCountQuery { UserId = (long)userId });
            return Ok(new { unreadCount = count });
        }
    }
}
