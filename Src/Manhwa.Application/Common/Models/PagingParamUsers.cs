using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Models
{
    public class PagingParamUsers
    {
        private int _pageSize = 10;
        public int PageIndex { get; init; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 5000) ? 5000 : (value <= 0 ? 10 : value);
        }
    }
}
