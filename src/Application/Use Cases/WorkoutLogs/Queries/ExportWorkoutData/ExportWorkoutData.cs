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

        return FormatWorkoutLogs(workoutLogs);
    }

    private string FormatWorkoutLogs(List<WorkoutLog> workoutLogs)
    {
        var result = new StringBuilder();

        result.AppendLine("Date,Note,Exercise,Order,Sets,Weight,Reps,ExerciseNote");

        foreach (var log in workoutLogs)
        {
            foreach (var exerciseLog in log.ExerciseLogs)
            {
                var exerciseName = exerciseLog.Exercise?.ExerciseName ?? "Unknown";
                var note = exerciseLog.Note ?? "No note";
                var order = exerciseLog.OrderInSession;
                var sets = exerciseLog.NumberOfSets;

                var setLog = new List<string>();
                for (int i = 0; i < sets; i++)
                {
                    string weight = (exerciseLog.WeightsUsedValue == null || i >= exerciseLog.WeightsUsedValue.Count) ? "No weight" : $"{exerciseLog.WeightsUsedValue[i]}kg";
                    string reps = (exerciseLog.NumberOfRepsValue == null || i >= exerciseLog.NumberOfRepsValue.Count) ? "No reps" : $"{exerciseLog.NumberOfRepsValue[i]}";
                    setLog.Add($"{weight}x{reps}");
                }

                var row = $"{log.Created:yyyy-MM-dd},{log.Note},{exerciseName},{order},{sets},{string.Join(" / ", setLog)},{note}";
                result.AppendLine(row);  // Ensure each entry ends with a newline
            }
        }

        // Return the result, trimming any trailing newlines
        return result.ToString().TrimEnd();
    }



}
