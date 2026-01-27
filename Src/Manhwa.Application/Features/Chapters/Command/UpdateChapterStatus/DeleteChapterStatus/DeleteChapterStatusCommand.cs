using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Chapters.Command.UpdateChapterStatus.DeleteChapterStatus
{
    public class DeleteChapterStatusCommand : IRequest<DeleteChapterStatusResponse>
    {
        public long UserId { get; set; }
        public long ChapterId { get; set; }
        public string UserRole { get; set; } = null!;
    }
}
