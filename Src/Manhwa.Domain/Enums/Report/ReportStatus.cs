using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Enums.Report
{
    public enum ReportStatus : short
    {
        Pending = 0,
        Resolved = 1,
        Rejected = 2
    }
}
