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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) => _context = context;

        public async Task<User?> GetByIdAsync(long id, CancellationToken ct)
            => await _context.Users.FindAsync(id, ct);

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct)
            => await _context.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
        public async Task<User?> GetByIdentifierAsync(string identifier, CancellationToken ct)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == identifier || u.Email == identifier, ct);
        }
        public async Task AddAsync(User user, CancellationToken ct)
            => await _context.Users.AddAsync(user, ct);

        public void Update(User user)
            => _context.Users.Update(user);

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
            => await _context.Users.AnyAsync(u => u.Email == email, ct);

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct)
            => await _context.Users.AnyAsync(u => u.Username == username, ct);
        public async Task<List<User>> GetFollowersByStoryIdAsync(long storyId, CancellationToken ct = default)
        {
            return await _context.UserFavorites 
                .Where(f => f.StoryId == storyId)
                .Select(f => f.User) 
                .AsNoTracking() 
                .ToListAsync(ct);
        }
    }
}
