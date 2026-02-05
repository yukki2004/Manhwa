using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Reports.Command.CreateReport;
using Manhwa.Application.Features.Reports.Queries.GetReport;
using Manhwa.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manhwa.WebAPI.Controllers.Reports
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostReport([FromBody] CreateReportRequest request)
        {
            var userId = User.GetUserId();
            if(userId == null)
            {
                return Unauthorized();
            }
            var command = new CreateReportCommand
            {
                TargetId = request.TargetId,
                UserId = (long)userId,
                TargetType = request.TargetType,
                Reason = request.Reason,
            };
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetAdminReports([FromQuery] GetReportsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
