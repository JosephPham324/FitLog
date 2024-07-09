using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUsers;

namespace FitLog.Application.CoachProfiles.Queries.GetCoachApplicationsWithPagination;

public record GetCoachApplicationsWithPaginationQuery : IRequest<PaginatedList<CoachApplicationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCoachApplicationsWithPaginationQueryValidator : AbstractValidator<GetCoachApplicationsWithPaginationQuery>
{
    public GetCoachApplicationsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than zero.");
        RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size must be greater than zero.");
    }
}

public class GetCoachApplicationsWithPaginationQueryHandler : IRequestHandler<GetCoachApplicationsWithPaginationQuery, PaginatedList<CoachApplicationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCoachApplicationsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CoachApplicationDto>> Handle(GetCoachApplicationsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = await  _context.CoachApplications
            .AsNoTracking()
            .ProjectTo<CoachApplicationDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNumber, request.PageSize);

        return query;
    }
}
