using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
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
    }
}
