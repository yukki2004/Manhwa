using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Common.Interfaces.Notifications
{
    public interface INotificationStrategy
    {
        short Type { get; }
        (string Title, string Content, string RedirectUrl) Build(string? rawDataJson);
    }
}
