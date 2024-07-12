using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Statistics_Workout.Queries.GetTotalTrainingTonnage;

public record GetTotalTrainingTonnageQuery : IRequest<object>
{
}

public class GetTotalTrainingTonnageQueryValidator : AbstractValidator<GetTotalTrainingTonnageQuery>
{
    public GetTotalTrainingTonnageQueryValidator()
    {
    }
}

public class GetTotalTrainingTonnageQueryHandler : IRequestHandler<GetTotalTrainingTonnageQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetTotalTrainingTonnageQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public /*async */Task<object> Handle(GetTotalTrainingTonnageQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
