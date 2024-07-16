using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Statistics_Exercise.Queries.GetExercisesWithHistory;

public record GetExercisesWithHistoryQuery : IRequest<Dictionary<string,int>>
{
}

public class GetExercisesWithHistoryQueryValidator : AbstractValidator<GetExercisesWithHistoryQuery>
{
    public GetExercisesWithHistoryQueryValidator()
    {
    }
}

public class GetExercisesWithHistoryQueryHandler : IRequestHandler<GetExercisesWithHistoryQuery, Dictionary<string,int>>
{
    private readonly IApplicationDbContext _context;

    public GetExercisesWithHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public /*async*/ Task<Dictionary<string,int>> Handle(GetExercisesWithHistoryQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
