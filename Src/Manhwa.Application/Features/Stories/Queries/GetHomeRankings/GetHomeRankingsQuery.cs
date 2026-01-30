using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Stories.Queries.GetHomeRankings
{
    public class GetHomeRankingsQuery : IRequest<HomeRankingsDto>
    {
    }
}
