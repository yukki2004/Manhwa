using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStoryAvatar
{
    public class UpdateStoryAvatarCommandHandler : IRequestHandler<UpdateStoryAvatarCommand, UpdateStoryAvatarResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IUserRepository _userRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateStoryAvatarCommandHandler(IStorageService storageService, IUserRepository userRepository, IStoryRepository storyRepository, IUnitOfWork unitOfWork)
        {
            _storageService = storageService;
            _userRepository = userRepository;
            _storyRepository = storyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<UpdateStoryAvatarResponse> Handle(UpdateStoryAvatarCommand command, CancellationToken ct)
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
            if (command.UserRole == "Admin")
            {
                if (command.ThumbnailFile != null)
                {
                    var extension = Path.GetExtension(command.ThumbnailFile.FileName);

                    var r2Path = $"stories/{story.StoryId}/cover/cover.webp";

                    using var stream = command.ThumbnailFile.OpenReadStream();

                    await _storageService.UploadAsync(stream, r2Path, "image/webp", false);

                    story.ThumbnailUrl = r2Path;
                    story.UpdatedAt = DateTimeOffset.UtcNow;
                }
            } else
            {
                if (story.UserId != command.UserId) throw new ForbiddenAccessException();
                if (story.AdminLockStatus == Domain.Enums.AdminLockStatus.Locked)
                {
                    throw new BusinessRuleViolationException(
                    $"Không thể thao tác: Truyện đã bị khóa bởi Admin. Lý do: {story.AdminNote}",
                    "STORY_IS_LOCKED");
                }
                if (story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Deleted)
                {
                    throw new BusinessRuleViolationException(
                    $"Không thể thao tác: Truyện đã bị xóa",
                    "STORY_IS_DELETED");
                }
                if (command.ThumbnailFile != null)
                {
                    var extension = Path.GetExtension(command.ThumbnailFile.FileName);
                    var r2Path = $"stories/{story.StoryId}/cover/cover.webp";
                    using var stream = command.ThumbnailFile.OpenReadStream();
                    await _storageService.UploadAsync(stream, r2Path, "image/webp", false);
                    story.ThumbnailUrl = r2Path;
                    story.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }
            await _unitOfWork.SaveChangesAsync(ct);
            return new UpdateStoryAvatarResponse
            {
                NewAvatarUrl = story.ThumbnailUrl.ToFullUrl()
            };


        }
    }
}
