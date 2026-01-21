using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetUserProfile
{
    public class GetUserProfileResponse
    {
        public long UserId { get; set; }
        public string UserName { get; set; } = null!;
        public short Level { get; set; }
        public int CurrentExp { get; set; }
        public string? Description { get; set; }
        public string? Avatar { get; set; }

    }
}
