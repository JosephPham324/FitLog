using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.ValidationRules;
using FitLog.Domain.Entities;
using FitLog.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace FitLog.Application.Exercises.Commands.CreateExercise;

public record CreateExerciseCommand : IRequest<int>
{
    public string? CreatedBy { get; init; }
    public int? MuscleGroupId { get; init; }
    public int? EquipmentId { get; init; }
    public string? ExerciseName { get; init; }
    public string? DemoUrl { get; init; }
    public string Type { get; init; } = null!;
    public string? Description { get; init; }
    public bool? PublicVisibility { get; init; }
}

public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateExerciseCommandValidator(IApplicationDbContext context)
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

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Exercise
        {
            CreatedBy = request.CreatedBy,
            MuscleGroupId = request.MuscleGroupId,
            EquipmentId = request.EquipmentId,
            ExerciseName = request.ExerciseName,
            DemoUrl = request.DemoUrl,
            Type = request.Type,
            Description = request.Description,
            PublicVisibility = request.PublicVisibility
        };

        _context.Exercises.Add(entity);
        
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ExerciseId;
    }
}
