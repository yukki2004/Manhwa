using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(long id, CancellationToken ct = default);
        Task AddAsync(Comment comment, CancellationToken ct = default);
        void Update(Comment comment);
        Task<bool> ExistsAsync(long id, CancellationToken ct = default);
    }
}
