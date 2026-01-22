using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Common.Extensions;
using Manhwa.Application.Common.Interfaces.Queries;
using Manhwa.Application.Features.Users.Management.Queries.GetAllUsers;
using Manhwa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly AppDbContext _context;
        public UserQueries(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<GetAllUsersDto>> GetAllUsersAsync(GetAllUsersQuery request, CancellationToken ct)
        {
            var query = _context.Users.AsNoTracking();
            if (request.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == request.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.ToLower();
                query = query.Where(u => u.Username.ToLower().Contains(search) ||
                                         u.Email.ToLower().Contains(search));
            }

            var projection = query.Select(u => new GetAllUsersDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                Avatar = u.Avatar.ToFullUrl(),
                Description = u.Description,
                IsActive = u.IsActive,
                loginType = u.LoginType,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            });

            return await projection
                .OrderByDescending(x => x.CreatedAt)
                .ToPagedListAsync(request.PageIndex, request.PageSize, ct);
        }
    
    }
}
