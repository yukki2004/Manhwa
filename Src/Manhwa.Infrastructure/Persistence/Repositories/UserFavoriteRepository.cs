using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Features.Users.Profile.Queries.GetFavorites;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Story;
using Manhwa.Domain.Repositories;
using Manhwa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Repositories
{
    public class UserFavoriteRepository : IUserFavoriteRepository
    {
        private readonly AppDbContext _context;
        public UserFavoriteRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(UserFavorite favorite, CancellationToken ct = default)
            => await _context.UserFavorites.AddAsync(favorite, ct);

        public void Remove(UserFavorite favorite)
            => _context.UserFavorites.Remove(favorite);

        public async Task<UserFavorite?> GetAsync(long userId, long storyId, CancellationToken ct = default)
            => await _context.UserFavorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.StoryId == storyId, ct);

        public async Task<bool> IsFavoriteAsync(long userId, long storyId, CancellationToken ct = default)
            => await _context.UserFavorites
                .AnyAsync(f => f.UserId == userId && f.StoryId == storyId, ct);

    }
}
