using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.FollowStory
{
    public class FollowStoryCommandHandler : IRequestHandler<FollowStoryCommand, FollowStoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUserRepository _userRepository;
        private readonly IUserFavoriteRepository _userFavoriteRepository;
        private readonly IStoryRepository _storyRepository;
        public FollowStoryCommandHandler(IUnitOfWork unitOfWork,
                                         IPublishEndpoint publishEndpoint,
                                         IUserRepository userRepository,
                                         IUserFavoriteRepository userFavoriteRepository,
                                         IStoryRepository storyRepository)
        {
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
            _userRepository = userRepository;
            _userFavoriteRepository = userFavoriteRepository;
            _storyRepository = storyRepository;
        }
        public async Task<FollowStoryResponse> Handle(FollowStoryCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if(user == null)
            {
                throw new NotFoundException("user", command.UserId);
            }
            if(user.IsActive == false)
            {
                throw new ForbiddenAccessException();
            }
            var story = await _storyRepository.GetByIdAsync(command.StoryId, ct);
            if (story == null)
            {
                throw new NotFoundException("story", command.StoryId);
            }
            if(story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Deleted)
            {
                throw new BusinessRuleViolationException("Không thể theo dõi truyện đã bị xóa", "STORY_DELETED");
            }
            if (story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Hidden)
            {
                throw new BusinessRuleViolationException("Không thể theo dõi truyện đã bị xóa", "STORY_HIDDEN");
            }
            var checkFollow = await _userFavoriteRepository.IsFavoriteAsync(command.UserId, command.StoryId, ct);
            if (checkFollow)
            {
                throw new BusinessRuleViolationException("Đã theo dõi truyện này rồi", "ALREADY_FOLLOWED");
            }
            var userFavorite = new Domain.Entities.UserFavorite
            {
                UserId = command.UserId,
                StoryId = command.StoryId,
            };
            await _userFavoriteRepository.AddAsync(userFavorite, ct);
            story.FollowCount += 1;
            await _unitOfWork.SaveChangesAsync(ct);
            await _publishEndpoint.Publish(new UserExpActionEvent
            {
                UserId = user.UserId,
                Action = ExpActionType.FollowStory
            }, ct);
            await _publishEndpoint.Publish(new StoryInteractionEvent
            {
                StoryId = story.StoryId,
                ChapterId = null,
                Identity = $"u_{command.UserId}",
                ActionType = InteractionType.Follow,

            });
            var respone = new FollowStoryResponse
            {
                IsFollowing = true,
                Message = "Theo dõi truyện thành công",
                FollowCount = story.FollowCount,
            };
            return respone;
        }

    }
}
