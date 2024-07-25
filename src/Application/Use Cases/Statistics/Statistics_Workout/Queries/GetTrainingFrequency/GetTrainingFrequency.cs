using System.Text.Json.Serialization;
using FitLog.Application.Common.Extensions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.ValidationRules;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;

namespace FitLog.Application.Statistics_Workout.Queries.GetTrainingFrequency;

public record GetTrainingFrequencyQuery : IRequest<Dictionary<DateTime, int>>
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public string TimeFrame { get; set; } = string.Empty;
}

public class GetTrainingFrequencyQueryValidator : AbstractValidator<GetTrainingFrequencyQuery>
{
    public GetTrainingFrequencyQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.TimeFrame).NotEmpty().WithMessage("TimeFrame is required.")
                                  .Must(ValidationRules.ValidTimeFrame).WithMessage("Invalid TimeFrame.");
    }
}

public class GetTrainingFrequencyQueryHandler : IRequestHandler<GetTrainingFrequencyQuery, Dictionary<DateTime, int>>
{
    private readonly IApplicationDbContext _context;

    public GetTrainingFrequencyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public /*async*/ Task<Dictionary<DateTime, int>> Handle(GetTrainingFrequencyQuery request, CancellationToken cancellationToken)
    {
        DateTimeOffset endDate = DateTimeOffset.Now;
        DateTimeOffset startDate = new DateTimeOffset(new DateTime(1900, 1, 1));

        Func<WorkoutLog, DateTime> groupingKeySelector = request.TimeFrame switch
        {
            TimeFrames.Weekly => wl => wl.Created.DateTime.StartOfWeek(DayOfWeek.Monday),
            TimeFrames.Monthly => wl => new DateTime(wl.Created.Year, wl.Created.Month, 1),
            TimeFrames.Yearly => wl => new DateTime(wl.Created.Year, 1, 1),
            _ => throw new ArgumentException("Invalid TimeFrame", nameof(request.TimeFrame))
        };

        var workoutLogCounts = _context.WorkoutLogs
                .Where(wl => wl.CreatedBy != null && wl.CreatedBy.Equals(request.UserId))
                .Where(wl => wl.Created >= startDate && wl.Created <= endDate)
                .GroupBy(groupingKeySelector)
                .Select(g => new
                {
                    PeriodStart = g.Key,
                    Count = g.Count()
                });

        return Task.FromResult(workoutLogCounts.ToDictionary(x => x.PeriodStart, x => x.Count));
    }
}
