using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class StoryCategory
    {
        public long StoryId { get; set; }
        public int CategoryId { get; set; } 

        // Navigation properties
        public Story Story { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
