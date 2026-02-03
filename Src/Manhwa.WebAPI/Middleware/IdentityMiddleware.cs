using Manhwa.Application.Common.Interfaces;
using Manhwa.WebAPI.Extensions;
using System.Security.Claims;

namespace Manhwa.WebAPI.Middleware
{
    public class IdentityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IdentityMiddleware> _logger;

        public IdentityMiddleware(RequestDelegate next, ILogger<IdentityMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ICacheService cacheService)
        {
            string identity = string.Empty;

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                identity = $"{IdentityConstants.UserPrefix}{userId}";

                TryDeleteGuestCookie(context);
            }
            else
            {
                bool hasRefreshToken = context.Request.Cookies.TryGetValue(IdentityConstants.RefreshTokenCookieName, out var refreshToken);

                if (hasRefreshToken && !string.IsNullOrWhiteSpace(refreshToken))
                {
                    string redisKey = $"rt_map:{refreshToken}";
                    var userId = await cacheService.GetAsync<string>(redisKey);

                    if (!string.IsNullOrEmpty(userId))
                    {
                        identity = $"{IdentityConstants.UserPrefix}{userId}";

                        TryDeleteGuestCookie(context);

                        _logger.LogInformation($"Silent Identity Recovery: Identified User {userId} via Redis.");
                    }
                }

                if (string.IsNullOrEmpty(identity))
                {
                    if (!context.Request.Cookies.TryGetValue(IdentityConstants.GuestCookieName, out var guestId))
                    {
                        guestId = Guid.NewGuid().ToString("N");
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Path = "/",
                            Expires = DateTimeOffset.UtcNow.AddDays(30),
                            IsEssential = true
                        };
                        context.Response.Cookies.Append(IdentityConstants.GuestCookieName, guestId, cookieOptions);
                    }
                    identity = $"{IdentityConstants.GuestPrefix}{guestId}";
                }
            }

            context.Items[IdentityConstants.ContextItemKey] = identity;

            await _next(context);
        }

        private void TryDeleteGuestCookie(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey(IdentityConstants.GuestCookieName))
            {
                context.Response.Cookies.Delete(IdentityConstants.GuestCookieName, new CookieOptions
                {
                    Path = "/",
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
            }
        }
    }
}