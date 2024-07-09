using System.Linq.Expressions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Common.ValidationRules;
using FitLog.Domain.Entities;

namespace FitLog.Application.MuscleGroups.Commands.UpdateMuscleGroup;

public record UpdateMuscleGroupCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string? MuscleGroupName { get; init; }
    public string? ImageUrl { get; init; }
}


public class UpdateMuscleGroupCommandValidator : AbstractValidator<UpdateMuscleGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMuscleGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(mg => mg.Id)
            .NotEmpty();

        RuleFor(mg => mg.MuscleGroupName)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("Muscle group name already exists.");

        RuleFor(mg => mg.ImageUrl)
            .NotEmpty()
            .Must(ValidationRules.BeAValidUrl)
                .WithMessage("Invalid URL format.");
    }

    private async Task<bool> BeUniqueName(UpdateMuscleGroupCommand command, string muscleGroupName, CancellationToken cancellationToken)
    {
        return await _context.MuscleGroups
            .AllAsync(mg => mg.MuscleGroupName != muscleGroupName || mg.MuscleGroupId == command.Id, cancellationToken);
    }
}


public class UpdateMuscleGroupCommandHandler : IRequestHandler<UpdateMuscleGroupCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateMuscleGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateMuscleGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MuscleGroups.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Muscle group not found"]);
        }

        entity.MuscleGroupName = request.MuscleGroupName;
        entity.ImageUrl = request.ImageUrl;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Successful();
        }
        catch (Exception)
        {
            // Log the exception (optional)
            //_logger.LogError(ex, "Error updating muscle group");

            return Result.Failure(["Error updating muscle group"]);
        }
    }
}
