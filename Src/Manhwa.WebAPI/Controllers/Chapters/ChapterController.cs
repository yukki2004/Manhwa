using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Manhwa.Application.Features.Chapters.Command.AddChapter;
using Manhwa.WebAPI.Extensions;
namespace Manhwa.WebAPI.Controllers.Chapters
{
    [ApiController]
    [Route("api/story")]
    public class ChapterController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChapterController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("{storyId}/chapter")]
        [Authorize]
        public async Task<IActionResult> CreateChapter([FromRoute] long storyId, [FromForm] AddChapterRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if(userRole == null)
            {
                return Forbid();
            }
            var command = new AddChapterCommand
            {
                StoryId = storyId,
                Title = request.Title,
                UserId = (long)userId,
                Images = request.Images,
                UserRole = userRole,
                ChapterNumber = request.ChapterNumber
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
