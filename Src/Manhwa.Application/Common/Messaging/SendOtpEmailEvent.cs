using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Messaging
{
    public class SendOtpEmailEvent
    {
        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
    }
}
