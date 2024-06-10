using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Exercises.Queries.GetExercises;

public record GetExercisesQuery : IRequest<ExercisesDTO>
{
}

public class GetExercisesQueryValidator : AbstractValidator<GetExercisesQuery>
{
    public GetExercisesQueryValidator()
    {
    }
}

public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, ExercisesDTO>
{
    private readonly IApplicationDbContext _context;

    public GetExercisesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExercisesDTO> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
