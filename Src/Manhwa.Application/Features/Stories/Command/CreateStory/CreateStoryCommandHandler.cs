using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.CreateStory
{
    public class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, CreateStoryResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStorageService _storageService;
        private readonly IUserRepository _userRepository;
        public CreateStoryCommandHandler(IStoryRepository storyRepository,
                                         IUnitOfWork unitOfWork,
                                         IPublishEndpoint publishEndpoint,
                                         ICategoryRepository categoryRepository,
                                         IStorageService storageService,
                                         IUserRepository userRepository)
        {
            _storyRepository = storyRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
            _categoryRepository = categoryRepository;
            _storageService = storageService;
            _userRepository = userRepository;
        }
        public async Task<CreateStoryResponse> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            if (!user.IsActive)
            {
                throw new Exception("User is banned");
            }

            var story = new Story
            {
                Title = request.Title,
                Description = request.Description,
                Slug = $"{request.Title.ToSlug()}-temp-{Guid.NewGuid().ToString()[..4]}",
                UserId = request.UserId,
                ReleaseYear = request.ReleaseYear,
                Author = request.Author,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow

            };
            await _storyRepository.AddAsync(story, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (request.ThumbnailFile != null)
            {
                var extension = Path.GetExtension(request.ThumbnailFile.FileName);
               
                var r2Path = $"stories/{story.StoryId}/cover/cover.webp";

                using var stream = request.ThumbnailFile.OpenReadStream();

                await _storageService.UploadAsync(stream, r2Path, "image/webp", false);

                story.ThumbnailUrl = r2Path;
            }
            if (request.GenreIds.Any())
            {
                var storyCategories = request.GenreIds.Select(catId => new StoryCategory
                {
                    StoryId = story.StoryId,
                    CategoryId = catId
                }).ToList();
                await _categoryRepository.AddStoryCategoriesAsync(storyCategories, cancellationToken);
            }
            story.Slug = $"{request.Title.ToSlug()}-{story.StoryId}";
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _publishEndpoint.Publish(new UserExpActionEvent
            {
                UserId = request.UserId,
                Action = Domain.Enums.ExpActionType.CreateStory
            }, cancellationToken);
            return new CreateStoryResponse
            {
                StoryId = story.StoryId,
                Slug = story.Slug,
                Title = story.Title
            };

        }

    }
}
