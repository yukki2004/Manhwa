using Manhwa.Application.Common.Models;
using Manhwa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Users.Management.Queries.GetUserExpLogs
{
    public class ExpUserLogRequest : PagingParamUsers
    {
        public ExpActionType? Action { get; set; }
    }
}
