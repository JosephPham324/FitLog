using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Constants;

namespace FitLog.Application.Exercises.Commands.UpdateExercise;

public record UpdateExerciseCommand : IRequest<bool>
{
    public int ExerciseId { get; init; }
    public string? CreatedBy { get; init; }
    public int? MuscleGroupId { get; init; }
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

        RuleFor(e => e.MuscleGroupId)
            .NotEmpty()
            .MustAsync(MuscleGroupExists).WithMessage("Muscle group does not exist.");

        RuleFor(e => e.EquipmentId)
            .NotEmpty()
            .MustAsync(EquipmentExists).WithMessage("Equipment does not exist.");

        RuleFor(e => e.DemoUrl)
            .Must(BeAValidUrl)
            .When(e => !string.IsNullOrEmpty(e.DemoUrl))
            .WithMessage("Invalid URL format.");
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

    private async Task<bool> MuscleGroupExists(int? muscleGroupId, CancellationToken cancellationToken)
    {
        if (muscleGroupId == null) return false;
        return await _context.MuscleGroups.AnyAsync(mg => mg.MuscleGroupId == muscleGroupId, cancellationToken);
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

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises.FindAsync(new object[] { request.ExerciseId }, cancellationToken);

        if (entity == null)
        {
            return false; // Entity not found
        }

        entity.CreatedBy = request.CreatedBy;
        entity.MuscleGroupId = request.MuscleGroupId;
        entity.EquipmentId = request.EquipmentId;
        entity.ExerciseName = request.ExerciseName;
        entity.DemoUrl = request.DemoUrl;
        entity.Type = request.Type;
        entity.Description = request.Description;
        entity.PublicVisibility = request.PublicVisibility;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Successfully updated
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            // _logger.LogError(ex, "Error updating exercise");
            var exception = ex;
            return false; // Update failed
        }
    }
}

