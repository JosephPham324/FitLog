using FitLog.Application.Common.Extensions;
using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;

public record GetWorkoutHistoryQuery : IRequest<object>
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public GetWorkoutHistoryQuery(DateTime? startDate, DateTime? endDate)
    {

        StartDate = startDate ?? DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);
        EndDate = endDate ?? StartDate.Value.AddDays(6);
    }
}

public class GetWorkoutHistoryQueryValidator : AbstractValidator<GetWorkoutHistoryQuery>
{
    public GetWorkoutHistoryQueryValidator()
    {
    }
}

public class GetWorkoutHistoryQueryHandler : IRequestHandler<GetWorkoutHistoryQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetWorkoutHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> Handle(GetWorkoutHistoryQuery request, CancellationToken cancellationToken)
    {
        var workoutLogs = await _context.WorkoutLogs
            .Where(wl => wl.DateCreated >= request.StartDate && wl.DateCreated <= request.EndDate)
            .Include(wl => wl.ExerciseLogs)
            .ThenInclude(el => el.Exercise)
            .ToListAsync(cancellationToken);

        var result = workoutLogs.Select(wl => new
        {
            wl.WorkoutLogId,
            wl.CreatedBy,
            wl.Note,
            wl.Duration,
            wl.DateCreated,
            wl.LastModified,
            ExerciseLogs = wl.ExerciseLogs.Select(el => new
            {
                el.ExerciseLogId,
                el.ExerciseId,
                el.DateCreated,
                el.LastModified,
                el.OrderInSession,
                el.OrderInSuperset,
                el.Note,
                el.NumberOfSets,
                WeightsUsed = el.WeightsUsedValue,
                NumberOfReps = el.NumberOfRepsValue,
                FootageUrls = el.FootageURLsList,
                ExerciseName = el.Exercise?.ExerciseName
            })
        });

        return result;
    }
}
