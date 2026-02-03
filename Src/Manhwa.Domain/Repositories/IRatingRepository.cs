using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IRatingRepository
    {
        Task<Rating?> GetByUserIdAndStoryIdAsync(long userId, long storyId, CancellationToken ct);
        Task AddAsync(Rating rating, CancellationToken ct);
        void Update(Rating rating);
    }
}
