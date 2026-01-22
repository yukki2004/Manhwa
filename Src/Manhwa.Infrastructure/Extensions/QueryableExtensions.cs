using Manhwa.Application.Common.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize,
        CancellationToken ct = default)
        {
            var count = await source.CountAsync(ct);
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            return new PagedResult<T>(items, count, pageIndex, pageSize);
        }
    }
}
