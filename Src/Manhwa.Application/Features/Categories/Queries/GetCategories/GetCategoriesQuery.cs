using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<List<GetCategoriesDto>>
    {
    }
}
