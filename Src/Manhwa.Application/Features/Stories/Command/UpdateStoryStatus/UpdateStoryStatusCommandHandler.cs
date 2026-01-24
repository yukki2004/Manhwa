using Amazon.Runtime.Internal;
using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Enums.Story;
using Manhwa.Domain.Exceptions;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MassTransit.ValidationResultExtensions;

namespace Manhwa.Application.Features.Stories.Command.UpdateStoryStatus
{
    public class UpdateStoryStatusCommandHandler : IRequestHandler<UpdateStoryStatusCommand, UpdateStoryStatusResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoryRepository _storyRepository;
        private readonly IUserRepository _userRepository;
        public UpdateStoryStatusCommandHandler(IUnitOfWork unitOfWork, IStoryRepository storyRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _storyRepository = storyRepository;
            _userRepository = userRepository;
        }
        public async Task<UpdateStoryStatusResponse> Handle(UpdateStoryStatusCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if (user == null)
            {
                throw new NotFoundException("user", command.UserId);
            }
            if(user.IsActive == false)
            {
                throw new ForbiddenAccessException();
            }
            var story = await _storyRepository.GetByIdAsync(command.StoryId, ct);
            if(story == null)
            {
                throw new NotFoundException("story", command.StoryId);
            }
            if (story.UserId != command.UserId) throw new ForbiddenAccessException();
            story.Status = (StoryStatus)command.Status;
            await _unitOfWork.SaveChangesAsync(ct);
            return new UpdateStoryStatusResponse
            {
                Status = story.Status.ToString()
            };
        }
    }
}
