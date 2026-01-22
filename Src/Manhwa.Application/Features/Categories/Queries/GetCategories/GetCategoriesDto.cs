using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesDto
    {
        public int CategoryId { get; set; }
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
