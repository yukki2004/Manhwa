using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(long id, CancellationToken ct = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<User?> GetByIdentifierAsync(string identifier, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        void Update(User user); 

        // Kiểm tra tồn tại (Dùng cho Validator)
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
        Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct = default);
        Task<List<User>> GetFollowersByStoryIdAsync(long storyId, CancellationToken ct = default);
    }
}
