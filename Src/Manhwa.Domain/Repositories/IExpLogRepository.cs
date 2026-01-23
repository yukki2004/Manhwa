using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IExpLogRepository
    {
        Task AddAsync(ExpLog expLog, CancellationToken ct = default);
    }
}
