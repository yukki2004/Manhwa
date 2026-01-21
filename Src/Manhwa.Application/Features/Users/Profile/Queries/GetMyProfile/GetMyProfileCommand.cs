using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Profile.Queries.GetMyProfile
{
    public class GetMyProfileCommand : IRequest<GetMyProfileResponse>
    {
        public long UserId { get; set; }
    }
}
