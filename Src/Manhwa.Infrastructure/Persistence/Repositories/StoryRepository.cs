using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public StoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddAsync(Story story, CancellationToken ct)
        {
            await _appDbContext.Stories.AddAsync(story, ct);
        }
        public async Task<Story?> GetByIdAsync(long storyId, CancellationToken ct)
        {
            return await _appDbContext.Stories.FindAsync(new object[] { storyId }, ct);
        }
        public async Task<Story?> GetBySlugAsync(string slug, CancellationToken ct)
        {
            return await _appDbContext.Stories.FirstOrDefaultAsync(s => s.Slug == slug, ct);
        }
        public async Task<Story?> GetByIdWithCategoriesAsync(long id, CancellationToken ct)
        {
            return await _appDbContext.Stories
                .Include(s => s.StoryCategories)
                .ThenInclude(sc => sc.Category)
                .FirstOrDefaultAsync(s => s.StoryId == id, ct);
        }
        public async Task<List<Story>> GetActiveStoriesByIdsAsync(List<long> ids, CancellationToken ct = default)
        {
            return await _appDbContext.Set<Story>()
                .Where(s => ids.Contains(s.StoryId)
                         && s.IsPublish == Domain.Enums.Story.StoryPublishStatus.Published)          
                .AsNoTracking()                    
                .ToListAsync(ct);
        }
    }
}
