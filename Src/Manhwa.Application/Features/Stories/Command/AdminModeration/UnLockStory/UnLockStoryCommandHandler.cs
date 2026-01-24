using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Application.Features.Stories.Command.AdminModeration.LockStory;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.UnLockStory
{
    public class UnLockStoryCommandHandler : IRequestHandler<UnLockStoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoryRepository _storyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public UnLockStoryCommandHandler(IUnitOfWork unitOfWork, IStoryRepository storyRepository, IUserRepository userRepository, IPublishEndpoint publishEndpoint)
        {
            _unitOfWork = unitOfWork;
            _storyRepository = storyRepository;
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<bool> Handle(UnLockStoryCommand command, CancellationToken ct)
        {
            var story = await _storyRepository.GetByIdAsync(command.StoryId, ct);
            if (story == null)
            {
                throw new NotFoundException("story", command.StoryId);
            }
            if(story.AdminLockStatus == Domain.Enums.AdminLockStatus.Normal)
            {
                return true;
            }
            var lockCheck = story.AdminLockStatus == Domain.Enums.AdminLockStatus.Locked;
            story.AdminLockStatus = Domain.Enums.AdminLockStatus.Normal;
            story.AdminNote = command.AdminNote;
            if (lockCheck && story.UserId.HasValue)
            {
                var levelData = new
                {
                    title = story.Title,
                    adminNote = story.AdminNote,
                    slug = story.Slug,
                    reason = "mở khóa"
                };
                string metadataJson = JsonSerializer.Serialize(levelData);
                await _publishEndpoint.Publish(new SendNotificationEvent
                {
                    ReceiverIds = new List<long> { story.UserId.Value },
                    Type = Domain.Enums.Notification.NotificationType.StoryAlert,
                    RawDataJson = metadataJson,
                    SenderId = 9 // system
                }, ct);
            }
            await _unitOfWork.SaveChangesAsync(ct);
            return true;

        }
    }
}
