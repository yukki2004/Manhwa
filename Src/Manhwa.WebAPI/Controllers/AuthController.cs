using Manhwa.Application.Features.Users.Auth.Commands.Register;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Manhwa.Application.Features.Users.Auth.Commands.Login;
using Manhwa.WebAPI.Extensions;
using Manhwa.Application.Features.Users.Auth.Commands.RefreshToken;
using Microsoft.AspNetCore.Authorization;
using Manhwa.Application.Features.Users.Auth.Commands.Logout;
using System.Security.Claims;
using Manhwa.Application.Features.Users.Auth.Commands.ForgotPassword;
namespace Manhwa.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Application.Features.Users.Auth.Commands.Register.RegisterRequest request)
        {
            var ipAddress = HttpContext.GetRemoteIpAddress();
            var userAgent = Request.Headers["User-Agent"].ToString();
            var commandWithMetadata = new RegisterCommand
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
            var result = await _mediator.Send(commandWithMetadata);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Application.Features.Users.Auth.Commands.Login.LoginRequest request)
        {
            var ipAddress = HttpContext.GetRemoteIpAddress();
            var userAgent = Request.Headers["User-Agent"].ToString();
            var commandWithMetadata = new LoginCommand
            {
                Identifier = request.Identifier,
                Password = request.Password,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
            var result = await _mediator.Send(commandWithMetadata);
            // Thiết lập Cookie bảo mật cho AccessToken
            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            };

            // Thiết lập Cookie bảo mật cho RefreshToken
            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/api/auth",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("accessToken", result.AccessToken, accessCookieOptions);
            Response.Cookies.Append("refreshToken", result.RefreshToken, refreshCookieOptions);
            return Ok(new { result.userId, result.Username, result.Email });
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var oldRefreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(oldRefreshToken))
            {
                return Unauthorized(new { Message = "Phiên đăng nhập không tồn tại." });
            }
            var command = new RefreshTokenCommand
            {
                RefreshToken = oldRefreshToken
            };
            var result = await _mediator.Send(command);

            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            };
            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/api/auth",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("accessToken", result.AccessToken, accessCookieOptions);
            Response.Cookies.Append("refreshToken", result.RefreshToken, refreshCookieOptions);

            return Ok(new { Message = result.Massage });
        }
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { Message = "Phiên đăng nhập không tồn tại." });
            }
            var command = new LogoutCommand
            {
                UserId = userId,
                RefreshToken = refreshToken,
                IpAddress = HttpContext.GetRemoteIpAddress(),
                UserAgent = Request.Headers["User-Agent"].ToString()
            };
            await _mediator.Send(command);

            Response.Cookies.Delete("accessToken", new CookieOptions { Path = "/api" });
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/auth" });

            return Ok(new { Message = "Đã đăng xuất thành công." });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { Message = "Mã xác nhận đã được gửi đến email của bạn." });
            }
            else
            {
                return BadRequest(new { Message = "Yêu cầu đặt lại mật khẩu thất bại." });
            }
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] Application.Features.Users.Auth.Commands.VerifyOtp.VerifyOtpCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { Message = "Xác thực OTP thành công." });
            }
            else
            {
                return BadRequest(new { Message = "Xác thực OTP thất bại." });
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Application.Features.Users.Auth.Commands.ResetPassword.ResetPasswordRequest request)
        {
            var ipAddress = HttpContext.GetRemoteIpAddress();
            var userAgent = Request.Headers["User-Agent"].ToString();
            var commandWithMetadata = new Application.Features.Users.Auth.Commands.ResetPassword.ResetPasswordCommand
            {
                Email = request.Email,
                Otp = request.Otp,
                NewPassword = request.NewPassword,
                ComfirmPassword = request.ComfirmPassword,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
            var result = await _mediator.Send(commandWithMetadata);
            if (result)
            {
                return Ok(new { Message = "Đặt lại mật khẩu thành công." });
            }
            else
            {
                return BadRequest(new { Message = "Đặt lại mật khẩu thất bại." });
            }
        }
    }
}
