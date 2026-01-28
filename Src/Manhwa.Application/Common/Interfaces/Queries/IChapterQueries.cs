using Manhwa.Application.Features.Chapters.Queries.ViewChapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Queries
{
    public interface IChapterQueries
    {
        Task<ChapterViewDto?> GetChapterDetailAsync(string storySlug, string chapterSlug, CancellationToken ct);
    }
}
