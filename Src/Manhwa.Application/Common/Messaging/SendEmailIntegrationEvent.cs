using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Messaging
{
    public class SendEmailIntegrationEvent
    {
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string TemplateName { get; set; } = null!;
        public Dictionary<string, string> TemplateData { get; set; } = new();
    }
}
