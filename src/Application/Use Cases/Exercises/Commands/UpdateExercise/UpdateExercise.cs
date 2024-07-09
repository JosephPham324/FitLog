using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;

namespace FitLog.Application.Exercises.Commands.UpdateExercise;

public record UpdateExerciseCommand : IRequest<Result>
{
    public int ExerciseId { get; init; }
    public string? CreatedBy { get; init; }
    public List<int> MuscleGroupIds { get; init; } = new List<int>();
    public int? EquipmentId { get; init; }
    public string? ExerciseName { get; init; }
    public string? DemoUrl { get; init; }
    public string Type { get; init; } = null!;
    public string? Description { get; init; }
    public bool? PublicVisibility { get; init; }
}

public class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExerciseCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(e => e.CreatedBy)
           .NotEmpty()
           .MustAsync(UserExists).WithMessage("User does not exist.");

        RuleFor(e => e.Type)
            .NotEmpty()
            .Must(BeAValidExerciseType).WithMessage("Invalid exercise type.");

        RuleFor(e => e.ExerciseName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(e => e.MuscleGroupIds)
            .NotEmpty()
            .MustAsync(MuscleGroupsExist).WithMessage("One or more muscle groups do not exist.");

        RuleFor(e => e.EquipmentId)
            .NotEmpty()
            .MustAsync(EquipmentExists).WithMessage("Equipment does not exist.");

        RuleFor(e => e.DemoUrl)
            .Must(BeAValidUrl)
            .When(e => !string.IsNullOrEmpty(e.DemoUrl))
            .WithMessage("Invalid URL format.");
    }

    private async Task<bool> MuscleGroupsExist(List<int> muscleGroupIds, CancellationToken cancellationToken)
    {
        return muscleGroupIds.Count > 0 &&
               await _context.MuscleGroups.CountAsync(mg => muscleGroupIds.Contains(mg.MuscleGroupId), cancellationToken) == muscleGroupIds.Count;
    }

    private bool BeAValidExerciseType(string type)
    {
        return type == ExerciseTypes.WeightResistance ||
               type == ExerciseTypes.Calisthenics ||
               type == ExerciseTypes.Plyometrics ||
               type == ExerciseTypes.LissCardio ||
               type == ExerciseTypes.HitCardio;
    }

    private async Task<bool> UserExists(string? userId, CancellationToken cancellationToken)
    {
        return await _context.AspNetUsers.AnyAsync(u => u.Id == userId, cancellationToken);
    }

    private async Task<bool> EquipmentExists(int? equipmentId, CancellationToken cancellationToken)
    {
        if (equipmentId == null) return false;
        return await _context.Equipment.AnyAsync(e => e.EquipmentId == equipmentId, cancellationToken);
    }

    private bool BeAValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises
            .Include(e => e.ExerciseMuscleGroups)
            .FirstOrDefaultAsync(e => e.ExerciseId == request.ExerciseId, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Exercise not found"]); // Entity not found
        }

        entity.CreatedBy = request.CreatedBy;
        entity.EquipmentId = request.EquipmentId;
        entity.ExerciseName = request.ExerciseName;
        entity.DemoUrl = request.DemoUrl;
        entity.Type = request.Type;
        entity.Description = request.Description;
        entity.PublicVisibility = request.PublicVisibility;

        // Update muscle groups
        entity.ExerciseMuscleGroups.Clear();
        foreach (var mgId in request.MuscleGroupIds)
        {
            entity.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { ExerciseId = entity.ExerciseId, MuscleGroupId = mgId });
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Successful(); // Successfully updated
        }
        catch (Exception)
        {
            // Log the exception (optional)
            return Result.Failure(["Error updating exercise"]); // Update failed
        }
    }
}


