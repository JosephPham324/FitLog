using FitLog.Application.Common.Extensions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.ValidationRules;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Constants;

namespace FitLog.Application.Statistics_Workout.Queries.GetTotalTrainingTonnage;

public record GetTotalTrainingTonnageQuery : IRequest<Dictionary<DateTime, double>>
{
    public string UserId { get; set; } = string.Empty;
    public string TimeFrame { get; set; } = string.Empty;
}

public class GetTotalTrainingTonnageQueryValidator : AbstractValidator<GetTotalTrainingTonnageQuery>
{
    public GetTotalTrainingTonnageQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.TimeFrame).NotEmpty().WithMessage("TimeFrame is required.")
                                  .Must(ValidationRules.ValidTimeFrame).WithMessage("Invalid TimeFrame.");
    }
}

public class GetTotalTrainingTonnageQueryHandler : IRequestHandler<GetTotalTrainingTonnageQuery, Dictionary<DateTime, double>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetTotalTrainingTonnageQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Dictionary<DateTime, double>> Handle(GetTotalTrainingTonnageQuery request, CancellationToken cancellationToken)
    {
        DateTimeOffset endDate = DateTimeOffset.Now;
        DateTimeOffset startDate = new DateTimeOffset(new DateTime(1900, 1, 1)); // Fetch all history from January 1, 1900

        var workoutHistoryQuery = new GetWorkoutHistoryQuery(startDate.DateTime, endDate.DateTime)
        {
            UserId = request.UserId
        };

        var workoutLogs = await _mediator.Send(workoutHistoryQuery, cancellationToken) as List<WorkoutLogDTO> ?? new List<WorkoutLogDTO>();
        var totalTonnageByPeriod = new Dictionary<DateTime, double>();

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

            if (!totalTonnageByPeriod.ContainsKey(periodStart))
            {
                totalTonnageByPeriod[periodStart] = 0;
            }

            foreach (var exerciseLog in log.ExerciseLogs)
            {
                var weights = exerciseLog.WeightsUsed?
                                            .Trim(['[', ']'])?
                                            .Split([',', ';'])?
                                            //.Where(weight => !string.IsNullOrEmpty(weight))
                                            .Select(Double.Parse)?
                                            .ToList() ?? new List<double>();

                var reps = exerciseLog.NumberOfReps?
                          .Trim(['[', ']'])?
                          .Split([',', ';'])?
                          //.Where(rep => !string.IsNullOrEmpty(rep))
                          .Select(Double.Parse)?
                          .ToList();

                for (int i = 0; i < weights.Count; i++)
                {
                    totalTonnageByPeriod[periodStart] += weights[i] * (reps?.Count > i ? reps[i] : 0);
                }
            }
        }

        return totalTonnageByPeriod;
    }
}
