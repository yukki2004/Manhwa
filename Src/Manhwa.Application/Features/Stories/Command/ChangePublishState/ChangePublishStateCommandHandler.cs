using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Enums.Story;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.ChangePublishState
{
    public class ChangePublishStateCommandHandler : IRequestHandler<ChangePublishStateCommand, ChangePublishStateResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoryRepository _storyRepository;
        private readonly IUserRepository _userRepository;
        public ChangePublishStateCommandHandler(IUnitOfWork unitOfWork, IStoryRepository storyRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _storyRepository = storyRepository;
            _userRepository = userRepository;
        }
        public async Task<ChangePublishStateResponse> Handle(ChangePublishStateCommand command, CancellationToken ct)
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
            if (story.UserId != command.UserId) throw new ForbiddenAccessException();
            if(story.AdminLockStatus == Domain.Enums.AdminLockStatus.Locked)
            {
                throw new BusinessRuleViolationException(
        $"Không thể thao tác: Truyện đã bị khóa bởi Admin. Lý do: {story.AdminNote}",
        "STORY_IS_LOCKED");
            }
            story.IsPublish = (StoryPublishStatus)command.IsPublished;
            await _unitOfWork.SaveChangesAsync(ct);
            return new ChangePublishStateResponse
            {
                IsPublished = story.IsPublish.ToString(),
            };
        }
    }
}
