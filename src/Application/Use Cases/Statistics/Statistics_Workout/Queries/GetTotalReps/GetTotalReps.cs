using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Statistics_Workout.Queries.GetTotalReps;

public record GetTotalRepsQuery : IRequest<object>
{
}

public class GetTotalRepsQueryValidator : AbstractValidator<GetTotalRepsQuery>
{
    public GetTotalRepsQueryValidator()
    {
    }
}

public class GetTotalRepsQueryHandler : IRequestHandler<GetTotalRepsQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetTotalRepsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public /*async*/ Task<object> Handle(GetTotalRepsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
