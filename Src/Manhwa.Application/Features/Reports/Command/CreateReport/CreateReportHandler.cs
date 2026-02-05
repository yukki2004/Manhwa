using Manhwa.Application.Common.Exceptions;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Application.Common.Interfaces.Report;
using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Report;
using Manhwa.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application.Features.Reports.Command.CreateReport
{
    public class CreateReportHandler : IRequestHandler<CreateReportCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnumerable<IReportTargetStrategy> _targetStrategies;
        public CreateReportHandler(IUserRepository userRepository, IReportRepository reportRepository, IUnitOfWork unitOfWork, IEnumerable<IReportTargetStrategy> targetStrategies)
        {
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _targetStrategies = targetStrategies;
        }
        public async Task<bool> Handle(CreateReportCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, ct);
            if(user == null || user.IsActive == false)
            {
                throw new Exception();
            }
            var isAlreadyReported = await _reportRepository.IsAlreadyReportedAsync(command.UserId,command.TargetId, command.TargetType, ct);
            if(isAlreadyReported)
            {
                throw new BusinessRuleViolationException("bạn đã báo cáo", "REPORTED");
            }
            var metadataSnapshot = await _reportRepository.GetSnapshotAsync(
            command.TargetType,
            command.TargetId,
            ct);
            var report = new Report
            {
                UserId = command.UserId,
                TargetId = command.TargetId,
                TargetType = command.TargetType,
                Reason = command.Reason,
                Status = ReportStatus.Pending, 
                Metadata = metadataSnapshot     
            };

            await _reportRepository.AddAsync(report, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;


        }
    }
}
