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
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment?> GetByIdAsync(long id, CancellationToken ct = default)
        {
            return await _context.Set<Comment>()
                .FirstOrDefaultAsync(c => c.CommentId == id, ct);
        }

        public async Task AddAsync(Comment comment, CancellationToken ct = default)
        {
            await _context.Set<Comment>().AddAsync(comment, ct);
        }

        public void Update(Comment comment)
        {
            _context.Set<Comment>().Update(comment);
        }

        public async Task<bool> ExistsAsync(long id, CancellationToken ct = default)
        {
            return await _context.Set<Comment>()
                .AnyAsync(c => c.CommentId == id, ct);
        }
    }
}
