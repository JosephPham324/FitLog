using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Commands.CreateWorkoutTemplate;

public record CreateWorkoutTemplateCommand : IRequest<Result>
{
    [JsonIgnore]
    public string? UserId { get; set; } = "";//Temp user token
    public string? TemplateName { get; set; }
    public string? Duration { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<WorkoutTemplateExerciseDto> WorkoutTemplateExercises { get; init; } = new List<WorkoutTemplateExerciseDto>();
}

public record WorkoutTemplateExerciseDto
{
    public int? ExerciseId { get; set; }
    public int? OrderInSession { get; set; }
    public int? OrderInSuperset { get; set; }
    public string? Note { get; set; }
    public int? SetsRecommendation { get; set; }
    public int? IntensityPercentage { get; set; }
    public int? RpeRecommendation { get; set; }
    public string? WeightsUsed { get; set; }
    public string? NumbersOfReps { get; set; }
}
public class CreateWorkoutTemplateCommandValidator : AbstractValidator<CreateWorkoutTemplateCommand>
{
    public CreateWorkoutTemplateCommandValidator()
    {
        RuleFor(v => v.TemplateName)
            .NotEmpty().WithMessage("Template name is required.")
            .MaximumLength(100).WithMessage("Template name must not exceed 100 characters.");

        RuleFor(v => v.Duration)
            .MaximumLength(50).WithMessage("Duration must not exceed 50 characters.");

        RuleForEach(v => v.WorkoutTemplateExercises).SetValidator(new CreateWorkoutTemplateExerciseValidator());
    }
}
public class CreateWorkoutTemplateExerciseValidator : AbstractValidator<WorkoutTemplateExerciseDto>
{
    public CreateWorkoutTemplateExerciseValidator()
    {
        RuleFor(v => v.ExerciseId)
            .NotNull().WithMessage("Exercise ID is required.");
    }
}

public class CreateWorkoutTemplateCommandHandler : IRequestHandler<CreateWorkoutTemplateCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkoutTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateWorkoutTemplateCommand request, CancellationToken cancellationToken)
    {
        var workoutTemplate = new WorkoutTemplate
        {
            TemplateName = request.TemplateName,
            Duration = request.Duration,
            IsPublic = request.IsPublic,
            Created = DateTimeOffset.Now,
            CreatedBy  = request.UserId,
            LastModified = DateTimeOffset.Now,
            // Assume CreatedBy and LastModifiedBy are set from the current user context
        };

        foreach (var exerciseDto in request.WorkoutTemplateExercises)
        {
            var workoutTemplateExercise = new WorkoutTemplateExercise
            {
                ExerciseId = exerciseDto.ExerciseId,
                OrderInSession = exerciseDto.OrderInSession,
                OrderInSuperset = exerciseDto.OrderInSuperset,
                Note = exerciseDto.Note,
                SetsRecommendation = exerciseDto.SetsRecommendation,
                IntensityPercentage = exerciseDto.IntensityPercentage,
                RpeRecommendation = exerciseDto.RpeRecommendation,
                WeightsUsed = exerciseDto.WeightsUsed,
                NumbersOfReps = exerciseDto.NumbersOfReps,
            };

            workoutTemplate.WorkoutTemplateExercises.Add(workoutTemplateExercise);
        }

        _context.WorkoutTemplates.Add(workoutTemplate);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
