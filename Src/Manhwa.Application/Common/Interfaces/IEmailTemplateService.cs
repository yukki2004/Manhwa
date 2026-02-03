using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces
{
    public interface IEmailTemplateService
    {
        string GenerateHtmlBody(string templateName, Dictionary<string, string> data);
    }
}
