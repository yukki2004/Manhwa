using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Enums.Chapter;
using Manhwa.Domain.Enums.Comment;
using Manhwa.Domain.Enums.Story;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Interactions.Command.PublishComment
{
    public class PublishCommentCommandHandler : IRequestHandler<PublishCommentCommand, PublishCommentResponse>
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStoryRepository _storyRepo;
        private readonly IChapterRepository _chapterRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PublishCommentCommandHandler(
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

        public async Task<PublishCommentResponse> Handle(PublishCommentCommand request, CancellationToken ct)
        {
            var user = await _userRepo.GetByIdAsync(request.UserId, ct)
                ?? throw new NotFoundException("User", request.UserId);

            if (!user.IsActive)
                throw new BusinessRuleViolationException("Tài khoản đã bị khóa", "USER_LOCK");

            var comment = await _commentRepo.GetByIdAsync(request.CommentId, ct)
                ?? throw new NotFoundException("Comment", request.CommentId);


            if (comment.Status == CommentStatus.Published)
            {
                throw new BusinessRuleViolationException("Bình luận này đã được hiển thị rồi.", "COMMENT_ALREADY_PUBLISHED");
            }

            bool isAdmin = request.UserRole == UserRole.Admin.ToString();

            if (comment.Status == CommentStatus.Deleted && !isAdmin)
            {
                throw new ForbiddenAccessException();
            }

            var story = await _storyRepo.GetByIdAsync(comment.StoryId, ct)
                ?? throw new NotFoundException("Story", comment.StoryId);

            bool isStoryOwner = story.UserId == request.UserId;
            bool isCommentOwner = comment.UserId == request.UserId;

            if (story.IsPublish == StoryPublishStatus.Deleted && !isAdmin)
                throw new BusinessRuleViolationException("Truyện đã bị xóa, không thể thao tác bình luận", "STORY_DELETED");

            if (story.IsPublish == StoryPublishStatus.Hidden && !isAdmin && !isStoryOwner)
                throw new BusinessRuleViolationException("Truyện đang bị ẩn", "STORY_HIDDEN");

            if (comment.ChapterId.HasValue)
            {
                var chapter = await _chapterRepo.GetByIdAsync(comment.ChapterId.Value, ct);
                if (chapter != null)
                {
                    if (chapter.Status == ChapterStatus.Deleted && !isAdmin)
                        throw new BusinessRuleViolationException("Chương bị xóa, không thể thao tác", "CHAPTER_DELETED");

                    if (chapter.Status == ChapterStatus.Hidden && !isAdmin && !isStoryOwner)
                        throw new BusinessRuleViolationException("Chương đang ẩn", "CHAPTER_HIDDEN");
                }
            }

            if (!isAdmin && !isStoryOwner && !isCommentOwner)
            {
                throw new ForbiddenAccessException();
            }
            comment.Status = CommentStatus.Published;
            comment.UpdatedAt = DateTimeOffset.UtcNow;

            _commentRepo.Update(comment);
            await _unitOfWork.SaveChangesAsync(ct);

            return new PublishCommentResponse
            {
                Success = true,
                Message = "Khôi phục bình luận thành công."
            };
        }

    }
}