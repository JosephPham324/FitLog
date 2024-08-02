using System.Text.Json;
using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.ExerciseLogs.Commands.CreateExerciseLogs;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;
public record CreateWorkoutLogCommandDTO
{
    public string? WorkoutLogName { get; init; }
    public string? Note { get; init; }
    public TimeOnly? Duration { get; init; }
    public List<CreateExerciseLogCommand>? ExerciseLogs { get; init; }
}

public record CreateWorkoutLogCommand : IRequest<Result>
{
    [JsonIgnore]
    public string? CreatedBy { get; set; }
    public string? WorkoutLogName { get; init; }

    public string? Note { get; init; }
    public TimeOnly? Duration { get; init; }
    public List<CreateExerciseLogCommand>? ExerciseLogs { get; init; }

    public CreateWorkoutLogCommand(string? createdBy, CreateWorkoutLogCommandDTO dto)
    {
        WorkoutLogName = dto.WorkoutLogName;
        CreatedBy = createdBy;
        Note = dto.Note;
        Duration = dto.Duration;
        ExerciseLogs = dto.ExerciseLogs;
    }
}

public record CreateExerciseLogCommand : IRequest<Result>
{
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
}



public class CreateWorkoutLogCommandValidator : AbstractValidator<CreateWorkoutLogCommand>
{
    public CreateWorkoutLogCommandValidator()
    {
        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CreatedBy is required.");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters.");

        RuleForEach(x => x.ExerciseLogs).SetValidator(new CreateExerciseLogCommandValidator());
    }
}

public class CreateExerciseLogCommandValidator : AbstractValidator<CreateExerciseLogCommand>
{
    public CreateExerciseLogCommandValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotNull().WithMessage("ExerciseId is required.");

        RuleFor(x => x.FootageUrls)
            .Must(BeValidJson).WithMessage("FootageUrls must be a valid JSON array.");

        // Additional rules can be added here if needed
    }

    private bool BeValidJson(string? footageUrls)
    {
        if (string.IsNullOrEmpty(footageUrls))
        {
            return true;
        }

        try
        {
            JsonDocument.Parse(footageUrls);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
public class CreateWorkoutLogCommandHandler : IRequestHandler<CreateWorkoutLogCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkoutLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateWorkoutLogCommand request, CancellationToken cancellationToken)
    {
        var workoutLog = new WorkoutLog
        {
            WorkoutLogName = request.WorkoutLogName ?? DateTime.Now.TimeOfDay.ToString() + "Workout",
            CreatedBy = request.CreatedBy,
            Note = request.Note,
            Duration = request.Duration,
            Created = DateTime.Now,
            LastModified = DateTime.Now // Set to current date and time
        };

        _context.WorkoutLogs.Add(workoutLog);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure([ex.Message]);
        }

        if (request.ExerciseLogs != null)
        {
            foreach (var exerciseLog in request.ExerciseLogs)
            {
                var exerciseLogEntity = new ExerciseLog
                {
                    WorkoutLogId = workoutLog.Id,
                    ExerciseId = exerciseLog.ExerciseId,
                    DateCreated = DateTime.Now, // Set to current date and time
                    LastModified = DateTime.Now, // Set to current date and time
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
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure([ex.Message]);
        }

        return Result.Successful();
    }
}

