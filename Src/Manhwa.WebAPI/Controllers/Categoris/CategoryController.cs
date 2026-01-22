using Manhwa.Application.Features.Categories.Queries.GetCategories;
using Microsoft.AspNetCore.Mvc;
using MediatR;
namespace Manhwa.WebAPI.Controllers.Categoris
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _mediator.Send(new Manhwa.Application.Features.Categories.Queries.GetCategories.GetCategoriesQuery());
            return Ok(result);
        }
    }
}
