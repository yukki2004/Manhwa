using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.Unfollow
{
    public class UnfollowStoryCommandHandler : IRequestHandler<UnfollowStoryCommand, UnfollowStoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserFavoriteRepository _userFavoriteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStoryRepository _storyRepository;
        public UnfollowStoryCommandHandler(IUnitOfWork unitOfWork,
                                           IUserFavoriteRepository userFavoriteRepository,
                                           IUserRepository userRepository,
                                           IStoryRepository storyRepository)
        {
            _unitOfWork = unitOfWork;
            _userFavoriteRepository = userFavoriteRepository;
            _userRepository = userRepository;
            _storyRepository = storyRepository;
        }
        public async Task<UnfollowStoryResponse> Handle(UnfollowStoryCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if (user == null)
            {
                throw new NotFoundException("user", command.UserId);
            }
            if (user.IsActive == false)
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
                throw new BusinessRuleViolationException("Không thể hủy theo dõi truyện đã bị xóa", "STORY_DELETED");
            }
            if(story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Hidden)
            {
                throw new BusinessRuleViolationException("Không thể hủy theo dõi truyện đã bị ẩn", "STORY_HIDDEN");
            }
            var followCheck = await _userFavoriteRepository.IsFavoriteAsync(command.UserId, command.StoryId, ct);
            if (!followCheck)
            {
                throw new BusinessRuleViolationException("Chưa theo dõi truyện này", "NOT_FOLLOWED_YET");
            }
            var userFavorite = new UserFavorite
            {
                UserId = command.UserId,
                StoryId = command.StoryId
            };
            _userFavoriteRepository.Remove(userFavorite);
            story.FollowCount = Math.Max(0, story.FollowCount - 1);
            await _unitOfWork.SaveChangesAsync(ct);
            return new UnfollowStoryResponse
            {
                IsFollowing = false,
                FollowCount = story.FollowCount,
                Message = "Hủy theo dõi truyện thành công"
            };

        }


    }
}
