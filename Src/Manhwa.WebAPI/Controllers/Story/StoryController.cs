using Manhwa.Application.Features.Stories.Command.AdminModeration.LockStory;
using Manhwa.Application.Features.Stories.Command.AdminModeration.UnLockStory;
using Manhwa.Application.Features.Stories.Command.ChangePublishState;
using Manhwa.Application.Features.Stories.Command.ChangePublishState.DeleteStory;
using Manhwa.Application.Features.Stories.Command.ChangePublishState.HideStory;
using Manhwa.Application.Features.Stories.Command.ChangePublishState.PublishStory;
using Manhwa.Application.Features.Stories.Command.CreateStory;
using Manhwa.Application.Features.Stories.Command.UpdateStory;
using Manhwa.Application.Features.Stories.Command.UpdateStoryAvatar;
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
        [HttpPatch("{storyId}/lock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LockStory([FromRoute] long storyId, [FromBody] LockStoryRequest request)
        {
            var command = new LockStoryCommand
            {
                StoryId = storyId,
                AdminNote = request.AdminNote
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{storyId}/unlock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnLockStory([FromRoute] long storyId, [FromBody] UnlockStoryRequest request)
        {
            var command = new UnLockStoryCommand
            {
                StoryId = storyId,
                AdminNote = request.AdminNote
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{storyId}/avatar")]
        [Authorize]
        public async Task<IActionResult> UpdateStoryAvatar([FromRoute] long storyId, [FromForm] UpdateStoryAvatarRequest request)
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
            var command = new UpdateStoryAvatarCommand
            {
                StoryId = storyId,
                UserId = (long)userId,
                UserRole = userRole,
                ThumbnailFile = request.ThumbnailFile
            };
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPut("{storyId}")]
        [Authorize]
        public async Task<IActionResult> UpdateStory([FromRoute] long storyId, [FromBody] UpdateStoryRequest request)
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
            var command = new UpdateStoryCommand
            {
                StoryId = storyId,
                UserId = (long)userId,
                UserRole = userRole,
                Title = request.Title,
                Description = request.Description,
                ReleaseYear = request.ReleaseYear,
                AuthorName = request.AuthorName,
                CategoryIds = request.CategoryIds
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }


    }
}
