using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using System;

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
    public bool IsDeleted { get; init; }
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
            return Result.Failure(new[] { "Workout Template not found" });
        }

        entity.TemplateName = request.TemplateName;
        entity.Duration = request.Duration;
        entity.IsPublic = request.IsPublic;
        entity.LastModified = DateTimeOffset.UtcNow;

        foreach (var exerciseDto in request.WorkoutTemplateExercises)
        {
            var existingExercise = entity.WorkoutTemplateExercises
                .FirstOrDefault(e => e.ExerciseId == exerciseDto.ExerciseId && e.OrderInSession == exerciseDto.OrderInSession);

            if (existingExercise != null)
            {
                if (exerciseDto.IsDeleted)
                {
                    _context.WorkoutTemplateExercises.Remove(existingExercise);
                }
                else
                {
                    existingExercise.OrderInSession = exerciseDto.OrderInSession;
                    existingExercise.OrderInSuperset = exerciseDto.OrderInSuperset;
                    existingExercise.Note = exerciseDto.Note;
                    existingExercise.SetsRecommendation = exerciseDto.SetsRecommendation;
                    existingExercise.IntensityPercentage = exerciseDto.IntensityPercentage;
                    existingExercise.RpeRecommendation = exerciseDto.RpeRecommendation;
                    existingExercise.WeightsUsed = exerciseDto.WeightsUsed;
                    existingExercise.NumbersOfReps = exerciseDto.NumbersOfReps;

                }
            }
            else if (!exerciseDto.IsDeleted)
            {
                var newExercise = new WorkoutTemplateExercise
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
                    WorkoutTemplateId = entity.Id,
                };

                _context.WorkoutTemplateExercises.Add(newExercise);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
