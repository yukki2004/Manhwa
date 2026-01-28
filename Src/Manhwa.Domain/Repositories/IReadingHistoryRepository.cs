using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IReadingHistoryRepository
    {
        Task UpsertHistoryAsync(long userId, long storyId, long chapterId);
    }
}
