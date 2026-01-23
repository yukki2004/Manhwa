using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Extensions
{
    public static class StoryExtensions
    {

        public static string GenerateSlugId(this Story story)
        {
            if (story == null) return string.Empty;
            if (story.StoryId <= 0) return story.Slug;
            return $"{story.Slug}-{story.StoryId}";
        }
    }
}
