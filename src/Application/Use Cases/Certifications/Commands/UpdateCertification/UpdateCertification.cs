using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Certifications.Commands.UpdateCertification
{
	public class UpdateCertificationCommand : IRequest<Result>
	{
		public int CertificationId { get; set; }
		public string? UserId { get; set; }
		public string? CertificationName { get; set; }
		public DateOnly? CertificationDateIssued { get; set; }
		public DateOnly? CertificationExpirationData { get; set; }
	}

	public class UpdateCertificationCommandHandler : IRequestHandler<UpdateCertificationCommand, Result>
	{
		private readonly IApplicationDbContext _context;

		public UpdateCertificationCommandHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Result> Handle(UpdateCertificationCommand request, CancellationToken cancellationToken)
		{
			var certification = await _context.Certifications
				.FirstOrDefaultAsync(c => c.CertificationId == request.CertificationId, cancellationToken);

			if (certification == null)
			{
				return Result.Failure(["Certification not found."]);
			}

			certification.UserId = request.UserId;
			certification.CertificationName = request.CertificationName;
			certification.CertificationDateIssued = request.CertificationDateIssued;
			certification.CertificationExpirationData = request.CertificationExpirationData;

			_context.Certifications.Update(certification);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Successful();
		}
	}
}
