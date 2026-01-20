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

namespace Manhwa.Application.Features.Users.Profile.Command.UpdateAvt
{
    public class UpdateAvtCommandHandler : IRequestHandler<UpdateAvtCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IStorageService _storageService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateAvtCommandHandler(
            IPublishEndpoint publishEndpoint,
            IStorageService storageService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _publishEndpoint = publishEndpoint;
            _storageService = storageService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateAvtCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if (user == null) throw new Exception("Không tìm thấy người dùng.");
            var path = $"users/{user.UserId}/avatar/avatar.webp";
            using var stream = command.File.OpenReadStream();
            var relativePath = await _storageService.UploadAsync(stream, path, "image/webp", false, ct);
            user.Avatar = relativePath;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(ct);


            await _publishEndpoint.Publish(new ProfileUpdatedIntegrationEvent
            {
                UserId = user.UserId,
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
                Action = Domain.Enums.UserLogAction.UpdateProfile,
                CreateAt = DateTimeOffset.UtcNow
            }, ct);

            return true;
        }
    }
}
