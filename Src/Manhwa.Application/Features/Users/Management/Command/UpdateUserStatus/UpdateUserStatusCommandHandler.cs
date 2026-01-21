using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus
{
    public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public UpdateUserStatusCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IRefreshTokenRepository refreshTokenRepository, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<bool> Handle(UpdateUserStatusCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }
            user.IsActive = command.IsActive;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            _userRepository.Update(user);
            if (!command.IsActive)
            {
                await _refreshTokenRepository.DeleteAllUserTokensAsync(user.UserId, ct);
            }
            await _publishEndpoint.Publish(new UserAccountStatusEvent
            {
                UserId = user.UserId,
                UserAgent = command.UserAgent,
                IpAddress = command.IpAddress,
                Action = command.IsActive ? UserLogAction.UnlockAccount : UserLogAction.LockAccount,
                CreateAt = DateTimeOffset.UtcNow
            });
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }

    }
}
