using Manhwa.Application.Features.Stories.Command.AdminModeration.ModerateWithStatus;
using Manhwa.Application.Features.Stories.Command.ChangePublishState;
using Manhwa.Application.Features.Stories.Command.ChangePublishState.DeleteStory;
using Manhwa.Application.Features.Stories.Command.ChangePublishState.HideStory;
using Manhwa.Application.Features.Stories.Command.ChangePublishState.PublishStory;
using Manhwa.Application.Features.Stories.Command.CreateStory;
using Manhwa.Application.Features.Stories.Command.UpdateStoryStatus.CompleteStory;
using Manhwa.Application.Features.Stories.Command.UpdateStoryStatus.DropStory;
using Manhwa.Application.Features.Stories.Command.UpdateStoryStatus.OngingStory;
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
        [HttpPatch("{storyId}/publish")]
        [Authorize]
        public async Task<IActionResult> PublishStory([FromRoute] long storyId)
        {
            var userId = User.GetUserId();
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userId == null || userRole == null)
            {
                return Unauthorized();
            }
            var command = new PublishStoryCommand
            {
                StoryId = storyId,
                UserId = (long)userId,
                UserRole = userRole
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{storyId}/hidden")]
        [Authorize]
        public async Task<IActionResult> HiddenStory([FromRoute] long storyId)
        {
            var userId = User.GetUserId();
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userId == null || userRole == null)
            {
                return Unauthorized();
            }
            var command = new HideStoryCommand
            {
                StoryId = storyId,
                UserId = (long)userId,
                UserRole = userRole
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{storyId}")]
        [Authorize]
        public async Task<IActionResult> DeleteStory([FromRoute] long storyId)
        {
            var userId = User.GetUserId();
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userId == null || userRole == null)
            {
                return Unauthorized();
            }
            var command = new DeleteStoryCommand
            {
                StoryId = storyId,
                UserId = (long)userId,
                UserRole = userRole
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{storyId}/ongoing")]
        [Authorize]
        public async Task<IActionResult> ModerateStoryToOngoing([FromRoute] long storyId)
        {
            var userId = User.GetUserId();
            if(userId == null)
            {
                return Unauthorized();
            }
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == null)
            {
                return Unauthorized();
            }
            var command = new OngoingStoryCommand
            {
                StoryId = storyId,
                UserRole = userRole,
                UserId = (long)userId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{storyId}/drop")]
        [Authorize]
        public async Task<IActionResult> ModerateStoryToDrop([FromRoute] long storyId)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == null)
            {
                return Unauthorized();
            }
            var command = new DropStoryCommand
            {
                StoryId = storyId,
                UserRole = userRole,
                UserId = (long)userId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{storyId}/complete")]
        [Authorize]
        public async Task<IActionResult> ModerateStoryToComplete([FromRoute] long storyId)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == null)
            {
                return Unauthorized();
            }
            var command = new CompleteStoryCommand
            {
                StoryId = storyId,
                UserRole = userRole,
                UserId = (long)userId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }


    }
}
