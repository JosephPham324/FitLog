using FitLog.Application.Common.Extensions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.ValidationRules;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Constants;

namespace FitLog.Application.Statistics_Workout.Queries.GetTotalReps;

public record GetTotalRepsQuery : IRequest<Dictionary<DateTime, int>>
{
    public string UserId { get; set; } = string.Empty;
    public string TimeFrame { get; set; } = string.Empty;
}

public class GetTotalRepsQueryValidator : AbstractValidator<GetTotalRepsQuery>
{
    public GetTotalRepsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.TimeFrame).NotEmpty().WithMessage("TimeFrame is required.")
                                  .Must(ValidationRules.ValidTimeFrame).WithMessage("Invalid TimeFrame.");
    }
}

public class GetTotalRepsQueryHandler : IRequestHandler<GetTotalRepsQuery, Dictionary<DateTime, int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetTotalRepsQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Dictionary<DateTime, int>> Handle(GetTotalRepsQuery request, CancellationToken cancellationToken)
    {
        DateTimeOffset endDate = DateTimeOffset.Now;
        DateTimeOffset startDate = new DateTimeOffset(new DateTime(1900, 1, 1)); // Fetch all history from January 1, 1900


        var workoutHistoryQuery = new GetWorkoutHistoryQuery(startDate.DateTime, endDate.DateTime)
        {
            UserId = request.UserId
        };

        var workoutLogs = await _mediator.Send(workoutHistoryQuery, cancellationToken) as List<WorkoutLogDTO> ?? new List<WorkoutLogDTO>();
        
        var totalRepsByPeriod = new Dictionary<DateTime, int>();

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

            if (!totalRepsByPeriod.ContainsKey(periodStart))
            {
                totalRepsByPeriod[periodStart] = 0;
            }

            foreach (var exerciseLog in log.ExerciseLogs)
            {
                var reps = exerciseLog.GetNumberOfReps();
                if (reps!= null) totalRepsByPeriod[periodStart] += reps.Sum();
            }
        }

        return totalRepsByPeriod;
    }
}
