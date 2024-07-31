using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Certifications.Commands.DeleteCertification
{
    public class DeleteCertificationCommand : IRequest<Result>
    {
        public int CertificationId { get; set; }
    }

    public class DeleteCertificationCommandHandler : IRequestHandler<DeleteCertificationCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCertificationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteCertificationCommand request, CancellationToken cancellationToken)
        {
            var certification = await _context.Certifications
                .FirstOrDefaultAsync(c => c.CertificationId == request.CertificationId, cancellationToken);

            if (certification == null)
            {
                return Result.Failure(["Certification not found."]);
            }

            _context.Certifications.Remove(certification);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Successful();
        }
    }
}
