using Manhwa.Application.Features.Users.Auth.Commands.Register;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Manhwa.Application.Features.Users.Auth.Commands.Login;
using Manhwa.WebAPI.Extensions;
using Manhwa.Application.Features.Users.Auth.Commands.RefreshToken;
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
                Password =  request.Password,
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
                SameSite = SameSiteMode.Strict,
                Path = "/api",
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            };

            // Thiết lập Cookie bảo mật cho RefreshToken
            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("accessToken", result.AccessToken, accessCookieOptions);
            Response.Cookies.Append("refreshToken", result.RefreshToken, refreshCookieOptions);
            return Ok(new {result.userId, result.Username, result.Email});
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
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth/refresh-token",
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            };
            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth/refresh-token",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("accessToken", result.AccessToken, accessCookieOptions);
            Response.Cookies.Append("refreshToken", result.RefreshToken, refreshCookieOptions);

            return Ok(new { Message = result.Massage });
        }

    }
}
