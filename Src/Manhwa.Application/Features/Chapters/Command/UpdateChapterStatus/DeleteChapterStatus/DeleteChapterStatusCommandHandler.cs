using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapterStatus.DeleteChapterStatus
{
    public class DeleteChapterStatusCommandHandler : IRequestHandler<DeleteChapterStatusCommand, DeleteChapterStatusResponse>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteChapterStatusCommandHandler(IChapterRepository chapterRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _chapterRepository = chapterRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteChapterStatusResponse> Handle(DeleteChapterStatusCommand commad, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(commad.UserId);
            if (user == null)
            {
                throw new NotFoundException("user", commad.UserId);
            }
            if(user.IsActive == false)
            {
                throw new ForbiddenAccessException();
            }
            var chapter = await _chapterRepository.GetWithStoryByIdAsync(commad.ChapterId);
            if(chapter == null)
            {
                throw new NotFoundException("chapter", commad.ChapterId);
            }
            if(chapter.Story == null)
            {
                throw new NotFoundException("story", chapter.StoryId);
            }
            if (chapter.Story.UserId != commad.UserId && commad.UserRole != "Admin")
            {
                throw new ForbiddenAccessException();
            }
            if(chapter.Story.AdminLockStatus == Domain.Enums.AdminLockStatus.Locked && commad.UserRole != "Admin")
            {
                throw new BusinessRuleViolationException(
                    $"Không thể thao tác: Truyện đã bị khóa bởi Admin. Lý do: {chapter.Story.AdminNote}",
                    "STORY_IS_LOCKED");
            }
            if(chapter.Story.IsPublish == Domain.Enums.Story.StoryPublishStatus.Deleted && commad.UserRole != "Admin")
            {
                throw new BusinessRuleViolationException("Không thể thao tác trên chương của truyện đã bị xóa", "STORY_DELETED");
            }
            if(chapter.Status == Domain.Enums.Chapter.ChapterStatus.Deleted && commad.UserRole != "Admin")
            {
                throw new BusinessRuleViolationException("Không thể thao tác trên chương đã bị xóa", "CHAPTER_DELETED");
            }
            chapter.Status = Domain.Enums.Chapter.ChapterStatus.Deleted;
            chapter.UpdatedAt = DateTimeOffset.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new DeleteChapterStatusResponse
            {
                ChapterId = chapter.ChapterId,
                Status = chapter.Status.ToString(),
                Message = "Chapter status deleted successfully.",
                UpdatedAt = chapter.UpdatedAt
            };
        }

    }
}
