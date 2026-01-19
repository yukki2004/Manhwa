using Manhwa.Application.Features.Users.Profile.Command.UpdateProfile;
using Manhwa.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manhwa.WebAPI.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }
    
        [HttpPut("update-description")]
        [Authorize]
        public async Task<IActionResult> UpdateDescription([FromBody] UpdateProfileRequest request, CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var command = new UpdateProfileCommand
            {
                UserId = (long)userId,
                Description = request.Description,
                IpAddress = HttpContext.GetRemoteIpAddress(),
                UserAgent = Request.Headers["User-Agent"].ToString()
            };
            var result = await _mediator.Send(command, ct);
            if (!result)
            {
                return BadRequest("Lỗi khi cập nhật profile.");
            }
            return Ok("Cập nhật thông tin thành c.");
        }
    }
}

