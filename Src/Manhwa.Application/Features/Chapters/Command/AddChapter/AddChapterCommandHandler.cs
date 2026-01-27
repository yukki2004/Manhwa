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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.AddChapter
{
    public class AddChapterCommandHandler : IRequestHandler<AddChapterCommand, AddChapterResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChapterRepository _chapterRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICacheService _cacheService;
        private readonly IStorageService _storageService;
        public AddChapterCommandHandler(IUnitOfWork unitOfWork,
                                        IChapterRepository chapterRepository,
                                        IStoryRepository storyRepository,
                                        IUserRepository userRepository,
                                        IPublishEndpoint publishEndpoint,
                                        ICacheService cacheService,
                                        IStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _chapterRepository = chapterRepository;
            _storyRepository = storyRepository;
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _cacheService = cacheService;
            _storageService = storageService;
        }
        public async Task<AddChapterResponse> Handle(AddChapterCommand command, CancellationToken ct)
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
            if (story.UserId != command.UserId && command.UserRole != "Admin")
            {
                throw new ForbiddenAccessException();
            }
            if(story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Deleted)
            {
                throw new BusinessRuleViolationException("Không thể thêm chương cho truyện đã bị xóa", "STORY_DELETED");
            }
            if(story.AdminLockStatus == Domain.Enums.AdminLockStatus.Locked && command.UserRole != "Admin")
            {
                throw new BusinessRuleViolationException("Không thể thêm chương cho truyện đã bị khóa bởi Admin", "STORY_LOCKED");
            }
            string slug = $"chuong-{command.ChapterNumber.ToString().Replace(".", "-")}";
            var newChapter = new Chapter
            {
                StoryId = command.StoryId,
                Title = command.Title,
                Slug = slug,
                ChapterNumber = command.ChapterNumber,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _chapterRepository.AddAsync(newChapter, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            var files = command.Images.ToList();
            var uploadTasks = files.Select(async (file, index) =>
            {
                var orderIndex = index + 1;
                var path = $"stories/{story.StoryId}/chapters/{newChapter.ChapterId}/images/{orderIndex:D3}.webp";

                using var inputStream = file.OpenReadStream();
                using var outputStream = new MemoryStream();

                using (var image = await Image.LoadAsync(inputStream, ct))
                {
                    var encoder = new WebpEncoder
                    {
                        Quality = 75, 
                        Method = WebpEncodingMethod.BestQuality
                    };

                    await image.SaveAsWebpAsync(outputStream, encoder, ct);
                }

                outputStream.Position = 0;

                var imageUrl = await _storageService.UploadAsync(
                    outputStream,
                    path,
                    "image/webp",
                    true,
                    ct);

                return new ChapterImage
                {
                    ChapterId = newChapter.ChapterId,
                    ImageUrl = imageUrl,
                    OrderIndex = orderIndex
                };
            });

            var chapterImages = await Task.WhenAll(uploadTasks);
            await _chapterRepository.AddImagesAsync(chapterImages.ToList(), ct);
            story.UpdatedAt = DateTimeOffset.UtcNow;
            await _unitOfWork.SaveChangesAsync(ct);
            await _publishEndpoint.Publish(new UserExpActionEvent
            {
                UserId = user.UserId,
                Action = ExpActionType.CreateChapter
            }, ct);
            var useFollowers = await _userRepository.GetFollowersByStoryIdAsync(story.StoryId, ct);
            if(useFollowers != null)
            {
                var rawDataJson = new
                {
                    StoryTitle = story.Title,
                    ChapterNumber = command.ChapterNumber,
                    ChapterSlug = slug,
                    StorySlug = story.Slug
                };
                string metadataJson = JsonSerializer.Serialize(rawDataJson);
                await _publishEndpoint.Publish(new SendNotificationEvent
                {
                    ReceiverIds = useFollowers.Select(u => u.UserId).ToList(),
                    Type = Domain.Enums.Notification.NotificationType.NewChapter,
                    RawDataJson = metadataJson,
                    SenderId = 9 // System
                }, ct);
            }
            var response = new AddChapterResponse
            {
                ChapterId = newChapter.ChapterId,
                ChapterSlug = slug,
                StorySlug = story.Slug,
                ChapterNumber = command.ChapterNumber,
            };
            return response;
        }
    }
}
