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
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;
        public RatingRepository(AppDbContext context) => _context = context;

        public async Task<Rating?> GetByUserIdAndStoryIdAsync(long userId, long storyId, CancellationToken ct)
            => await _context.Set<Rating>().FirstOrDefaultAsync(r => r.UserId == userId && r.StoryId == storyId, ct);

        public async Task AddAsync(Rating rating, CancellationToken ct)
            => await _context.Set<Rating>().AddAsync(rating, ct);

        public void Update(Rating rating) => _context.Set<Rating>().Update(rating);
    }
}
