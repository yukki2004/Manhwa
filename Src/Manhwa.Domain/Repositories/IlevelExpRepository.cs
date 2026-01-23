using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Repositories
{
    public interface IlevelExpRepository
    {
        Task<int> GetThresholdForLevelAsync(int level, CancellationToken ct = default);
    }
}
