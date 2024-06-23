using System.Text.Json;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.ExerciseLogs.Commands.CreateExerciseLogs;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;

public record CreateWorkoutLogCommand : IRequest<int>
{
    public string? CreatedBy { get; init; }
    public string? Note { get; init; }
    public TimeOnly? Duration { get; init; }
    public List<CreateExerciseLogCommand>? ExerciseLogs { get; init; }
}

public record CreateExerciseLogCommand : IRequest<int>
{
    public int? ExerciseId { get; init; }
    public int? OrderInSession { get; init; }
    public int? OrderInSuperset { get; init; }
    public string? Note { get; init; }
    public int? NumberOfSets { get; init; }
    public List<int>? WeightsUsedValue { get; set; }
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
}



public class CreateWorkoutLogCommandValidator : AbstractValidator<CreateWorkoutLogCommand>
{
    public CreateWorkoutLogCommandValidator()
    {
    }
}

public class CreateWorkoutLogCommandHandler : IRequestHandler<CreateWorkoutLogCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkoutLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateWorkoutLogCommand request, CancellationToken cancellationToken)
    {
        var workoutLog = new WorkoutLog
        {
            CreatedBy = request.CreatedBy,
            Note = request.Note,
            Duration = request.Duration,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow // Set to current date and time
        };

        _context.WorkoutLogs.Add(workoutLog);
        await _context.SaveChangesAsync(cancellationToken);

        if (request.ExerciseLogs != null)
        {
            foreach (var exerciseLog in request.ExerciseLogs)
            {
                var exerciseLogEntity = new ExerciseLog
                {
                    WorkoutLogId = workoutLog.WorkoutLogId,
                    ExerciseId = exerciseLog.ExerciseId,
                    DateCreated = DateTime.UtcNow, // Set to current date and time
                    LastModified = DateTime.UtcNow, // Set to current date and time
                    OrderInSession = exerciseLog.OrderInSession,
                    OrderInSuperset = exerciseLog.OrderInSuperset,
                    Note = exerciseLog.Note,
                    NumberOfSets = exerciseLog.NumberOfSets,
                    WeightsUsed = exerciseLog.WeightsUsed,
                    NumberOfReps = exerciseLog.NumberOfReps,
                    FootageUrls = exerciseLog.FootageUrls
                };

                _context.ExerciseLogs.Add(exerciseLogEntity);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return workoutLog.WorkoutLogId;
    }
}

