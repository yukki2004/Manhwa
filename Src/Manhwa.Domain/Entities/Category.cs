using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        // Navigation property 
        public ICollection<StoryCategory> StoryCategories { get; set; } = new List<StoryCategory>();
    }
}
