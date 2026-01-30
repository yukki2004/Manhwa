using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHomeRankings
{
    public class HomeRankingsDto
    {
        public List<GetHomeRankingsStoryDto> Daily { get; set; } = new();
        public List<GetHomeRankingsStoryDto> Weekly { get; set; } = new();
        public List<GetHomeRankingsStoryDto> Monthly { get; set; } = new();
        public List<GetHomeRankingsStoryDto> AllTime { get; set; } = new();
    }
}
