using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Enums.Comment
{
    public enum CommentStatus : short
    {
        Published = 1,
        Hidden = 2,
        Deleted = 3
    }
}
