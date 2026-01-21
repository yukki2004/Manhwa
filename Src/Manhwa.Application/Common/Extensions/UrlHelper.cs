using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Extensions
{
    public static class UrlHelper
    {
        public static string? BaseUrl { get; set; }
        public static string ToFullUrl(this string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return $"{BaseUrl?.TrimEnd('/')}/uploads/avatars/default.png";

            if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return path;

            return $"{BaseUrl?.TrimEnd('/')}/{path.TrimStart('/')}";
        }
    }
}
