using Manhwa.Application.Features.Users.Profile.Command.ChangePassword;
using Manhwa.Application.Features.Users.Profile.Command.UpdateAvt;
using Manhwa.Application.Features.Users.Profile.Command.UpdateProfile;
using Manhwa.Application.Features.Users.Profile.Queries.GetFavorites;
using Manhwa.Application.Features.Users.Profile.Queries.GetMyProfile;
using Manhwa.Application.Features.Users.Profile.Queries.GetReadingHistory;
using Manhwa.Application.Features.Users.Profile.Queries.GetUserProfile;
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
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var command = new ChangePasswordCommand
            {
                UserId = (long)userId,
                OldPassword = request.OldPassword,
                NewPassword = request.NewPassword,
                ConfirmPassword = request.ConfirmPassword,
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
        [HttpGet("get-my-profile")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile(CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var query = new GetMyProfileCommand
            {
                UserId = (long)userId
            };
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }
        [HttpGet("get-user-profile/{username}")]
        public async Task<IActionResult> GetUserProfile([FromRoute] string username, CancellationToken ct)
        {
            var query = new GetUserProfileCommand
            {
                Username = username
            };
            var result = await _mediator.Send(query, ct);
            if (result == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }
            return Ok(result);
        }
        [HttpGet("favorites")]
        [Authorize]
        public async Task<IActionResult> Getfavorites([FromQuery] GetFavoriteStoriesQuery query, CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null) return NotFound();
            query.UserId = (long)userId;
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }
        [HttpGet("histories")]
        [Authorize]
        public async Task<IActionResult> GetHistories([FromQuery] GetReadingHistoryQuery query, CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null) return NotFound();
            query.UserId = (long)userId;
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }
    }
}

