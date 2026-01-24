using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.UpdateStory
{
    public class UpdateStoryCommandHandler : IRequestHandler<UpdateStoryCommand, UpdateStoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoryRepository _storyRepository;
        private readonly IUserRepository _userRepository;
        public UpdateStoryCommandHandler(IUnitOfWork unitOfWork, IStoryRepository storyRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _storyRepository = storyRepository;
            _userRepository = userRepository;
        }
        public async Task<UpdateStoryResponse> Handle(UpdateStoryCommand command, CancellationToken ct)
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
            var story = await _storyRepository.GetByIdWithCategoriesAsync(command.StoryId, ct);
            if (story == null)
            {
                throw new NotFoundException("story", command.StoryId);
            }
            if(command.UserRole == "Admin")
            {
                var newCategoryIds = command.CategoryIds;

                var toRemove = story.StoryCategories
                    .Where(sc => !newCategoryIds.Contains(sc.CategoryId))
                    .ToList();

                foreach (var item in toRemove)
                {
                    story.StoryCategories.Remove(item); 
                }

                var existingIds = story.StoryCategories.Select(sc => sc.CategoryId).ToList();
                var toAddIds = newCategoryIds.Except(existingIds).ToList();

                foreach (var catId in toAddIds)
                {
                    story.StoryCategories.Add(new StoryCategory
                    {
                        StoryId = story.StoryId,
                        CategoryId = catId
                    }); 
                }
                story.Author = command.AuthorName;
                story.Title = command.Title;
                story.Slug = $"{command.Title.ToSlug()}-{story.StoryId}";
                story.Description = command.Description;
                story.ReleaseYear = command.ReleaseYear;
                story.UpdatedAt = DateTimeOffset.UtcNow;
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
                var newCategoryIds = command.CategoryIds;
                var toRemove = story.StoryCategories
                    .Where(sc => !newCategoryIds.Contains(sc.CategoryId))
                    .ToList();
                foreach (var item in toRemove)
                {
                    story.StoryCategories.Remove(item);
                }
                var existingIds = story.StoryCategories.Select(sc => sc.CategoryId).ToList();
                var toAddIds = newCategoryIds.Except(existingIds).ToList();
                foreach (var catId in toAddIds)
                {
                    story.StoryCategories.Add(new StoryCategory
                    {
                        StoryId = story.StoryId,
                        CategoryId = catId
                    });
                }
                story.Author = command.AuthorName;
                story.Title = command.Title;
                story.Slug = $"{command.Title.ToSlug()}-{story.StoryId}";
                story.Description = command.Description;
                story.ReleaseYear = command.ReleaseYear;
                story.UpdatedAt = DateTimeOffset.UtcNow;
            }
            await _unitOfWork.SaveChangesAsync(ct);
            var storyDto = await _storyRepository.GetByIdWithCategoriesAsync(command.StoryId,ct);

            return new UpdateStoryResponse
            {
                Title = story.Title,
                AuthorName = story.Author,
                Description = story.Description,
                ReleaseYear = story.ReleaseYear,
                Categories = storyDto.StoryCategories.Select(sc => new DTO.UpdateStoryDto
                {
                    Id = sc.CategoryId,
                    Name = sc.Category.Name,
                    Slug = sc.Category.Slug
                }).ToList()
            };
        }

    }
}
