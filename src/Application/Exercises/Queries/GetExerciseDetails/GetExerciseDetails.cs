using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Exercises.Queries.GetExerciseDetails;

public record GetExerciseDetailsQuery : IRequest<ExerciseDetailsDTO>
{
}

public class GetExerciseDetailsQueryValidator : AbstractValidator<GetExerciseDetailsQuery>
{
    public GetExerciseDetailsQueryValidator()
    {
    }
}

public class GetExerciseDetailsQueryHandler : IRequestHandler<GetExerciseDetailsQuery, ExerciseDetailsDTO>
{
    private readonly IApplicationDbContext _context;

    public GetExerciseDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExerciseDetailsDTO> Handle(GetExerciseDetailsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
