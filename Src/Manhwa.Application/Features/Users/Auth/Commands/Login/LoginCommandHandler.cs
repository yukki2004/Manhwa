using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        public readonly IPasswordHasher _passwordHasher;
        public readonly IUserRepository _userRepository;
        public readonly IIdentityService _identityService;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IRefreshTokenRepository _refreshTokenRepository;
        public readonly IPublishEndpoint _publishEndpoint;
        public readonly ICacheService _cacheService;
        public LoginCommandHandler(IPasswordHasher passwordHasher, IUserRepository userRepository, IIdentityService identityService, IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, IPublishEndpoint publishEndpoint, ICacheService cacheService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _publishEndpoint = publishEndpoint;
            _cacheService = cacheService;
        }
        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken ct)
        {
            var userLogin = await _userRepository.GetByIdentifierAsync(command.Identifier,ct);
            if(userLogin == null)
            {
                throw new UnauthorizedAccessException("tài khoản hoặc mật khẩu không đúng");
            }
            if (!_passwordHasher.Verify(command.Password, userLogin.PasswordHash))
            {
                throw new UnauthorizedAccessException("tài khoản hoặc mật khẩu không đúng");
            }
            if(!userLogin.IsActive)
            {
                throw new UnauthorizedAccessException("tài khoản của bạn đã bị khóa");
            }
            var accessToken = _identityService.GenerateAccessToken(userLogin);
            var refreshToken = _identityService.GenerateRefreshToken();
            var userRefreshToken = new Domain.Entities.RefreshToken
            {
                UserId = userLogin.UserId,
                Token = refreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
                CreatedAt = DateTimeOffset.UtcNow,
                IsUsed = false,
            };
            await _refreshTokenRepository.AddAsync(userRefreshToken, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            string redisKey = $"rt_map:{refreshToken}";
            await _cacheService.SetAsync(redisKey, userLogin.UserId.ToString(), TimeSpan.FromDays(7), ct);
            await _publishEndpoint.Publish(new UserLoggedInIntegrationEvent
            {
                UserId = userLogin.UserId,
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
                CreateAt = DateTimeOffset.UtcNow,
            }, ct);
            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                userId = userLogin.UserId,
                Username = userLogin.Username,
                Email = userLogin.Email
            };


        }
    }
}
