using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Command.AdminModeration.UnmarkStoryHot
{
    public class UnmarkStoryHotCommandHandler : IRequestHandler<UnmarkStoryHotCommand, UnmarkStoryHotResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UnmarkStoryHotCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork)
        {
            _storyRepository = storyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnmarkStoryHotResponse> Handle(UnmarkStoryHotCommand request, CancellationToken ct)
        {
            var story = await _storyRepository.GetByIdAsync(request.StoryId, ct);
            if (story == null) throw new Exception("Không tìm thấy truyện tương ứng.");

            story.IsHot = false;
            await _unitOfWork.SaveChangesAsync(ct);
            return new UnmarkStoryHotResponse
            {
                StoryId = story.StoryId,
                IsHot = story.IsHot,
                UpdatedAt = story.UpdatedAt,
                Message = "Đã hủy trạng thái Hot của truyện thành công."
            };
        }
    }
}
