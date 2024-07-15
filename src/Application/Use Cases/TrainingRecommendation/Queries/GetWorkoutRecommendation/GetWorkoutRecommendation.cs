using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.TrainingRecommendation.Queries.GetWorkoutRecommendation;

public record GetWorkoutRecommendationQuery : IRequest<object>
{

}

public class GetWorkoutRecommendationQueryValidator : AbstractValidator<GetWorkoutRecommendationQuery>
{
    public GetWorkoutRecommendationQueryValidator()
    {
    }
}

public class GetWorkoutRecommendationQueryHandler : IRequestHandler<GetWorkoutRecommendationQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetWorkoutRecommendationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public /*async*/ Task<object> Handle(GetWorkoutRecommendationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
