using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.ValidationRules;

namespace FitLog.Application.Equipments.Commands.UpdateEquipment;

public record UpdateEquipmentCommand : IRequest<bool>
{
    public int EquipmentId { get; init; }
    public string? EquipmentName { get; init; }
    public string? ImageUrl { get; init; }
}

public class UpdateEquipmentCommandValidator : AbstractValidator<UpdateEquipmentCommand>
{
    public UpdateEquipmentCommandValidator()
    {
        RuleFor(e => e.EquipmentId)
            .GreaterThan(0);

        RuleFor(e => e.EquipmentName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(e => e.ImageUrl)
            .Must(ValidationRules.BeAValidUrl)
            .When(e => !string.IsNullOrEmpty(e.ImageUrl))
            .WithMessage("Invalid URL format.");
    }
}

public class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Equipment.FindAsync(new object[] { request.EquipmentId }, cancellationToken);

        if (entity == null)
        {
            return false; // Entity not found
        }

        entity.EquipmentName = request.EquipmentName;
        entity.ImageUrl = request.ImageUrl;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Successfully updated
        }
        catch (Exception)
        {
            // Log the exception (optional)
            // _logger.LogError(ex, "Error updating equipment");

            return false; // Update failed
        }
    }
}
