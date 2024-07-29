using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Statistics_Exercise.Queries.GetExercisesWithHistory;
public record ExerciseHistoryKey 
{
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; } = "";
}

public record ExerciseHistoryEntry
{
    public ExerciseHistoryKey? ExerciseKey { get; set; } 
    public int LogCount { get; set; }
}

public record GetExercisesWithHistoryQuery(string UserId) : IRequest<List<ExerciseHistoryEntry>>
{
}

public class GetExercisesWithHistoryQueryValidator : AbstractValidator<GetExercisesWithHistoryQuery>
{
    public GetExercisesWithHistoryQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID must be greater than 0.");
    }
}

public class GetExercisesWithHistoryQueryHandler : IRequestHandler<GetExercisesWithHistoryQuery, List<ExerciseHistoryEntry>>
{
    private readonly IApplicationDbContext _context;

    public GetExercisesWithHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExerciseHistoryEntry>> Handle(GetExercisesWithHistoryQuery request, CancellationToken cancellationToken)
    {
        var exerciseHistory = await _context.ExerciseLogs
            .Include(el => el.Exercise)
            .Include(el => el.WorkoutLog)
            .Where(el => el.WorkoutLog != null
                         && el.WorkoutLog.CreatedByNavigation != null
                         && el.WorkoutLog.CreatedByNavigation.Id == request.UserId)
            .GroupBy(el => new
            {
                ExerciseId = el.ExerciseId ?? 0,
                ExerciseName = el.Exercise != null ? el.Exercise.ExerciseName : "Unknown"
            })
            .Select(g => new ExerciseHistoryEntry
            {
                ExerciseKey = new ExerciseHistoryKey
                {
                    ExerciseId = g.Key.ExerciseId,
                    ExerciseName = g.Key.ExerciseName ?? "Unknown"
                },
                LogCount = g.Count()
            })
            .ToListAsync(cancellationToken);

        return exerciseHistory;
    }
}
