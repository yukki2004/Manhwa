using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IChapterRepository
    {
        Task AddAsync(Chapter chapter, CancellationToken ct = default);
        Task AddImagesAsync(IEnumerable<ChapterImage> images, CancellationToken ct = default);
        Task<Chapter?> GetWithImagesAsync(long chapterId, CancellationToken ct = default);
    }
}
