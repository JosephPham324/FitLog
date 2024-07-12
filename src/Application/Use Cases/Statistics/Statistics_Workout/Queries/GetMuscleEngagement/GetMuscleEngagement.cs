using FitLog.Application.Common.Extensions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;

namespace FitLog.Application.Statistics_Workout.Queries.GetMuscleEngagement;

public record GetMuscleEngagementQuery : IRequest<object>
{
    public string UserId { get; set; } = string.Empty;
    public string TimeFrame { get; set; } = string.Empty;
}

public class GetMuscleEngagementQueryValidator : AbstractValidator<GetMuscleEngagementQuery>
{
    public GetMuscleEngagementQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.TimeFrame).NotEmpty().WithMessage("TimeFrame is required.")
                                  .Must(ValidTimeFrame).WithMessage("Invalid TimeFrame.");

    }

    private bool ValidTimeFrame(string timeFrame)
    {
        if (string.IsNullOrEmpty(timeFrame))
        {
            return false;
        }

        var normalizedTimeFrame = timeFrame.ToUpper();
        var result = normalizedTimeFrame.Equals(TimeFrames.Weekly.ToUpper()) ||
               normalizedTimeFrame.Equals(TimeFrames.Monthly.ToUpper()) ||
               normalizedTimeFrame.Equals(TimeFrames.Yearly.ToUpper());
        return result;
    }
}

public class GetMuscleEngagementQueryHandler : IRequestHandler<GetMuscleEngagementQuery, object>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetMuscleEngagementQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<object> Handle(GetMuscleEngagementQuery request, CancellationToken cancellationToken)
    {
        DateTime startDate;
        DateTime endDate = DateTime.UtcNow;

        switch (request.TimeFrame.ToUpperInvariant())
        {
            case string weekly when weekly == TimeFrames.Weekly.ToUpperInvariant():
                startDate = endDate.StartOfWeek(DayOfWeek.Monday);
                break;
            case string monthly when monthly == TimeFrames.Monthly.ToUpperInvariant():
                startDate = new DateTime(endDate.Year, endDate.Month, 1);
                break;
            case string yearly when yearly == TimeFrames.Yearly.ToUpperInvariant():
                startDate = new DateTime(endDate.Year, 1, 1);
                break;
            default:
                throw new ArgumentException("Invalid TimeFrame", nameof(request.TimeFrame));
        }

        var workoutHistoryQuery = new GetWorkoutHistoryQuery(startDate, endDate)
        {
            UserId = request.UserId
        };

        var workoutHistory = await _mediator.Send(workoutHistoryQuery, cancellationToken);

        if (workoutHistory == null)
        {
            return new List<object>();
        }

        var muscleEngagement = new Dictionary<string, int>();

        foreach (var workoutLog in (List<WorkoutLogDTO>)workoutHistory)
        {
            foreach (var exerciseLog in workoutLog.ExerciseLogs)
            {
                var exercise = await _context.Exercises
                    .Include(e => e.ExerciseMuscleGroups)
                    .ThenInclude(emg => emg.MuscleGroup)
                    .FirstOrDefaultAsync(e => e.ExerciseId == exerciseLog.ExerciseId, cancellationToken);

                if (exercise != null)
                {
                    foreach (var exerciseMuscleGroup in exercise.ExerciseMuscleGroups)
                    {
                        var muscleGroup = exerciseMuscleGroup.MuscleGroup;
                        if (muscleGroup == null)
                        {
                            continue;
                        }
                        if (muscleEngagement.ContainsKey(muscleGroup?.MuscleGroupName??""))
                        {
                            muscleEngagement[muscleGroup?.MuscleGroupName ?? ""] += exerciseLog.NumberOfSets ?? 0;
                        }
                        else
                        {
                            muscleEngagement[muscleGroup?.MuscleGroupName ?? ""] = exerciseLog.NumberOfSets ?? 0;
                        }
                    }
                }
            }
        }

        return muscleEngagement.Select(me => new { Muscle = me.Key, Sets = me.Value }).ToList();
    }
}
