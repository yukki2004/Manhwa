using Manhwa.Application.Features.Interactions.Command.FollowStory;
using Manhwa.Application.Features.Interactions.Command.Unfollow;
using Manhwa.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manhwa.WebAPI.Controllers.Interactions
{
    [ApiController]
    [Route("api/interaction")]
    public class InteractionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InteractionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("follow/{storyId}")]
        [Authorize]
        public async Task<IActionResult> FollowStory([FromRoute] long storyId)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new FollowStoryCommand
            {
                StoryId = storyId,
                UserId = (long)userId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("unfollow/{storyId}")]
        [Authorize]
        public async Task<IActionResult> UnfollowStory([FromRoute] long storyId)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new UnfollowStoryCommand
            {
                StoryId = storyId,
                UserId = (long)userId,
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
