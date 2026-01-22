using Manhwa.Application.Features.Categories.Queries.GetCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Queries
{
    public interface ICategoryQueries
    {
        Task<List<GetCategoriesDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    }
}
