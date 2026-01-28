using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Common.Messaging;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Chapter;
using Manhwa.Domain.Enums.Story;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Queries.ViewChapter
{
    public class ViewChapterCommandHandler : IRequestHandler<ViewChapterCommand, ChapterViewDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IChapterQueries _chapterQueries;
        private readonly IChapterRepository _chapterRepository;
        public ViewChapterCommandHandler(IUserRepository userRepository, IPublishEndpoint publishEndpoint, IChapterQueries chapterQueries, IChapterRepository chapterRepository)
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _chapterQueries = chapterQueries;
            _chapterRepository = chapterRepository;
        }
        public async Task<ChapterViewDto> Handle(ViewChapterCommand command, CancellationToken ct)
        {
            var response = await _chapterQueries.GetChapterDetailAsync(command.StorySlug, command.ChapterSlugs, ct);
            if (response == null) throw new NotFoundException("Chapter", command.ChapterSlugs);

            if (command.Identity.StartsWith("u_") && command.UserId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(command.UserId.Value);
                if (user == null || !user.IsActive)
                {
                    throw new BusinessRuleViolationException("Tài khoản đã bị khóa hoặc không tồn tại", "USER_LOCKED");
                }
            }

            var chapter = await _chapterRepository.GetWithStoryByIdAsync(response.ChapterId);
            if (chapter == null) throw new NotFoundException("Chapter", command.ChapterSlugs);

            bool isAdmin = command.UserRole == "Admin";
            bool isOwner = command.UserId == chapter.Story.UserId;

            if (!isAdmin && (chapter.Status == ChapterStatus.Deleted || chapter.Story.IsPublish == StoryPublishStatus.Deleted))
            {
                throw new BusinessRuleViolationException("Nội dung này đã bị xóa", "CONTENT_DELETED");
            }

            if (!isAdmin && !isOwner)
            {
                if (chapter.Status == ChapterStatus.Hidden || chapter.Story.IsPublish == StoryPublishStatus.Hidden)
                {
                    throw new BusinessRuleViolationException("Nội dung này hiện đang bị ẩn", "CONTENT_HIDDEN");
                }
            }

            await _publishEndpoint.Publish(new StoryInteractionEvent
            {
                StoryId = response.StoryId,
                ChapterId = response.ChapterId,
                Identity = command.Identity,
                ActionType = InteractionType.View,
            }, ct);

            if (command.Identity.StartsWith("u_"))
            {
                await _publishEndpoint.Publish(new UserExpActionEvent
                {
                    UserId = long.Parse(command.Identity.Replace("u_", "")),
                    Action = ExpActionType.ViewChapter,
                }, ct);
            }

            return response;
        }

    }
}
