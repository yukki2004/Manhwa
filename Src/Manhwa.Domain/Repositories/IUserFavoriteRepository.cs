using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IUserFavoriteRepository
    {
        Task AddAsync(UserFavorite favorite, CancellationToken ct = default);
        void Remove(UserFavorite favorite);
        Task<UserFavorite?> GetAsync(long userId, long storyId, CancellationToken ct = default);
        Task<bool> IsFavoriteAsync(long userId, long storyId, CancellationToken ct = default);

    }
}

