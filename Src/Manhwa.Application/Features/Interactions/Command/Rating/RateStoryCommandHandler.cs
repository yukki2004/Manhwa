using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.Rating
{
    public class RateStoryCommandHandler : IRequestHandler<RateStoryCommand, RateStoryResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;
        public RateStoryCommandHandler(IUserRepository userRepository, IRatingRepository ratingRepository, IStoryRepository storyRepository, IChapterRepository chapterRepository, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
            _storyRepository = storyRepository;
            _chapterRepository = chapterRepository;
            _publishEndpoint = publishEndpoint;
            _unitOfWork = unitOfWork;
        }
        public async Task<RateStoryResponse> Handle(RateStoryCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if(user == null || !user.IsActive)
            {
                throw new NotFoundException("user", command.UserId);
            }
            var story = await _storyRepository.GetByIdAsync(command.StoryId, ct);
            if(story == null)
            {
                throw new NotFoundException("story", command.StoryId);
            }
            bool isAdmin = command.UserRole == UserRole.Admin.ToString();
            if(story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Deleted && !isAdmin)
            {
                throw new BusinessRuleViolationException("Truyện đã bị xóa, không thể thao tác đánh giá", "STORY_DELETED");
            }
            if(story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Hidden &&  !isAdmin)
            {
                throw new BusinessRuleViolationException("Truyện đã bị ẩn, không thể thao tác đánh giá", "STORY_HIDDEN");
            }
            var existingRating = await _ratingRepository.GetByUserIdAndStoryIdAsync(command.UserId, command.StoryId, ct);
            int oldScore = 0;
            bool isNewRating = existingRating == null;

            if (isNewRating)
            {
                var rating = new Domain.Entities.Rating
                {
                    StoryId = command.StoryId,
                    UserId = command.UserId,
                    Score = command.Score,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                };
                await _ratingRepository.AddAsync(rating,ct);
            } else
            {
                oldScore = existingRating.Score;
                existingRating.Score = command.Score;
                existingRating.UpdatedAt = DateTimeOffset.UtcNow;
            }
            await _unitOfWork.SaveChangesAsync(ct);
            await _storyRepository.UpdateStoryStatsAsync(command.StoryId, oldScore, command.Score, isNewRating, ct);
            await _publishEndpoint.Publish(new UserExpActionEvent
            {
                UserId = command.UserId,
                Action = ExpActionType.FollowStory,
            });
            return new RateStoryResponse
            {
                Success = true,
                Message = isNewRating ? "Đánh giá thành công!" : "Cập nhật đánh giá thành công!"
            };
        }
             
    }

}
