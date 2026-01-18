using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutResponse>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        public LogoutCommandHandler(
            IPublishEndpoint publishEndpoint,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IIdentityService identityService)
        {
            _publishEndpoint = publishEndpoint;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
        }
        public async Task<LogoutResponse> Handle(LogoutCommand command, CancellationToken ct)
        {
            if (!command.RefreshToken.IsNullOrEmpty())
            {
                var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, ct);
                if (existingRefreshToken != null && existingRefreshToken.UserId == command.UserId)
                {
                    await _refreshTokenRepository.MarkTokenAsUsedAsync(command.RefreshToken, ct);
                } else
                {
                    throw new SecurityTokenException("Invalid refresh token");
                }

            }
            await _publishEndpoint.Publish(new UserLoggedOutIntegrationEvent
            {
                UserId = command.UserId,
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
                CreateAt = DateTimeOffset.UtcNow,
                Action = Domain.Enums.UserLogAction.Logout
            }, ct);
            return new LogoutResponse
            {
               Message = "Đăng xuất thành công."
            };
        }

    }
}
