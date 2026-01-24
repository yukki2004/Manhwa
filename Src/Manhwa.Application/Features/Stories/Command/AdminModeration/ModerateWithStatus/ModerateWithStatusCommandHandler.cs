using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Enums.Story;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.ModerateWithStatus
{
    public class ModerateWithStatusCommandHandler : IRequestHandler<ModerateWithStatusCommand, ModerateWithStatusResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        public ModerateWithStatusCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _storyRepository = storyRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<ModerateWithStatusResponse> Handle(ModerateWithStatusCommand command, CancellationToken ct)
        {
            var story = await _storyRepository.GetByIdAsync(command.StoryId, ct);
            if (story == null)
            {
                throw new NotFoundException("story", $"không tồn tại {command.StoryId}");
            }
            bool wasLocked = story.AdminLockStatus == AdminLockStatus.Normal;
            story.IsPublish = (StoryPublishStatus)command.IsPublished;
            story.AdminLockStatus = Domain.Enums.AdminLockStatus.Locked;
            story.AdminNote = command.AdminNote;
            story.UpdatedAt = DateTimeOffset.UtcNow;
            await _unitOfWork.SaveChangesAsync(ct);

            if (wasLocked && story.UserId.HasValue)
            {
                var levelData = new
                {
                    title = story.Title,
                    adminNote = story.AdminNote,
                    slug = story.Slug,
                };
                string metadataJson = JsonSerializer.Serialize(levelData);
                await _publishEndpoint.Publish(new SendNotificationEvent
                {
                    ReceiverIds = new List<long> { story.UserId.Value},
                    Type = Domain.Enums.Notification.NotificationType.StoryAlert,
                    RawDataJson = metadataJson,
                    SenderId = 9 // system
                }, ct);
            }
            return new ModerateWithStatusResponse
            {
                IsPublished = story.IsPublish.ToString(),
            };
        }
    }

}
