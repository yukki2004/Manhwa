using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string path, string contentType, bool isImmutable = false, CancellationToken ct = default);
        Task DeleteAsync(string key, CancellationToken ct = default);
    }
}
