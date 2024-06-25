using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.Exercises.Commands.ImportExercises;

public record ExerciseImport
{
    public string? MuscleGroupName { get; init; }
    public string? EquipmentName { get; init; }
    public string? ExerciseName { get; init; }
    public string? DemoUrl { get; init; }
    public string Type { get; init; } = null!;
    public string? Description { get; init; }
    public bool? PublicVisibility { get; init; }
}

public record ImportExercisesCommand : IRequest<int>
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
            exercise.RuleFor(e => e.MuscleGroupName).NotEmpty();
            exercise.RuleFor(e => e.EquipmentName).NotEmpty();
            exercise.RuleFor(e => e.DemoUrl).Must(Common.ValidationRules.ValidationRules.BeAValidUrl).When(e => !string.IsNullOrEmpty(e.DemoUrl));
        });
    }
}


public class ImportExercisesCommandHandler : IRequestHandler<ImportExercisesCommand, int>
{
    private readonly IApplicationDbContext _context;

    public ImportExercisesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(ImportExercisesCommand request, CancellationToken cancellationToken)
    {
        int importedCount = 0;

        foreach (var exerciseImport in request.Exercises)
        {
            var muscleGroup = await _context.MuscleGroups
                .FirstOrDefaultAsync(mg => mg.MuscleGroupName == exerciseImport.MuscleGroupName, cancellationToken);
            if (muscleGroup == null)
            {
                muscleGroup = new MuscleGroup
                {
                    MuscleGroupName = exerciseImport.MuscleGroupName,
                };
                _context.MuscleGroups.Add(muscleGroup);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(e => e.EquipmentName == exerciseImport.EquipmentName, cancellationToken);
            if (equipment == null)
            {
                equipment = new Equipment
                {
                    EquipmentName = exerciseImport.EquipmentName,
                };
                _context.Equipment.Add(equipment);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var exercise = new Exercise
            {
                MuscleGroupId = muscleGroup.MuscleGroupId,
                EquipmentId = equipment.EquipmentId,
                ExerciseName = exerciseImport.ExerciseName,
                DemoUrl = exerciseImport.DemoUrl,
                Type = exerciseImport.Type,
                Description = exerciseImport.Description,
                PublicVisibility = exerciseImport.PublicVisibility,
            };

            _context.Exercises.Add(exercise);
            importedCount++;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return importedCount;
    }
}

