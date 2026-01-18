using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IIdentityService identityService, IUserRepository userRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _userRepository = userRepository;
        }
        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand command, CancellationToken ct)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, ct);
            if (refreshToken == null || refreshToken.ExpiresAt < DateTimeOffset.UtcNow)
            {
                throw new UnauthorizedAccessException("Phiên đăng nhập hết hạn.");
            }
            if (refreshToken.IsUsed)
            {
                await _refreshTokenRepository.DeleteAllUserTokensAsync(refreshToken.UserId, ct);
                throw new UnauthorizedAccessException("Cảnh báo bảo mật: Token đã được sử dụng trước đó.");
            }
            refreshToken.IsUsed = true;
            var user = await _userRepository.GetByIdAsync(refreshToken.UserId, ct);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Người dùng không tồn tại.");
            }
            var newAccessToken = _identityService.GenerateAccessToken(user);
            var newRefreshToken = _identityService.GenerateRefreshToken();
            var newRefreshTokenEntity = new Domain.Entities.RefreshToken
            {
                UserId = user.UserId,
                Token = newRefreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
                CreatedAt = DateTimeOffset.UtcNow,
                IsUsed = false,
            };
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Massage = "Làm mới token thành công."
            };
        }
    }
}
