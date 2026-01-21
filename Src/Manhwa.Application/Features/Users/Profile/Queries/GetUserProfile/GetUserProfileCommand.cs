using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetUserProfile
{
    public class GetUserProfileCommand : IRequest<GetUserProfileResponse>
    {
        public string Username { get; set; } = null!;
    }
}
