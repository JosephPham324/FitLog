using System.Text.Json.Serialization;
using FitLog.Application.Common.Extensions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;

namespace FitLog.Application.WorkoutLogs.Queries.GetWorkoutHistory;

public record GetWorkoutHistoryQuery : IRequest<List<WorkoutLogDTO>>
{
    [JsonIgnore]
    public string UserId { get; init; } = string.Empty;
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public GetWorkoutHistoryQuery(string userId,DateTime? startDate, DateTime? endDate)
    {
        UserId = userId;
        StartDate = startDate ?? DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);
        EndDate = endDate ?? StartDate.Value.AddDays(6);
    }
}

public class GetWorkoutHistoryQueryValidator : AbstractValidator<GetWorkoutHistoryQuery>
{
    public GetWorkoutHistoryQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty();
    }
}

public class GetWorkoutHistoryQueryHandler : IRequestHandler<GetWorkoutHistoryQuery, List<WorkoutLogDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkoutHistoryQueryHandler(IApplicationDbContext context , IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WorkoutLogDTO>> Handle(GetWorkoutHistoryQuery request, CancellationToken cancellationToken)
    {
        var workoutLogs = await _context.WorkoutLogs
            .Where(wl=> wl.CreatedBy != null ? wl.CreatedBy.Equals(request.UserId) : false)
            .Where(wl => wl.Created >= request.StartDate && wl.Created <= request.EndDate)
            .Include(wl => wl.ExerciseLogs)
            .ThenInclude(el => el.Exercise)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<List<WorkoutLogDTO>>(workoutLogs);

        return result;
    }
}
