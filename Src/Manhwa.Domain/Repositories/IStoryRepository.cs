using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IStoryRepository
    {
        Task AddAsync(Story story, CancellationToken ct = default);
        Task<Story?> GetByIdAsync(long id, CancellationToken ct = default);
        Task<Story?> GetBySlugAsync(string slug, CancellationToken ct = default);

    }
}
