using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class ExpLogRepository : IExpLogRepository
    {
        private readonly AppDbContext _context;
        public ExpLogRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ExpLog expLog, CancellationToken ct = default)
        {
            await _context.ExpLogs.AddAsync(expLog, ct);
        }
    }
}
