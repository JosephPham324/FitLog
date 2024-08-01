using FitLog.Application.Common.Interfaces;
using FitLog.Application.Exercises.Queries.GetExercises;

namespace FitLog.Application.Exercises.Queries.SearchExercises;


public record SearchExercisesQuery : IRequest<List<ExerciseDTO>>
{
    public string? ExerciseName { get; init; }
    public int? EquipmentId { get; init; }
    public List<int> MuscleGroupIds { get; init; } = new();
}

public class SearchExercisesQueryValidator : AbstractValidator<SearchExercisesQuery>
{
    public SearchExercisesQueryValidator()
    {
        //RuleFor(x => x.EquipmentId).NotNull().WithMessage("EquipmentId is required.");
        //RuleFor(x => x.MuscleGroupIds).NotEmpty().WithMessage("At least one MuscleGroupId is required.");
    }
}

public class SearchExercisesQueryHandler : IRequestHandler<SearchExercisesQuery, List<ExerciseDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchExercisesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ExerciseDTO>> Handle(SearchExercisesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Exercises.AsQueryable();

        if (!string.IsNullOrEmpty(request.ExerciseName))
        {
            var lowerCaseExerciseName = request.ExerciseName.ToLower();
            query = query.Where(e => e.ExerciseName != null && e.ExerciseName.ToLower().Contains(lowerCaseExerciseName));
        }
        if (request.EquipmentId.HasValue)
        {
            query = query.Where(e => e.EquipmentId == request.EquipmentId.Value);
        }

        if (request.MuscleGroupIds.Any())
        {
            query = query.Where(e => e.ExerciseMuscleGroups
                                     .All(emg => request.MuscleGroupIds.Contains(emg.MuscleGroupId)));
        }

        var exercises = await query
            .OrderBy(e => e.ExerciseName)
            .ProjectTo<ExerciseDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return exercises;
    }
}
