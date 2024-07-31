using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Certifications.Commands.CreateCertification
{
    public class CreateCertificationCommand : IRequest<Result>
    {
        public string? UserId { get; set; }
        public string? CertificationName { get; set; }
        public DateOnly? CertificationDateIssued { get; set; }
        public DateOnly? CertificationExpirationData { get; set; }
    }

    public class CreateCertificationCommandHandler : IRequestHandler<CreateCertificationCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateCertificationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateCertificationCommand request, CancellationToken cancellationToken)
        {
            var certification = new Certification
            {
                UserId = request.UserId,
                CertificationName = request.CertificationName,
                CertificationDateIssued = request.CertificationDateIssued,
                CertificationExpirationData = request.CertificationExpirationData
            };

            _context.Certifications.Add(certification);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Successful();
        }
    }
}
