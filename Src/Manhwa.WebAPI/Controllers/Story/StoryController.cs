using Manhwa.Application.Features.Stories.Command.ChangePublishState;
using Manhwa.Application.Features.Stories.Command.CreateStory;
using Manhwa.Application.Features.Stories.Command.UpdateStoryStatus;
using Manhwa.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manhwa.WebAPI.Controllers.Story
{
    [ApiController]
    [Route("api/story")]
    public class StoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("post-story")]
        [Authorize]
        public async Task<IActionResult> PostStory([FromForm] CreateStoryRequest request)
        {
            var userId = User.GetUserId();
            if(userId == null)
            {
                return Unauthorized();
            }
            var command = new CreateStoryCommand
            {
                Title = request.Title,
                Description = request.Description,
                ThumbnailFile = request.ThumbnailFile,
                ReleaseYear = request.ReleaseYear,
                Author = request.Author,
                UserId = (long)userId,
                GenreIds = request.GenreIds
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{storyId}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStoryStatus([FromRoute] long storyId, [FromBody] UpdateStoryStatusRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new UpdateStoryStatusCommand
            {
                StoryId = storyId,
                Status = request.Status,
                UserId = (long)userId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{storyId}/publish-state")]
        [Authorize]
        public async Task<IActionResult> ChangePublishState([FromRoute] long storyId, [FromBody] ChangePublishStateRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new ChangePublishStateCommand
            {
                StoryId = storyId,
                IsPublished = request.IsPublished,
                UserId = (long)userId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
