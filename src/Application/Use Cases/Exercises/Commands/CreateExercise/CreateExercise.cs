using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.ValidationRules;
using FitLog.Domain.Entities;
using FitLog.Domain.Constants;
using Microsoft.Extensions.Logging;
using FitLog.Application.Common.Models;
using System.Text.Json.Serialization;

namespace FitLog.Application.Exercises.Commands.CreateExercise;

public record CreateExerciseCommand : IRequest<Result>
{
    [JsonIgnore]
    public string? CreatedBy { get; set; }
    public List<int> MuscleGroupIds { get; init; } = new List<int>();
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

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateExerciseCommand command, CancellationToken cancellationToken)
    {
        var entity = new Exercise
        {
            CreatedBy = command.CreatedBy,
            EquipmentId = command.EquipmentId,
            ExerciseName = command.ExerciseName,
            DemoUrl = command.DemoUrl,
            Type = command.Type,
            Description = command.Description,
            PublicVisibility = command.PublicVisibility
        };

        foreach (var muscleGroupId in command.MuscleGroupIds)
        {
            entity.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup { MuscleGroupId = muscleGroupId });
        }

        _context.Exercises.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful();
    }
}
