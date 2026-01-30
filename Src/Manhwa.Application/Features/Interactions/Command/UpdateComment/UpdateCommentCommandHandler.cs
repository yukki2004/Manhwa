using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Enums.Chapter;
using Manhwa.Domain.Enums.Comment;
using Manhwa.Domain.Enums.Story;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, UpdateCommentResponse>
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStoryRepository _storyRepo;
        private readonly IChapterRepository _chapterRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommentCommandHandler(
            ICommentRepository commentRepo,
            IStoryRepository storyRepo,
            IChapterRepository chapterRepo,
            IUserRepository userRepo,
            IUnitOfWork unitOfWork)
        {
            _commentRepo = commentRepo;
            _storyRepo = storyRepo;
            _chapterRepo = chapterRepo;
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateCommentResponse> Handle(UpdateCommentCommand request, CancellationToken ct)
        {
            var user = await _userRepo.GetByIdAsync(request.UserId, ct)
                ?? throw new NotFoundException("User", request.UserId);

            if (!user.IsActive)
                throw new BusinessRuleViolationException("Tài khoản đã bị khóa", "USER_LOCK");

            bool isAdmin = request.UserRole == UserRole.Admin.ToString();

            var comment = await _commentRepo.GetByIdAsync(request.CommentId, ct)
                ?? throw new NotFoundException("Bình luận không tồn tại.", request.CommentId);

            if (comment.Status == CommentStatus.Deleted && !isAdmin)
                throw new ForbiddenAccessException();

            var story = await _storyRepo.GetByIdAsync(comment.StoryId, ct)
                ?? throw new NotFoundException("Truyện không tồn tại.", comment.StoryId);

            bool isStoryOwner = story.UserId == request.UserId;

            if (story.IsPublish == StoryPublishStatus.Deleted && !isAdmin)
                throw new BusinessRuleViolationException("Truyện đã bị xóa, chỉ Admin mới có quyền thao tác.", "STORY_DELETED");

            if (story.IsPublish == StoryPublishStatus.Hidden && !isAdmin && !isStoryOwner)
                throw new BusinessRuleViolationException("Truyện đang bị ẩn.", "STORY_HIDDEN");

            if (comment.ChapterId.HasValue)
            {
                var chapter = await _chapterRepo.GetByIdAsync(comment.ChapterId.Value, ct)
                    ?? throw new NotFoundException("Chương không tồn tại.", comment.ChapterId.Value);

                if (chapter.Status == ChapterStatus.Deleted && !isAdmin)
                    throw new BusinessRuleViolationException("Chương đã bị xóa.", "CHAPTER_DELETED");

                if (chapter.Status == ChapterStatus.Hidden && !isAdmin && !isStoryOwner)
                    throw new BusinessRuleViolationException("Chương đang bị ẩn.", "CHAPTER_HIDDEN");
            }

            if (comment.UserId != request.UserId && !isAdmin)
            {
                throw new ForbiddenAccessException();
            }

            comment.Content = request.Content;
            comment.UpdatedAt = DateTimeOffset.UtcNow;

            _commentRepo.Update(comment);
            await _unitOfWork.SaveChangesAsync(ct);

            return new UpdateCommentResponse
            {
                Success = true,
                Message = "Cập nhật bình luận thành công.",
                UpdatedAt = comment.UpdatedAt
            };
        }
    }
}
