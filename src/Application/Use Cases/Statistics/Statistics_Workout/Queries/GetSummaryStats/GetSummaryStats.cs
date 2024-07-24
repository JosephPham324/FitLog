using FitLog.Application.Common.Extensions;
using System.Globalization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Constants;
using FitLog.Application.Common.ValidationRules;

namespace FitLog.Application.Statistics_Workout.Queries.GetSummaryStats;


public class SummaryWorkoutLogStatsDTO
{
    public int NumberOfWorkouts { get; set; }
    public double HoursAtTheGym { get; set; }
    public double WeightLifted { get; set; }
    public double WeekStreak { get; set; }
}

public record GetSummaryStatsQuery : IRequest<Dictionary<DateTime, SummaryWorkoutLogStatsDTO>>
{
    public string UserId { get; set; } = string.Empty;
    public string TimeFrame { get; set; } = string.Empty;
}

public class GetSummaryStatsQueryValidator : AbstractValidator<GetSummaryStatsQuery>
{
    public GetSummaryStatsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.TimeFrame).NotEmpty().WithMessage("TimeFrame is required.")
                                  .Must(ValidationRules.ValidTimeFrame).WithMessage("Invalid TimeFrame.");

    }
}

public class GetSummaryStatsQueryHandler : IRequestHandler<GetSummaryStatsQuery, Dictionary<DateTime, SummaryWorkoutLogStatsDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetSummaryStatsQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Dictionary<DateTime,SummaryWorkoutLogStatsDTO>> Handle(GetSummaryStatsQuery request, CancellationToken cancellationToken)
    {
        DateTimeOffset endDate = DateTimeOffset.Now;
        DateTimeOffset startDate = new DateTimeOffset(new DateTime(1900, 1, 1)); // Fetch all history from January 1, 1900

        var workoutHistoryQuery = new GetWorkoutHistoryQuery(request.UserId, startDate.DateTime, endDate.DateTime);
        var workoutLogs = await _mediator.Send(workoutHistoryQuery, cancellationToken) as List<WorkoutLogDTO> ?? new List<WorkoutLogDTO>();

        var statsByPeriod = new Dictionary<DateTime, SummaryWorkoutLogStatsDTO>();

        foreach (var log in workoutLogs)
        {
            DateTime periodStart;
            switch (request.TimeFrame.ToUpperInvariant())
            {
                case string weekly when weekly == TimeFrames.Weekly.ToUpperInvariant():
                    periodStart = log.Created.UtcDateTime.StartOfWeek(DayOfWeek.Monday);
                    break;
                case string monthly when monthly == TimeFrames.Monthly.ToUpperInvariant():
                    periodStart = new DateTime(log.Created.Year, log.Created.Month, 1);
                    break;
                case string yearly when yearly == TimeFrames.Yearly.ToUpperInvariant():
                    periodStart = new DateTime(log.Created.Year, 1, 1);
                    break;
                default:
                    throw new ArgumentException("Invalid TimeFrame", nameof(request.TimeFrame));
            }

            if (!statsByPeriod.ContainsKey(periodStart))
            {
                statsByPeriod[periodStart] = new SummaryWorkoutLogStatsDTO
                {
                    NumberOfWorkouts = 0,
                    HoursAtTheGym = 0,
                    WeightLifted = 0,
                    WeekStreak = 0
                };
            }

            var stats = statsByPeriod[periodStart];
            stats.NumberOfWorkouts++;
            stats.HoursAtTheGym += GetTotalTimeFromWorkoutLog(log.Duration);
            stats.WeightLifted += GetWeightFromExerciseLogs(log.ExerciseLogs);

            // Calculate week streak for the specific period
            stats.WeekStreak = GetWeekStreak(workoutLogs.Where(w => w.Created.UtcDateTime >= periodStart && w.Created.UtcDateTime < periodStart.AddTimeFrame(request.TimeFrame)).ToList());
        }

        return statsByPeriod;
    }


    private static double GetTotalTimeFromWorkoutLog(TimeOnly? duration)
    {
        if (duration == null) return 0;

        double totalTime = 0.0;
        
        totalTime += (double)duration.Value.Hour;
        totalTime += (double)duration.Value.Minute / 60;
        
        return totalTime;
    }

    private static double GetWeightFromExerciseLogs(List<ExerciseLogDTO> exerciseLogs)
    {
        double weightLifted = 0;
        foreach (var exerciseLog in exerciseLogs)
        {
            var weightsUsed = exerciseLog.GetWeightsUsed();

            var reps = exerciseLog.GetNumberOfReps();

            for (int i = 0; i < weightsUsed?.Count; i++)
            {
                weightLifted += weightsUsed[i] * (reps != null ? reps[i] : 0);
            }
        }

        return weightLifted;
    }

    private double GetWeekStreak(List<WorkoutLogDTO>? workoutLogs)
    {
        if (workoutLogs == null)
        {
            return 0;
        }

        var groupedByWeek = workoutLogs
            .GroupBy(wl => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(wl.Created.DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
            .OrderBy(g => g.Key)
            .ToList();

        //double streak = 0;
        //double maxStreak = 0;
        //for (int i = 0; i < groupedByWeek.Count - 1; i++)
        //{
        //    if (groupedByWeek[i].Key + 1 == groupedByWeek[i + 1].Key)
        //    {
        //        streak++;
        //        maxStreak = Math.Max(maxStreak, streak);
        //    }
        //    else
        //    {
        //        streak = 0;
        //    }
        //}

        return groupedByWeek.Count;
    }
}
