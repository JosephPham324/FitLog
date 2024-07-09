using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using FitLog.Application.Common.ValidationRules;
using FitLog.Application.Common.Models;

namespace FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;

public record CreateMuscleGroupCommand : IRequest<Result>
{
    public string? MuscleGroupName { get; set; }
    public string? ImageUrl { get; set; }
}

public class CreateMuscleGroupCommandValidator : AbstractValidator<CreateMuscleGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateMuscleGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;

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

    private async Task<bool> BeUniqueName(string muscleGroupName, CancellationToken cancellationToken)
    {
        return await _context.MuscleGroups
            .AllAsync(mg => mg.MuscleGroupName != muscleGroupName, cancellationToken);
    }
}


public class CreateMuscleGroupCommandHandler : IRequestHandler<CreateMuscleGroupCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateMuscleGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateMuscleGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new MuscleGroup
        {
            MuscleGroupName = request.MuscleGroupName,
            ImageUrl = request.ImageUrl
        };

        _context.MuscleGroups.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
