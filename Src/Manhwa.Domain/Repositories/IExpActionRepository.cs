using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IExpActionRepository
    {
        Task<ExpAction?> GetByActionTypeAsync(ExpActionType actionType, CancellationToken ct = default);
    }
}
