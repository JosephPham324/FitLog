using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.Exercises.Queries.GetExerciseDetails;

public record GetExerciseByIdQuery : IRequest<ExerciseDetailsDTO>
{
    public int Id { get; init; }
}
public class GetExerciseByIdQueryValidator : AbstractValidator<GetExerciseByIdQuery>
{
    public GetExerciseByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID must be greater than 0.");
    }
}

public class GetExerciseByIdQueryHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseDetailsDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetExerciseByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ExerciseDetailsDTO> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises
            .ProjectTo<ExerciseDetailsDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => e.ExerciseId == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Exercise), request.Id + "");
        }

        return entity;
    }
}
