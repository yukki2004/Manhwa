using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.MarkStoryHot
{
    public class MarkStoryHotCommandHandler : IRequestHandler<MarkStoryHotCommand, MarkStoryHotResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MarkStoryHotCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork)
        {
            _storyRepository = storyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MarkStoryHotResponse> Handle(MarkStoryHotCommand request, CancellationToken ct)
        {
            var story = await _storyRepository.GetByIdAsync(request.StoryId, ct);
            if (story == null) throw new Exception("Không tìm thấy truyện tương ứng.");
            story.IsHot = true;
            await _unitOfWork.SaveChangesAsync(ct);
            return new MarkStoryHotResponse
            {
                StoryId = story.StoryId,
                IsHot = story.IsHot,
                UpdatedAt = story.UpdatedAt,
                Message = "Truyện đã được đánh dấu là Hot thành công."
            };
        }
    }
}
