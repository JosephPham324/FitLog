using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination;

public record GetMuscleGroupsListWithPaginationQuery : IRequest<CoachSummaryDTO>
{
}

public class GetMuscleGroupsListWithPaginationQueryValidator : AbstractValidator<GetMuscleGroupsListWithPaginationQuery>
{
    public GetMuscleGroupsListWithPaginationQueryValidator()
    {
    }
}

public class GetMuscleGroupsListWithPaginationQueryHandler : IRequestHandler<GetMuscleGroupsListWithPaginationQuery, CoachSummaryDTO>
{
    private readonly IApplicationDbContext _context;

    public GetMuscleGroupsListWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public 
        //async 
        Task<CoachSummaryDTO> Handle(GetMuscleGroupsListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
