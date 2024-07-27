using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;
using FitLog.Application.WorkoutTemplates.Commands.CreateWorkoutTemplate;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutPrograms.Commands.CreateWorkoutProgram;

public record CreateWorkoutProgramCommand : IRequest<Result>
{
    [JsonIgnore]
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
    private readonly IApplicationDbContext _context;

    public CreateWorkoutProgramCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.ProgramName).NotEmpty().WithMessage("Program name is required.");
        RuleFor(x => x.NumberOfWeeks).NotEmpty().WithMessage("Number of weeks is required.")
            .InclusiveBetween(1, 52).WithMessage("Number of weeks must be between 1 and 52.");
        RuleFor(x => x.DaysPerWeek).NotEmpty().WithMessage("Days per week is required.")
            .InclusiveBetween(1, 7).WithMessage("Days per week must be between 1 and 7.");
        RuleFor(x => x.Goal).NotEmpty().WithMessage("Goal is required.")
            .Must(BeAValidGoal).WithMessage("Invalid goal.");
        RuleFor(x => x.ExperienceLevel).NotEmpty().WithMessage("Experience level is required.")
            .Must(BeAValidExperienceLevel).WithMessage("Invalid experience level.");
        RuleFor(x => x.GymType).NotEmpty().WithMessage("Gym type is required.")
            .Must(BeAValidGymType).WithMessage("Invalid gym type.");
        RuleFor(x => x.MusclesPriority).Custom((musclesPriority, context) =>
        {
            if (!MusclesPriorityIsValid(musclesPriority ?? ""))
            {
                context.AddFailure("Invalid muscles priority.");
            }
        });
        RuleFor(x => x.ProgramWorkouts).NotEmpty().WithMessage("At least one workout is required.");
    }

    private bool BeAValidGoal(string? goal)
    {
        return ProgramAttributes.Goals.Contains(goal ?? "");
    }

    private bool BeAValidExperienceLevel(string? experienceLevel)
    {
        return ProgramAttributes.ExperienceLevels.Contains(experienceLevel ?? "");
    }

    private bool BeAValidGymType(string? gymType)
    {
        return ProgramAttributes.GymTypes.ContainsKey(gymType ?? "");
    }

    private bool MusclesPriorityIsValid(string musclesPriority)
    {
        if (string.IsNullOrEmpty(musclesPriority)) return true; // Allow empty value
        var muscles = musclesPriority.Split(',');
        var validMuscles = _context.MuscleGroups.Select(x => x.MuscleGroupName).ToList();
        return muscles.All(x => validMuscles.Contains(x));
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
