using AutoMapper;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Queries.GetUserDetails;
using FitLog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Certifications.Queries.GetCertificationById
{
    public class GetCertificationByIdQuery : IRequest<CertificationDTO>
    {
        public int CertificationId { get; set; }
    }

    public class GetCertificationByIdQueryHandler : IRequestHandler<GetCertificationByIdQuery, CertificationDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCertificationByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CertificationDTO> Handle(GetCertificationByIdQuery request, CancellationToken cancellationToken)
        {
            var certification = await _context.Certifications
                .FirstOrDefaultAsync(c => c.CertificationId == request.CertificationId, cancellationToken);

            if (certification == null)
            {
                throw new NotFoundException(nameof(Certification), request.CertificationId + "");
            }

            return _mapper.Map<CertificationDTO>(certification);
        }
    }
}
