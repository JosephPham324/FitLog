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

public record GetSummaryStatsQuery : IRequest<SummaryWorkoutLogStatsDTO>
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

public class GetSummaryStatsQueryHandler : IRequestHandler<GetSummaryStatsQuery, SummaryWorkoutLogStatsDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetSummaryStatsQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<SummaryWorkoutLogStatsDTO> Handle(GetSummaryStatsQuery request, CancellationToken cancellationToken)
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

        var workoutLogs = await _mediator.Send(workoutHistoryQuery, cancellationToken) as List<WorkoutLogDTO> ?? new List<WorkoutLogDTO>();
        double totalTime = 0;
        double weightLifted = 0;

        #region Iterate through workout logs and calculate total time and weight lifted
        foreach (var log in workoutLogs)
        {
            var exerciseLogs = log.ExerciseLogs;
            //-----------------
            // Calculate weight
            foreach (var exerciseLog in exerciseLogs)
            {
                var weightsUsed = exerciseLog.WeightsUsed?
                                            .Trim(['[', ']'])?
                                            .Split([',', ';'])?
                                            //.Where(weight => !string.IsNullOrEmpty(weight))
                                            .Select(Double.Parse)?
                                            .ToList() ?? new List<double>();
                var reps = exerciseLog.NumberOfReps?
                          .Trim(['[', ']'])?
                          .Split([',',';'])?
                          //.Where(rep => !string.IsNullOrEmpty(rep))
                          .Select(Double.Parse)?
                          .ToList();

                for (int i = 0; i < weightsUsed?.Count; i++)
                {
                    weightLifted += weightsUsed[i] * (reps != null ? reps[i] : 0);
                }
            }
            //-----------------
            //Calculate time
            if (log != null)
            {
                var duration = log.Duration;

                if (duration == null)
                {
                    continue;
                }
                totalTime += (double)duration.Value.Hour;
                totalTime += (double)duration.Value.Minute / 60;
            }

        }
        #endregion

        var summaryStats = new SummaryWorkoutLogStatsDTO
        {
            NumberOfWorkouts = workoutLogs.Count,
            HoursAtTheGym = totalTime,
            WeightLifted = weightLifted,
            WeekStreak = GetWeekStreak(workoutLogs)
        };

        return summaryStats;
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
