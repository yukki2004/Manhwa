using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
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

namespace Manhwa.Application.Features.Interactions.Command.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentResponse>
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStoryRepository _storyRepo;
        private readonly IChapterRepository _chapterRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(
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

        public async Task<DeleteCommentResponse> Handle(DeleteCommentCommand request, CancellationToken ct)
        {
            var user = await _userRepo.GetByIdAsync(request.UserId, ct)
                ?? throw new NotFoundException("User", request.UserId);

            if (!user.IsActive)
                throw new BusinessRuleViolationException("Tài khoản đã bị khóa", "USER_LOCK");

            var comment = await _commentRepo.GetByIdAsync(request.CommentId, ct)
                ?? throw new NotFoundException("Comment", request.CommentId);

            var story = await _storyRepo.GetByIdAsync(comment.StoryId, ct)
                ?? throw new NotFoundException("Story", comment.StoryId);

            bool isAdmin = request.UserRole == "Admin";
            bool isStoryOwner = story.UserId == request.UserId;
            bool isCommentOwner = comment.UserId == request.UserId;

            if (story.IsPublish == StoryPublishStatus.Deleted && !isAdmin)
                throw new BusinessRuleViolationException("Truyện đã bị xóa, chỉ Admin mới có quyền xử lý", "STORY_DELETED");

            if (story.IsPublish == StoryPublishStatus.Hidden && !isAdmin && !isStoryOwner)
                throw new BusinessRuleViolationException("Truyện bị ẩn, bạn không có quyền thực hiện", "STORY_HIDDEN");

            if (comment.ChapterId.HasValue)
            {
                var chapter = await _chapterRepo.GetByIdAsync(comment.ChapterId.Value, ct);
                if (chapter != null)
                {
                    if (chapter.Status == ChapterStatus.Deleted && !isAdmin)
                        throw new BusinessRuleViolationException("Chương bị xóa, chỉ Admin mới có quyền", "CHAPTER_DELETED");

                    if (chapter.Status == ChapterStatus.Hidden && !isAdmin && !isStoryOwner)
                        throw new BusinessRuleViolationException("Chương bị ẩn, bạn không có quyền", "CHAPTER_HIDDEN");
                }
            }

            if (!isAdmin && !isStoryOwner && !isCommentOwner)
            {
                throw new ForbiddenAccessException();
            }

            comment.Status = CommentStatus.Deleted; 
            comment.UpdatedAt = DateTimeOffset.UtcNow;

            _commentRepo.Update(comment);
            await _unitOfWork.SaveChangesAsync(ct);

            return new DeleteCommentResponse { Success = true, Message = "Xóa bình luận thành công" };
        }
    }
}
