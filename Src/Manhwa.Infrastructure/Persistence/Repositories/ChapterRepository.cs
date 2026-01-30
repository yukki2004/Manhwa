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
    public class ChapterRepository : IChapterRepository
    {
        private readonly AppDbContext _context;
        public ChapterRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Chapter chapter, CancellationToken ct = default)
            => await _context.Chapters.AddAsync(chapter, ct);

        public async Task AddImagesAsync(IEnumerable<ChapterImage> images, CancellationToken ct = default)
            => await _context.ChapterImages.AddRangeAsync(images, ct);
        public async Task RemoveAllImagesAsync(long chapterId, CancellationToken ct = default)
        {
            await _context.ChapterImages
                .Where(ci => ci.ChapterId == chapterId)
                .ExecuteDeleteAsync(ct);
        }
        public async Task<Chapter?> GetWithImagesAsync(long chapterId, CancellationToken ct = default)
            => await _context.Chapters
                .Include(c => c.ChapterImages.OrderBy(i => i.OrderIndex))
                .FirstOrDefaultAsync(c => c.ChapterId == chapterId, ct);
        public async Task<Chapter?> GetWithStoryByIdAsync(long chapterId, CancellationToken ct = default)
        {
            return await _context.Chapters
                .Include(c => c.Story)
                .FirstOrDefaultAsync(c => c.ChapterId == chapterId, ct);
        }
        public async Task<Chapter?> GetWithStoryAndImagesByIdAsync(long id, CancellationToken ct = default)
        {
            return await _context.Chapters
                .Include(c => c.Story)       
                .Include(c => c.ChapterImages) 
                .FirstOrDefaultAsync(c => c.ChapterId == id, ct);
        }
        public async Task<Chapter?> GetByIdAsync(long id, CancellationToken ct)
        {
            return await _context.Chapters.FindAsync(id, ct);
        }
    }
}
