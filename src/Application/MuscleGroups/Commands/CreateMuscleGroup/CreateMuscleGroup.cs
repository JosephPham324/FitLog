using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;

public record CreateMuscleGroupCommand : IRequest<int>
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
            .Must(BeAValidUrl)
                .WithMessage("Invalid URL format.");
    }

    private bool BeAValidUrl(string? imageUrl)
    {
        return Uri.TryCreate(imageUrl, UriKind.Absolute, out _);
    }

    private async Task<bool> BeUniqueName(string muscleGroupName, CancellationToken cancellationToken)
    {
        return await _context.MuscleGroups
            .AllAsync(mg => mg.MuscleGroupName != muscleGroupName, cancellationToken);
    }
}


public class CreateMuscleGroupCommandHandler : IRequestHandler<CreateMuscleGroupCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateMuscleGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMuscleGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new MuscleGroup
        {
            MuscleGroupName = request.MuscleGroupName,
            ImageUrl = request.ImageUrl
        };

        _context.MuscleGroups.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.MuscleGroupId;
    }
}
