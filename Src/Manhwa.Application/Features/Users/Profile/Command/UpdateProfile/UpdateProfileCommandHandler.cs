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

namespace Manhwa.Application.Features.Users.Profile.Command.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProfileCommandHandler(IUserRepository userRepository, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, ct);
            if (user == null)
            {
                return false;
            }
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khóa không thể cập nhật hồ sơ.");
            }
            user.Description = request.Description;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(ct);
            await _publishEndpoint.Publish(new ProfileUpdatedIntegrationEvent
            {
                UserId = user.UserId,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent,
                Action = Domain.Enums.UserLogAction.UpdateProfile,
                CreateAt = DateTimeOffset.UtcNow
            }, ct);
            return true;
        }
    }
}
