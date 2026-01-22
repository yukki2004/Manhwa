using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Categories.Queries.GetCategories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Queries
{
    public class CategoryQueries : ICategoryQueries
    {
        private readonly AppDbContext _context;
        public CategoryQueries(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<GetCategoriesDto>> GetAllCategoriesAsync(CancellationToken ct)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Select(c => new GetCategoriesDto
                {
                    CategoryId = c.CategoryId,
                    Slug = c.Slug,
                    Name = c.Name,
                })
                .ToListAsync(ct);
            return categories;
        }
    }
}
