using Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus;
using Manhwa.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manhwa.WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminUserController(IMediator mediator) => _mediator = mediator;
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ToggleStatus(long id, [FromBody] bool isActive)
        {
            var result = await _mediator.Send(new UpdateUserStatusCommand
            {
                UserId = id,
                IsActive = isActive,
                IpAddress = HttpContext.GetRemoteIpAddress(),
                UserAgent = Request.Headers["User-Agent"].ToString()
            });

            return Ok(new
            {
                Success = result,
                Message = isActive ? "Mở khóa thành công." : "Đã khóa và đăng xuất khỏi các thiết bị."
            });
        }
    }
}
