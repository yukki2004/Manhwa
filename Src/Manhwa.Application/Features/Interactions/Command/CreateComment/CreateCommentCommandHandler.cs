using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Messaging;
using Manhwa.Application.Features.Interactions.Command.CreateComment;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Enums.Chapter;
using Manhwa.Domain.Enums.Notification;
using Manhwa.Domain.Enums.Story;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System.Text.Json;

namespace Manhwa.Application.Features.Interactions.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CreateCommentResponse>
{
    private readonly ICommentRepository _commentRepo;
    private readonly IUserRepository _userRepo;
    private readonly IStoryRepository _storyRepo;
    private readonly IChapterRepository _chapterRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateCommentCommandHandler(
        ICommentRepository commentRepo,
        IUserRepository userRepo,
        IStoryRepository storyRepo,
        IChapterRepository chapterRepo,
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _commentRepo = commentRepo;
        _userRepo = userRepo;
        _storyRepo = storyRepo;
        _chapterRepo = chapterRepo;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CreateCommentResponse> Handle(CreateCommentCommand request, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, ct)
            ?? throw new NotFoundException("User", request.UserId);

        if (!user.IsActive)
            throw new BusinessRuleViolationException("Tài khoản đã bị khóa", "USER_LOCK");

        bool isAdmin = request.UserRole == UserRole.Admin.ToString();

        var story = await _storyRepo.GetByIdAsync(request.StoryId, ct)
            ?? throw new NotFoundException("Story", request.StoryId);

        bool isStoryOwner = story.UserId == request.UserId;

        ValidateStoryStatus(story, isAdmin, isStoryOwner);

        if (request.ChapterId.HasValue)
        {
            var chapter = await _chapterRepo.GetByIdAsync(request.ChapterId.Value, ct)
                ?? throw new NotFoundException("Chapter", request.ChapterId.Value);

            ValidateChapterStatus(chapter, isAdmin, isStoryOwner);
        }

        Comment? parentComment = null;
        if (request.ParentId.HasValue)
        {
            parentComment = await _commentRepo.GetByIdAsync(request.ParentId.Value, ct)
                ?? throw new NotFoundException("Parent Comment", request.ParentId.Value);
        }

        var comment = new Comment
        {
            Content = request.Content,
            UserId = request.UserId,
            StoryId = request.StoryId,
            ChapterId = request.ChapterId,
            ParentId = request.ParentId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _commentRepo.AddAsync(comment, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        await PublishEventsAsync(request, user, story, parentComment, ct);

        return new CreateCommentResponse
        {
            CommentId = comment.CommentId,
            Message = "Bình luận thành công",
            CreatedAt = comment.CreatedAt
        };
    }

    private void ValidateStoryStatus(Story story, bool isAdmin, bool isOwner)
    {
        if (isAdmin) return;

        if (story.IsPublish == StoryPublishStatus.Deleted)
            throw new BusinessRuleViolationException("Truyện đã bị xóa", "STORY_DELETED");

        if (story.IsPublish == StoryPublishStatus.Hidden && !isOwner)
            throw new BusinessRuleViolationException("Truyện đang ẩn", "STORY_HIDDEN");
    }

    private void ValidateChapterStatus(Chapter chapter, bool isAdmin, bool isOwner)
    {
        if (isAdmin) return;

        if (chapter.Status == ChapterStatus.Deleted)
            throw new BusinessRuleViolationException("Chương đã bị xóa", "CHAPTER_DELETED");

        if (chapter.Status == ChapterStatus.Hidden && !isOwner)
            throw new BusinessRuleViolationException("Chương đang ẩn", "CHAPTER_HIDDEN");
    }

    private async Task PublishEventsAsync(CreateCommentCommand request, User user, Story story, Comment? parent, CancellationToken ct)
    {
        await _publishEndpoint.Publish(new UserExpActionEvent { UserId = request.UserId, Action = ExpActionType.Comment }, ct);
        await _publishEndpoint.Publish(new StoryInteractionEvent { StoryId = request.StoryId, ActionType = InteractionType.Comment, Identity = request.Identity }, ct);

        var metadata = new
        {
            replierName = user.Username,
            commenterName = user.Username,
            storyTitle = story.Title,
            slug = story.Slug,
            avatar = user.Avatar.ToFullUrl(),
            avtReplier = user.Avatar.ToFullUrl()
        };
        string rawJson = JsonSerializer.Serialize(metadata);

        if (parent != null && parent.UserId != request.UserId && parent.UserId != null)
        {
            await _publishEndpoint.Publish(new SendNotificationEvent
            {
                SenderId = request.UserId,
                ReceiverIds = new List<long> { (long)parent.UserId },
                Type = NotificationType.CommentReply,
                RawDataJson = rawJson
            }, ct);
        }

        if (story.UserId.HasValue && story.UserId != request.UserId)
        {
            await _publishEndpoint.Publish(new SendNotificationEvent
            {
                SenderId = request.UserId,
                ReceiverIds = new List<long> { story.UserId.Value },
                Type = NotificationType.NewComment,
                RawDataJson = rawJson
            }, ct);
            
        }
    }
}