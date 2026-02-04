using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetStoryDetail
{
    public class CategoryStoryDetailDto
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}
