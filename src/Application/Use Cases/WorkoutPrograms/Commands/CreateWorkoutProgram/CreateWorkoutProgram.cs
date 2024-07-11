using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;
using FitLog.Application.WorkoutTemplates.Commands.CreateWorkoutTemplate;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutPrograms.Commands.CreateWorkoutProgram;

public record CreateWorkoutProgramCommand : IRequest<Result>
{
    public string? UserId { get; set; }
    public string ProgramName { get; set; } = "";
    public string? ProgramThumbnail { get; set; }
    public int? NumberOfWeeks { get; set; }
    public int? DaysPerWeek { get; set; }
    public string? Goal { get; set; }
    public string? ExperienceLevel { get; set; }
    public string? GymType { get; set; }
    public string? MusclesPriority { get; set; }
    public string? AgeGroup { get; set; }
    public bool? PublicProgram { get; set; }
    public List<CreateProgramWorkoutCommand> ProgramWorkouts { get; set; } = new List<CreateProgramWorkoutCommand>();
}
public record CreateProgramWorkoutCommand : IRequest<Result>
{
    public int? WeekNumber { get; set; }
    public int? OrderInWeek { get; set; }
    public CreateWorkoutTemplateCommand? WorkoutTemplate { get; set; }
}

public class CreateWorkoutProgramCommandValidator : AbstractValidator<CreateWorkoutProgramCommand>
{
    public CreateWorkoutProgramCommandValidator()
    {
    }
}

public class CreateWorkoutProgramCommandHandler : IRequestHandler<CreateWorkoutProgramCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkoutProgramCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateWorkoutProgramCommand request, CancellationToken cancellationToken)
    {
        var entity = new Program
        {
            UserId = request.UserId,
            ProgramName = request.ProgramName,
            ProgramThumbnail = request.ProgramThumbnail,
            NumberOfWeeks = request.NumberOfWeeks,
            DaysPerWeek = request.DaysPerWeek,
            Goal = request.Goal,
            ExperienceLevel = request.ExperienceLevel,
            GymType = request.GymType,
            MusclesPriority = request.MusclesPriority,
            AgeGroup = request.AgeGroup,
            PublicProgram = request.PublicProgram,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };

        // Add ProgramWorkouts and their WorkoutTemplates
        foreach (var workoutDto in request.ProgramWorkouts)
        {
            var workoutTemplate = new WorkoutTemplate
            {
                TemplateName = workoutDto.WorkoutTemplate?.TemplateName,
                Duration = workoutDto.WorkoutTemplate?.Duration,
                IsPublic = workoutDto.WorkoutTemplate?.IsPublic ?? false,
                CreatedBy = request.UserId,
                LastModifiedBy = request.UserId,
            };

            if (workoutDto.WorkoutTemplate?.WorkoutTemplateExercises != null)
            {
                foreach (var exerciseDto in workoutDto.WorkoutTemplate.WorkoutTemplateExercises)
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
            }

            var programWorkout = new ProgramWorkout
            {
                WeekNumber = workoutDto.WeekNumber,
                OrderInWeek = workoutDto.OrderInWeek,
                WorkoutTemplate = workoutTemplate,
                
            };
            entity.ProgramWorkouts.Add(programWorkout);
        }

        _context.Programs.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
