using Manhwa.Application.Common.Abstractions;
using Manhwa.Application.Features.Users.Management.Queries.GetAllUsers;
using Manhwa.Application.Features.Users.Management.Queries.GetUserLogs;
using Manhwa.Application.Features.Users.Profile.Queries.GetFavorites;
using Manhwa.Application.Features.Users.Profile.Queries.GetReadingHistory;
using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Queries
{
    public interface IUserQueries
    {
        Task<PagedResult<GetAllUsersDto>> GetAllUsersAsync(GetAllUsersQuery request, CancellationToken ct);
        Task<PagedResult<UserFavoriteDto>> GetPagedFavoritesWithChaptersAsync(
            long userId, int pageIndex, int pageSize, CancellationToken ct);
        Task<PagedResult<ReadingHistoryDto>> GetPagedReadingHistoryAsync(
            long userId, int pageIndex, int pageSize, CancellationToken ct);
        Task<PagedResult<UserLogResponse>> GetPagedUserLogsAsync(long userId, int pageIndex, int pageSize, UserLogAction? action, CancellationToken ct);
    }
}
