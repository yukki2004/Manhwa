using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) => _context = context;

        public async Task AddStoryCategoriesAsync(IEnumerable<StoryCategory> storyCategories, CancellationToken ct = default)
        {
            await _context.Set<StoryCategory>().AddRangeAsync(storyCategories, ct);
        }
    }
}
