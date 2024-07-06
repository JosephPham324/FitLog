using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Equipments.Commands.Delete;

public record DeleteEquipmentCommand : IRequest<Result>
{
    public int EquipmentId { get; init; }
}

public class DeleteEquipmentCommandValidator : AbstractValidator<DeleteEquipmentCommand>
{
    public DeleteEquipmentCommandValidator()
    {
    }
}

public class DeleteEquipmentCommandHandler : IRequestHandler<DeleteEquipmentCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Equipment.FindAsync(new object[] { request.EquipmentId }, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Entity not found"]); // Entity not found
        }

        _context.Equipment.Remove(entity);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Successful(); // Successfully deleted
        }
        catch (Exception)
        {
            // Log the exception (optional)
            // _logger.LogError(ex, "Error deleting equipment");

            return Result.Failure(["Error deleting equipment"]); // Deletion failed
        }
    }
}

