using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.Exercises.Commands.ImportExercises;

public record ExerciseImport
{
    public List<string> MuscleGroupNames { get; init; } = new List<string>();
    public string? EquipmentName { get; init; }
    public string? ExerciseName { get; init; }
    public string? DemoUrl { get; init; }
    public string Type { get; init; } = null!;
    public string? Description { get; init; }
    public bool? PublicVisibility { get; init; }
}


public record ImportExercisesCommand : IRequest<Result>
{
    public List<ExerciseImport> Exercises { get; init; } = new();
}
public class ImportExercisesCommandValidator : AbstractValidator<ImportExercisesCommand>
{
    public ImportExercisesCommandValidator()
    {
        RuleForEach(x => x.Exercises).ChildRules(exercise =>
        {
            exercise.RuleFor(e => e.ExerciseName).NotEmpty().MaximumLength(200);
            exercise.RuleFor(e => e.Type).NotEmpty();
            exercise.RuleFor(e => e.MuscleGroupNames).NotEmpty().Must(names => names.Count > 0);
            exercise.RuleFor(e => e.EquipmentName).NotEmpty();
            exercise.RuleFor(e => e.DemoUrl).Must(Common.ValidationRules.ValidationRules.BeAValidUrl)
                .When(e => !string.IsNullOrEmpty(e.DemoUrl));
        });
    }
}



public class ImportExercisesCommandHandler : IRequestHandler<ImportExercisesCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public ImportExercisesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ImportExercisesCommand request, CancellationToken cancellationToken)
    {
        int importedCount = 0;
        List<Exercise> exercises = new List<Exercise>();
        foreach (var exerciseImport in request.Exercises)
        {
            List<MuscleGroup> muscleGroups = new List<MuscleGroup>();
            foreach (var groupName in exerciseImport.MuscleGroupNames)
            {
                var muscleGroup = await _context.MuscleGroups
                    .FirstOrDefaultAsync(mg => mg.MuscleGroupName == groupName, cancellationToken);
                if (muscleGroup == null)
                {
                    muscleGroup = new MuscleGroup { MuscleGroupName = groupName };
                    _context.MuscleGroups.Add(muscleGroup);
                }
                muscleGroups.Add(muscleGroup);
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(e => e.EquipmentName == exerciseImport.EquipmentName, cancellationToken);
            if (equipment == null)
            {
                equipment = new Equipment { EquipmentName = exerciseImport.EquipmentName };
                _context.Equipment.Add(equipment);
            }

            var exercise = new Exercise
            {
                EquipmentId = equipment.EquipmentId,
                Equipment = equipment,
                ExerciseName = exerciseImport.ExerciseName,
                DemoUrl = exerciseImport.DemoUrl,
                Type = exerciseImport.Type,
                Description = exerciseImport.Description,
                PublicVisibility = exerciseImport.PublicVisibility
            };
            List<ExerciseMuscleGroup> exerciseMuscleGroups = new List<ExerciseMuscleGroup>();

            //foreach (var muscleGroup in muscleGroups)
            //{
            //    var exerciseMuscleGroup  = new ExerciseMuscleGroup { MuscleGroupId = muscleGroup.MuscleGroupId};
            //    exercise.ExerciseMuscleGroups.Add(exerciseMuscleGroup);
            //}
            exercise.ExerciseMuscleGroups = muscleGroups.Select(mg => new ExerciseMuscleGroup { MuscleGroupId = mg.MuscleGroupId }).ToList();

            //_context.Exercises.Add(exercise);
            exercises.Add(exercise);
            foreach (var muscleGroup in exercise.ExerciseMuscleGroups)
            {
                muscleGroup.ExerciseId = exercise.ExerciseId;
            }
            importedCount++;
        }

        await _context.Exercises.AddRangeAsync(exercises, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful();
    }
}


