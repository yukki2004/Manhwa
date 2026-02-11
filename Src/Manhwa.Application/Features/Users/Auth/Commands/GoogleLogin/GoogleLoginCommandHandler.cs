using Google.Apis.Auth;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Application.Features.Users.Auth.Commands.Login;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;


namespace Manhwa.Application.Features.Users.Auth.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, GoogleLoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        private readonly IStorageService _storageService;
        private readonly ICacheService _cacheService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly HttpClient _httpClient;

        public GoogleLoginCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IIdentityService identityService,
            IStorageService storageService,
            ICacheService cacheService,
            IPublishEndpoint publishEndpoint,
            IHttpClientFactory httpClientFactory)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _storageService = storageService;
            _cacheService = cacheService;
            _publishEndpoint = publishEndpoint;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<GoogleLoginResponse> Handle(GoogleLoginCommand command, CancellationToken ct)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(command.IdToken);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Xác thực Google không hợp lệ.");
            }

            var user = await _userRepository.GetByEmailAsync(payload.Email, ct);

            if (user != null)
            {
                if (user.LoginType == LoginType.Local)
                {
                    throw new UnauthorizedAccessException("Email này đã được đăng ký bằng mật khẩu. Vui lòng đăng nhập bình thường.");
                }

                if (!user.IsActive)
                    throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khóa.");
            }
            else
            {
                user = new User
                {
                    Username = payload.Name ?? payload.Email.Split('@')[0],
                    Email = payload.Email,
                    LoginType = LoginType.Google,
                    GoogleId = payload.Subject,
                    Role = UserRole.User,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    IsActive = true
                };

                await _userRepository.AddAsync(user, ct);
                await _unitOfWork.SaveChangesAsync(ct); 

                if (!string.IsNullOrEmpty(payload.Picture))
                {
                    user.Avatar = await ProcessAndUploadAvatar(user.UserId, payload.Picture, ct);
                    _userRepository.Update(user);
                    await _unitOfWork.SaveChangesAsync(ct);
                }

                await _publishEndpoint.Publish(new UserRegisteredIntegrationEvent
                {
                    UserId = user.UserId,
                    UserAgent = command.UserAgent,
                    IpAddress = command.IpAddress,
                    CreateAt = DateTimeOffset.UtcNow
                }, ct);

                await _publishEndpoint.Publish(new SendEmailIntegrationEvent
                {
                    To = user.Email,
                    Subject = "Chào mừng bạn gia nhập thế giới TruyenVerse!",
                    TemplateName = "WELCOME_NEW_USER",
                    TemplateData = new Dictionary<string, string> { { "Username", user.Username } }
                }, ct);
            }

            var accessToken = _identityService.GenerateAccessToken(user);
            var refreshToken = _identityService.GenerateRefreshToken();

            var userRefreshToken = new Domain.Entities.RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
                CreatedAt = DateTimeOffset.UtcNow,
                IsUsed = false
            };
            await _refreshTokenRepository.AddAsync(userRefreshToken, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            string redisKey = $"rt_map:{refreshToken}";
            await _cacheService.SetAsync(redisKey, user.UserId.ToString(), TimeSpan.FromDays(7), ct);
            await _publishEndpoint.Publish(new UserLoggedInIntegrationEvent
            {
                UserId = user.UserId,
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
                CreateAt = DateTimeOffset.UtcNow
            }, ct);

            return new GoogleLoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email
            };
        }

        private async Task<string> ProcessAndUploadAvatar(long userId, string googlePicUrl, CancellationToken ct)
        {
            try
            {
                var response = await _httpClient.GetAsync(googlePicUrl, ct);
                if (!response.IsSuccessStatusCode) return null!;

                using var stream = await response.Content.ReadAsStreamAsync(ct);
                string r2Path = $"users/{userId}/avatar/avatar.webp";
                return await _storageService.UploadAsync(stream, r2Path, "image/webp", false, ct);
            }
            catch { return null!; }
        }
    }
}
