using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Equipments.Commands.Delete;

public record DeleteEquipmentCommand : IRequest<bool>
{
    public int EquipmentId { get; init; }
}

public class DeleteEquipmentCommandValidator : AbstractValidator<DeleteEquipmentCommand>
{
    public DeleteEquipmentCommandValidator()
    {
    }
}

public class DeleteEquipmentCommandHandler : IRequestHandler<DeleteEquipmentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Equipment.FindAsync(new object[] { request.EquipmentId }, cancellationToken);

        if (entity == null)
        {
            return false; // Entity not found
        }

        _context.Equipment.Remove(entity);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Successfully deleted
        }
        catch (Exception)
        {
            // Log the exception (optional)
            // _logger.LogError(ex, "Error deleting equipment");

            return false; // Deletion failed
        }
    }
}

