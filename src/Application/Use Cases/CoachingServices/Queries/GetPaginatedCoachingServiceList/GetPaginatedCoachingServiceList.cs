using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUserDetails;
namespace FitLog.Application.CoachingServices.Queries.GetPaginatedCoachingServiceList;

public record GetPaginatedCoachingServiceListQuery : IRequest<PaginatedList<CoachingServiceDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}



public class GetPaginatedCoachingServiceListQueryValidator : AbstractValidator<GetPaginatedCoachingServiceListQuery>
{
    public GetPaginatedCoachingServiceListQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.");
    }
}

public class GetPaginatedCoachingServiceListQueryHandler : IRequestHandler<GetPaginatedCoachingServiceListQuery, PaginatedList<CoachingServiceDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedCoachingServiceListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CoachingServiceDTO>> Handle(GetPaginatedCoachingServiceListQuery request, CancellationToken cancellationToken)
    {
        return await _context.CoachingServices
            .ProjectTo<CoachingServiceDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
