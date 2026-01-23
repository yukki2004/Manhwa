using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class ExpActionRepository : IExpActionRepository
    {
        private readonly AppDbContext _context;

        public ExpActionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ExpAction?> GetByActionTypeAsync(ExpActionType actionType, CancellationToken ct = default)
        {
            return await _context.Set<ExpAction>()
                .FirstOrDefaultAsync(a => a.IdAct == (int)actionType, ct);
        }
    }
}
