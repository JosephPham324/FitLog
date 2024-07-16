using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Common.ValidationRules;

namespace FitLog.Application.Equipments.Commands.UpdateEquipment;

public record UpdateEquipmentCommand : IRequest<Result>
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

public class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Equipment.FindAsync(new object[] { request.EquipmentId }, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Entity not found"]); // Entity not found
        }

        entity.EquipmentName = request.EquipmentName;
        entity.ImageUrl = request.ImageUrl;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Successful(); // Successfully updated
        }
        catch (Exception)
        {
            // Log the exception (optional)
            // _logger.LogError(ex, "Error updating equipment");

            return Result.Failure(["Error updating equipment"]); // Update failed
        }
    }
}
