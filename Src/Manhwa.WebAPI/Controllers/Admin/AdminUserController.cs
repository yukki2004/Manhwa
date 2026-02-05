using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus;
using Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.ActiveUser;
using Manhwa.Application.Features.Users.Management.Command.UpdateUserStatus.LockUser;
using Manhwa.Application.Features.Users.Management.Queries.GetAllUsers;
using Manhwa.Application.Features.Users.Management.Queries.GetUserExpLogs;
using Manhwa.Application.Features.Users.Management.Queries.GetUserLogs;
using Manhwa.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Manhwa.WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminUserController(IMediator mediator) => _mediator = mediator;
        [HttpPatch("{userId}/lock")]
        public async Task<IActionResult> LockUser([FromRoute] long userId)
        {
            var command = new LockUserCommand
            {
                UserId = userId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("{userId}/active")]
        public async Task<IActionResult> ActiveUser([FromRoute] long userId)
        {
            var command = new ActiveUserCommand
            {
                UserId = userId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("get-users")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{id}/logs")]
        public async Task<IActionResult> GetLogs( [FromRoute] long id, [FromQuery] UserLogRequest request)
        {
            var query = new GetUserLogsQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Action = request.Action,
                UserId = id
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet("{id}/exp-logs")]
        public async Task<IActionResult> GetExpLogs([FromRoute] long id, [FromQuery] ExpUserLogRequest request)
        {
            var query = new GetUserExpLogsQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                UserId = id,
                Action = request.Action,
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
