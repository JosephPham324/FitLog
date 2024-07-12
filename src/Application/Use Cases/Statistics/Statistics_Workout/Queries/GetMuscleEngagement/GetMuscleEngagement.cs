using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement;

public record GetMuscleEngagementQuery : IRequest<object>
{
}

public class GetMuscleEngagementQueryValidator : AbstractValidator<GetMuscleEngagementQuery>
{
    public GetMuscleEngagementQueryValidator()
    {
    }
}

public class GetMuscleEngagementQueryHandler : IRequestHandler<GetMuscleEngagementQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetMuscleEngagementQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public /*async*/ Task<object> Handle(GetMuscleEngagementQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
