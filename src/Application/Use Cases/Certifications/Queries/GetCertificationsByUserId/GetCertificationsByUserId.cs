using FitLog.Application.Certifications.Queries.GetCertificationsByUserId;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Queries.GetUserDetails;

namespace FitLog.Application.Certifications.Queries.GetCertificationsByUserId;

public record GetCertificationsByUserIdQuery : IRequest<List<CertificationDTO>>
{
    public string UserId { get; set; } = string.Empty;
}

public class GetCertificationsByUserIdQueryValidator : AbstractValidator<GetCertificationsByUserIdQuery>
{
    public GetCertificationsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}

public class GetCertificationsByUserIdQueryHandler : IRequestHandler<GetCertificationsByUserIdQuery, List<CertificationDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCertificationsByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CertificationDTO>> Handle(GetCertificationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var certifications = await _context.Certifications
            .Where(c => c.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CertificationDTO>>(certifications);
    }
}
