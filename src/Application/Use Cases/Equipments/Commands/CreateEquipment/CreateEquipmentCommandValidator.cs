using FitLog.Application.Common.Interfaces;
using FitLog.Application.Equipments.Commands.CreateEquipment;

namespace FitLog.Application.Equipments.Commands.CreateEquipment;

public class CreateEquipmentCommandValidator : AbstractValidator<CreateEquipmentCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateEquipmentCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(e => e.EquipmentName)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'{PropertyName}' already exists!")
                .WithErrorCode("Unique");

        RuleFor(e => e.ImageUrl)
            .NotEmpty()
            .WithMessage("Image URL cannot be empty.")
            .Must(BeAValidUrl)
                .WithMessage("Invalid URL format.")
                .WithErrorCode("Invalid URL");

    }

    public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
    {
        return await _context.Equipment
            .AllAsync(l => l.EquipmentName != name, cancellationToken);
    }

    private bool BeAValidUrl(string? imageUrl)
    {
        return Uri.TryCreate(imageUrl, UriKind.Absolute, out _);
    }
}
