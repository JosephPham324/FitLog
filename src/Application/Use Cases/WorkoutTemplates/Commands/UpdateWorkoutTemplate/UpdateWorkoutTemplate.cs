using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Commands.UpdateWorkoutTemplate;

public record UpdateWorkoutTemplateCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string? TemplateName { get; init; }
    public string? Duration { get; init; }
    public bool IsPublic { get; init; }
    public ICollection<WorkoutTemplateExerciseDto> WorkoutTemplateExercises { get; init; } = new List<WorkoutTemplateExerciseDto>();
}

public record WorkoutTemplateExerciseDto
{
    public int? ExerciseId { get; init; }
    public int? OrderInSession { get; init; }
    public int? OrderInSuperset { get; init; }
    public string? Note { get; init; }
    public int? SetsRecommendation { get; init; }
    public int? IntensityPercentage { get; init; }
    public int? RpeRecommendation { get; init; }
    public string? WeightsUsed { get; init; }
    public string? NumbersOfReps { get; init; }
}

public class UpdateWorkoutTemplateCommandValidator : AbstractValidator<UpdateWorkoutTemplateCommand>
{
    public UpdateWorkoutTemplateCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Invalid workout template ID.");

        RuleFor(v => v.TemplateName)
            .NotEmpty().WithMessage("Template name is required.")
            .MaximumLength(100).WithMessage("Template name must not exceed 100 characters.");

        RuleFor(v => v.Duration)
            .MaximumLength(50).WithMessage("Duration must not exceed 50 characters.");

        RuleForEach(v => v.WorkoutTemplateExercises).SetValidator(new WorkoutTemplateExerciseValidator());
    }
    private class WorkoutTemplateExerciseValidator : AbstractValidator<WorkoutTemplateExerciseDto>
    {
        public WorkoutTemplateExerciseValidator()
        {
            RuleFor(v => v.ExerciseId)
                .NotNull().WithMessage("Exercise ID is required.");
        }
    }

}


public class UpdateWorkoutTemplateCommandHandler : IRequestHandler<UpdateWorkoutTemplateCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorkoutTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateWorkoutTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WorkoutTemplates
            .Include(wt => wt.WorkoutTemplateExercises)
            .FirstOrDefaultAsync(wt => wt.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Workout Template not found"]);
        }

        entity.TemplateName = request.TemplateName;
        entity.Duration = request.Duration;
        entity.IsPublic = request.IsPublic;
        entity.LastModified = DateTimeOffset.UtcNow;
        // Assume LastModifiedBy is set from the current user context

        // Update the exercises
        entity.WorkoutTemplateExercises.Clear();
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

            entity.WorkoutTemplateExercises.Add(workoutTemplateExercise);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
