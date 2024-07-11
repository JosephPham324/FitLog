using FitLog.Application.Common.Interfaces;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;

namespace FitLog.Application.TrainingRecommendations.Queries.RecommendProgramQuery;

public record RecommendProgramQueryQuery : IRequest<WorkoutProgramListDTO>
{
}

public class RecommendProgramQueryQueryValidator : AbstractValidator<RecommendProgramQueryQuery>
{
    public RecommendProgramQueryQueryValidator()
    {
    }
}

public class RecommendProgramQueryQueryHandler : IRequestHandler<RecommendProgramQueryQuery, WorkoutProgramListDTO>
{
    private readonly IApplicationDbContext _context;

    public RecommendProgramQueryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<WorkoutProgramListDTO> Handle(RecommendProgramQueryQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
