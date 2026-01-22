using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Interfaces.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<GetCategoriesDto>>
    {
        private readonly ICategoryQueries _categoryQueries;
        private readonly ICacheService _cacheService;
        public GetCategoriesHandler(ICategoryQueries categoryQueries, ICacheService cacheService)
        {
            _categoryQueries = categoryQueries;
            _cacheService = cacheService;
        }
        public async Task<List<GetCategoriesDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "GetCategories:All";
            var cachedCategories = await _cacheService.GetAsync<List<GetCategoriesDto>>(cacheKey);
            if (cachedCategories != null)
            {
                return cachedCategories;
            }
            var categories = await _categoryQueries.GetAllCategoriesAsync();
            await _cacheService.SetAsync(cacheKey, categories, TimeSpan.FromHours(5));
            return categories;
        }

    }
}
