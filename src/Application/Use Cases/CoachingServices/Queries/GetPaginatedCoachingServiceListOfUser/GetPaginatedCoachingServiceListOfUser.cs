using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUserDetails;

namespace FitLog.Application.CoachingServices.Queries.GetPaginatedCoachingServiceListOfUser;

public record GetPaginatedCoachingServiceListOfUserQuery : IRequest<PaginatedList<CoachingServiceDTO>>
{
    public string UserId { get; init; } = null!;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPaginatedCoachingServiceListOfUserQueryValidator : AbstractValidator<GetPaginatedCoachingServiceListOfUserQuery>
{
    public GetPaginatedCoachingServiceListOfUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.");
    }
}

public class GetPaginatedCoachingServiceListOfUserQueryHandler : IRequestHandler<GetPaginatedCoachingServiceListOfUserQuery, PaginatedList<CoachingServiceDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedCoachingServiceListOfUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CoachingServiceDTO>> Handle(GetPaginatedCoachingServiceListOfUserQuery request, CancellationToken cancellationToken)
    {
        return await _context.CoachingServices
            .Where(cs => cs.CreatedBy == request.UserId)
            .ProjectTo<CoachingServiceDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
