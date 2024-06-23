using FitLog.Application.Common.Interfaces;
using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination;

namespace FitLog.Application.Users.Queries.GetCoachesListWithPagination;

public record GetCoachesListWithPaginationQuery : IRequest<CoachSummaryDTO>
{
}

public class GetCoachesListWithPaginationQueryValidator : AbstractValidator<GetCoachesListWithPaginationQuery>
{
    public GetCoachesListWithPaginationQueryValidator()
    {
    }
}

public class GetCoachesListWithPaginationQueryHandler : IRequestHandler<GetCoachesListWithPaginationQuery, CoachSummaryDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCoachesListWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public 
        //async 
        Task<CoachSummaryDTO> Handle(GetCoachesListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
