using Manhwa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
