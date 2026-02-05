using Manhwa.Application.Common.Exceptions;
using Manhwa.Domain.Enums.Report;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Reports.Command.ProcessReport
{
    public class ProcessReportHandler : IRequestHandler<ProcessReportCommand, bool>
    {
        private readonly IReportRepository _reportRepository;

        public ProcessReportHandler(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<bool> Handle(ProcessReportCommand request, CancellationToken ct)
        {
            var report = await _reportRepository.GetByIdAsync(request.ReportId, ct);
            if (report == null)
            {
                throw new BusinessRuleViolationException("báo cáo không tồn tại", "REPORT_NOT_FOUND");
            }

            await _reportRepository.UpdateStatusAsync(
                request.ReportId,
                ReportStatus.Rejected,
                ct);

            return true;
        }
    }
}
