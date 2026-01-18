using Manhwa.Domain.Entities;
using Manhwa.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class UserLogRepository : IUserLogRepository
    {
        private readonly AppDbContext _context;
        public UserLogRepository(AppDbContext context) => _context = context;
        public async Task AddAsync(UserLog userLog, CancellationToken cancellationToken = default)
        {
             await _context.UserLogs.AddAsync(userLog, cancellationToken);
        }
    }
}
