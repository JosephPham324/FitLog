using System.Text.Json.Serialization;
using FitLog.Application.Common.Extensions;
using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FitLog.Application.WorkoutLogs.Queries.ExportWorkoutData;

public record ExportWorkoutDataQuery : IRequest<string>
{
    [JsonIgnore]
    public string UserId { get; init; } = string.Empty;
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public ExportWorkoutDataQuery(string userId, DateTime? startDate, DateTime? endDate)
    {
        UserId = userId;
        StartDate = startDate ?? DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);
        EndDate = endDate ?? StartDate.Value.AddDays(6);
    }
}

public class ExportWorkoutDataQueryValidator : AbstractValidator<ExportWorkoutDataQuery>
{
    public ExportWorkoutDataQueryValidator()
    {
        RuleFor(v => v.UserId).NotEmpty();
    }
}

public class ExportWorkoutDataQueryHandler : IRequestHandler<ExportWorkoutDataQuery, string>
{
    private readonly IApplicationDbContext _context;

    public ExportWorkoutDataQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(ExportWorkoutDataQuery request, CancellationToken cancellationToken)
    {
        var workoutLogs = await _context.WorkoutLogs
            .Where(wl => wl.CreatedBy != null ? wl.CreatedBy.Equals(request.UserId) : false)
            .Where(wl => wl.Created >= request.StartDate && wl.Created <= request.EndDate)
            .Include(wl => wl.ExerciseLogs)
            .ThenInclude(el => el.Exercise)
            .ToListAsync(cancellationToken);

        var result = new StringBuilder();

        foreach (var log in workoutLogs)
        {
            result.AppendLine($"{log.Created:yyyy/MM/dd}");
            result.AppendLine($"Note: {log.Note}");
            result.AppendLine("Exercise\tSets\tWeight\tReps\tNote");

            foreach (var exerciseLog in log.ExerciseLogs)
            {
                if (exerciseLog is null) continue;
                var exerciseName = exerciseLog.Exercise?.ExerciseName;
                var note = exerciseLog.Note;
                var order = exerciseLog.OrderInSession;

                var sets = exerciseLog.NumberOfSets;
                var weights = string.Join("/", exerciseLog.WeightsUsed?.Trim('[', ']').Split(',').Select(w => $"{w}kg") ?? Array.Empty<string>());
                var reps = string.Join("/", exerciseLog.NumberOfReps?.Trim('[', ']').Split(',') ?? Array.Empty<string>());

                result.AppendLine($"{exerciseName}\t{order}\t{sets}\t{weights}\t{reps}\t{note}");
            }

            result.AppendLine();
        }

        return FormatWorkoutLogs(workoutLogs);
    }

    private string FormatWorkoutLogs(List<WorkoutLog> workoutLogs)
    {
        var result = new StringBuilder();

        result.AppendLine("Date,Note,Exercise,Order,Sets,Weight,Reps,ExerciseNote");

        foreach (var log in workoutLogs)
        {
            foreach (var exerciseGroup in log.ExerciseLogs.GroupBy(el => el.ExerciseId))
            {
                var firstExerciseLog = exerciseGroup.First();
                var exerciseName = firstExerciseLog.Exercise?.ExerciseName;
                var note = firstExerciseLog.Note;
                var order = firstExerciseLog.OrderInSession;

                var sets = exerciseGroup.Sum(el => el.NumberOfSets);
                var weights = string.Join("/", exerciseGroup.SelectMany(el => el.WeightsUsed?.Trim('[', ']').Split(',').Select(w => $"{w}kg") ?? Array.Empty<string>()));
                var reps = string.Join("/", exerciseGroup.SelectMany(el => el.NumberOfReps?.Trim('[', ']').Split(',') ?? Array.Empty<string>()));

                result.AppendLine($"{log.Created:yyyy-MM-dd},{log.Note},{exerciseName},{order},{sets},{weights},{reps},{note}");
            }
        }

        return result.ToString();
    }
}
