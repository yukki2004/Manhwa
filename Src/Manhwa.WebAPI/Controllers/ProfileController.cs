using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Manhwa.WebAPI.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
