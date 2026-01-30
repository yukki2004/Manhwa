using Manhwa.Application.Features.Interactions.Command.CreateComment;
using Manhwa.Application.Features.Interactions.Command.DeleteComment;
using Manhwa.Application.Features.Interactions.Command.FollowStory;
using Manhwa.Application.Features.Interactions.Command.PublishComment;
using Manhwa.Application.Features.Interactions.Command.Unfollow;
using Manhwa.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpPost("commnent")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var indentity = HttpContext.GetUserIdentity();
            var userRole = User.GetUserRole();
            if(userRole == null)
            {
                return Unauthorized();
            }
            var command = new CreateCommentCommand
            {
                UserId = (long)userId,
                UserRole = userRole,
                Identity = indentity,
                StoryId = request.StoryId,
                ParentId = request.ParentId,
                Content = request.Content,
                ChapterId = request.ChapterId,
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("comment/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(long id)
        {
            var userId = User.GetUserId();
            var userRole = User.GetUserRole();
            if(userId == null)
            {
                return Unauthorized();
            }
            if(userRole == null)
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(new DeleteCommentCommand
            {
                CommentId = id,
                UserId = (long)userId,
                UserRole = userRole
            });

            return Ok(result);
        }
        [HttpPatch("comment/{id}")]
        [Authorize]
        public async Task<IActionResult> PublishComment([FromRoute]   long id)
        {
            var userId = User.GetUserId();
            var userRole = User.GetUserRole();
            if (userId == null)
            {
                return Unauthorized();
            }
            if (userRole == null)
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(new PublishCommentCommand
            {
                CommentId = id,
                UserId = (long)userId,
                UserRole = userRole
            });

            return Ok(result);
        }
    }
}
