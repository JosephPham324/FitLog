using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;

public record GetWorkoutHistoryQuery : IRequest<object>
{
}

public class GetWorkoutHistoryQueryValidator : AbstractValidator<GetWorkoutHistoryQuery>
{
    public GetWorkoutHistoryQueryValidator()
    {
    }
}

public class GetWorkoutHistoryQueryHandler : IRequestHandler<GetWorkoutHistoryQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetWorkoutHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(GetWorkoutHistoryQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
