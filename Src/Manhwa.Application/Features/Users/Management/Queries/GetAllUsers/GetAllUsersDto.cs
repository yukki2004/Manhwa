using Manhwa.Domain.Enums;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetAllUsers
{
    public class GetAllUsersDto
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public LoginType loginType { get; set; }
        public UserRole? Role { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        
    }
}
