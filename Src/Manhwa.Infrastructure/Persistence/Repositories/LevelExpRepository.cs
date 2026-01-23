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
    public class LevelExpRepository : IlevelExpRepository
    {
        private readonly AppDbContext _context;

        public LevelExpRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetThresholdForLevelAsync(int level, CancellationToken ct = default)
        {
            return await _context.Set<LevelExp>()
                .Where(l => l.Level == level)
                .Select(l => l.ExpValue)
                .FirstOrDefaultAsync(ct);
        }
    }
}
