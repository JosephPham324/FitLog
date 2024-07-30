using System.Text.Json;
using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutLogs.Commands.UpdateWorkoutLog;

public record UpdateWorkoutLogCommand : IRequest<Result>
{

    public int WorkoutLogId { get; init; }
    public string? Note { get; init; }
    public TimeOnly? Duration { get; init; }
    public List<UpdateExerciseLogCommand>? ExerciseLogs { get; init; }
}
public record UpdateExerciseLogCommand : IRequest<Result>
{
    public int? ExerciseLogId { get; init; }
    public int? ExerciseId { get; init; }
    public int? OrderInSession { get; init; }
    public int? OrderInSuperset { get; init; }
    public string? Note { get; init; }
    public int? NumberOfSets { get; init; }
    [JsonIgnore]
    public List<int>? WeightsUsedValue { get; set; }
    [JsonIgnore]
    public List<int>? NumberOfRepsValue { get; set; }

    public string? WeightsUsed
    {
        get => JsonSerializer.Serialize(WeightsUsedValue);
        set => WeightsUsedValue = string.IsNullOrEmpty(value) ? new List<int>() : JsonSerializer.Deserialize<List<int>>(value);
    }

    public string? NumberOfReps
    {
        get => JsonSerializer.Serialize(NumberOfRepsValue);
        set => NumberOfRepsValue = string.IsNullOrEmpty(value) ? new List<int>() : JsonSerializer.Deserialize<List<int>>(value);
    }

    public string? FootageUrls { get; init; }

    public bool IsDeleted { get; init; }  //flag to indicate deletion

}

public class UpdateWorkoutLogCommandValidator : AbstractValidator<UpdateWorkoutLogCommand>
{
    public UpdateWorkoutLogCommandValidator()
    {
        RuleFor(x => x.WorkoutLogId).NotEmpty();

    }
}

public class UpdateWorkoutLogCommandHandler : IRequestHandler<UpdateWorkoutLogCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorkoutLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateWorkoutLogCommand request, CancellationToken cancellationToken)
    {
        var workoutLog = await _context.WorkoutLogs
           .Include(wl => wl.ExerciseLogs)
           .FirstOrDefaultAsync(wl => wl.Id == request.WorkoutLogId, cancellationToken);

        if (workoutLog == null)
        {
            return Result.Failure(new[] { $"Workout log with ID {request.WorkoutLogId} not found." });
        }

        workoutLog.Note = request.Note;
        workoutLog.Duration = request.Duration;
        workoutLog.LastModified = DateTime.UtcNow;

        if (request.ExerciseLogs != null)
        {
            foreach (var exerciseLog in request.ExerciseLogs)
            {
                var existingExerciseLog = workoutLog.ExerciseLogs
                    .FirstOrDefault(el => el.ExerciseLogId == exerciseLog.ExerciseLogId);

                if (existingExerciseLog != null)
                {
                    if (exerciseLog.IsDeleted)
                    {
                        _context.ExerciseLogs.Remove(existingExerciseLog);
                    }
                    else
                    {
                        existingExerciseLog.ExerciseId = exerciseLog.ExerciseId;
                        existingExerciseLog.OrderInSession = exerciseLog.OrderInSession;
                        existingExerciseLog.OrderInSuperset = exerciseLog.OrderInSuperset;
                        existingExerciseLog.Note = exerciseLog.Note;
                        existingExerciseLog.NumberOfSets = exerciseLog.NumberOfSets;
                        existingExerciseLog.WeightsUsed = exerciseLog.WeightsUsed;
                        existingExerciseLog.NumberOfReps = exerciseLog.NumberOfReps;
                        existingExerciseLog.FootageUrls = exerciseLog.FootageUrls;
                        existingExerciseLog.LastModified = DateTime.UtcNow;
                    }
                }
                else if (!exerciseLog.IsDeleted)
                {
                    var newExerciseLog = new ExerciseLog
                    {
                        WorkoutLogId = workoutLog.Id,
                        ExerciseId = exerciseLog.ExerciseId,
                        DateCreated = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        OrderInSession = exerciseLog.OrderInSession,
                        OrderInSuperset = exerciseLog.OrderInSuperset,
                        Note = exerciseLog.Note,
                        NumberOfSets = exerciseLog.NumberOfSets,
                        WeightsUsed = exerciseLog.WeightsUsed,
                        NumberOfReps = exerciseLog.NumberOfReps,
                        FootageUrls = exerciseLog.FootageUrls
                    };

                    _context.ExerciseLogs.Add(newExerciseLog);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

