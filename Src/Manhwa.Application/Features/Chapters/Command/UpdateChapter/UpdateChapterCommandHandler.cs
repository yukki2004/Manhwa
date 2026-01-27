using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapter
{
    public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, UpdateChapterResponse>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IStorageService _storageService;
        public UpdateChapterCommandHandler(IChapterRepository chapterRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IStorageService storageService)
        {
            _chapterRepository = chapterRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _storageService = storageService;
        }
        public async Task<UpdateChapterResponse> Handle(UpdateChapterCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if (user == null)
            {
                throw new NotFoundException("User", command.UserId);
            }
            if (user.IsActive == false)
            {
                throw new ForbiddenAccessException();
            }
            var chapter = await _chapterRepository.GetWithStoryAndImagesByIdAsync(command.ChapterId);
            if(chapter == null)
            {
                throw new NotFoundException("Chapter", command.ChapterId);
            }
            if (chapter.Story == null)
            {
                throw new NotFoundException("Story", chapter.StoryId);
            }
            if (chapter.Story.UserId != command.UserId && command.UserRole != "Admin")
            {
                throw new ForbiddenAccessException();
            }
            if (chapter.Story.AdminLockStatus == Domain.Enums.AdminLockStatus.Locked && command.UserRole != "Admin")
            {
                throw new BusinessRuleViolationException(
                    $"Không thể thao tác: Truyện đã bị khóa bởi Admin. Lý do: {chapter.Story.AdminNote}",
                    "STORY_IS_LOCKED");
            }
            if (chapter.Story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Deleted && command.UserRole != "Admin")
            {
                throw new BusinessRuleViolationException("Không thể thao tác trên chương của truyện đã bị xóa", "STORY_DELETED");
            }
            if(chapter.Status == Domain.Enums.Chapter.ChapterStatus.Deleted && command.UserRole != "Admin")
            {
                throw new BusinessRuleViolationException("Không thể thao tác trên chương đã bị xóa", "CHAPTER_DELETED");
            }
            chapter.Title = command.Title;
            chapter.ChapterNumber = command.ChapterNumber;
            chapter.Slug = $"chuong-{command.ChapterNumber.ToString().Replace(".", "-")}";
            chapter.UpdatedAt = DateTimeOffset.UtcNow;
            if(command.Images != null && command.Images.Any())
            {
                var oldImages = chapter.ChapterImages.OrderBy(i => i.OrderIndex).ToList();
                var oldCount = oldImages.Count;
                var newFiles = command.Images.OrderBy(f => f.FileName).ToList();
                var newCount = newFiles.Count;
                var storageFolder = $"stories/{chapter.StoryId}/chapters/{chapter.ChapterId}/images";
                var uploadTasks = newFiles.Select(async (file, index) =>
                {
                    var orderIndex = index + 1;
                    var path = $"{storageFolder}/{orderIndex:D3}.webp";

                    using var inputStream = file.OpenReadStream();
                    using var outputStream = new MemoryStream();

                    using (var image = await Image.LoadAsync(inputStream, ct))
                    {
                        var encoder = new WebpEncoder { Quality = 75, Method = WebpEncodingMethod.BestQuality };
                        await image.SaveAsWebpAsync(outputStream, encoder, ct);
                    }
                    outputStream.Position = 0;

                    var imageUrl = await _storageService.UploadAsync(outputStream, path, "image/webp", false, ct);

                    return new ChapterImage
                    {
                        ChapterId = chapter.ChapterId,
                        ImageUrl = imageUrl,
                        OrderIndex = orderIndex
                    };
                });

                var uploadedImages = await Task.WhenAll(uploadTasks);

                if (newCount < oldCount)
                {
                    var deleteTasks = oldImages
                        .Where(i => i.OrderIndex > newCount)
                        .Select(async i => {
                            var path = $"{storageFolder}/{i.OrderIndex:D3}.webp";
                            await _storageService.DeleteAsync(path, ct);
                        });
                    await Task.WhenAll(deleteTasks);
                }

                await _chapterRepository.RemoveAllImagesAsync(chapter.ChapterId, ct);
                await _chapterRepository.AddImagesAsync(uploadedImages.ToList(), ct);
            }
            await _unitOfWork.SaveChangesAsync(ct);
            return new UpdateChapterResponse
            {
                ChapterId = chapter.ChapterId,
                Message = "Cập nhật chương và tối ưu hình ảnh thành công.",
                UpdatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
