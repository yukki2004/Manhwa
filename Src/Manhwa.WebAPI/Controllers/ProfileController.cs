using Manhwa.Application.Features.Users.Profile.Command.ChangePassword;
using Manhwa.Application.Features.Users.Profile.Command.UpdateAvt;
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
                return BadRequest("Lỗi khi cập nhật thông tin.");
            }
            return Ok("Cập nhật thông tin thành công.");
        }
        [HttpPut("update-avt")]
        [Authorize]
        public async Task<IActionResult> UpdateAvt([FromForm] UpdateAvtRequest request, CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var command = new UpdateAvtCommand
            {
                UserId = (long)userId,
                File = request.File,
                IpAddress = HttpContext.GetRemoteIpAddress(),
                UserAgent = Request.Headers["User-Agent"].ToString()
            };
            var result = await _mediator.Send(command, ct);
            if (!result)
            {
                return BadRequest("Lỗi khi cập nhật ảnh đại diện.");
            }
            return Ok("Cập nhật ảnh đại diện thành công.");
        }
        [HttpPut("change-passwword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var command = new ChangePasswordCommand
            {
                UserId = (long)userId,
                Password = request.Password,
                IpAddress = HttpContext.GetRemoteIpAddress(),
                UserAgent = Request.Headers["User-Agent"].ToString()
            };
            var result = await _mediator.Send(command, ct);
            if (!result)
            {
                return BadRequest("Lỗi khi đổi mật khẩu.");
            }
            return Ok("Đổi mật khẩu thành công.");
        }
    }
}

