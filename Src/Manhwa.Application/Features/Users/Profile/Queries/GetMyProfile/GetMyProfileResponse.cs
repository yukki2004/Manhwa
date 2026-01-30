using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetMyProfile
{
    public class GetMyProfileResponse
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public int CurrentExp { get; set; }
        public short Level { get; set; }
        public bool IsActive { get; set; }
        public LoginType LoginType { get; set; }
        public UserRole? Role { get; set; }
        public DateTimeOffset CreateAt { get; set; }

    }
}
