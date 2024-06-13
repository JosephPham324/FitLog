using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.MuscleGroups.Commands.DeleteMuscleGroup;

public record DeleteMuscleGroupCommand : IRequest<bool>
{
    public int Id { get; init; }
}


public class DeleteMuscleGroupCommandValidator : AbstractValidator<DeleteMuscleGroupCommand>
{
    public DeleteMuscleGroupCommandValidator()
    {
    }
}

public class DeleteMuscleGroupCommandHandler : IRequestHandler<DeleteMuscleGroupCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public DeleteMuscleGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteMuscleGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MuscleGroups.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            return false; // Entity not found
        }

        _context.MuscleGroups.Remove(entity);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Successfully deleted
        }
        catch (Exception)
        {
            // Log the exception (optional)
            // _logger.LogError(ex, "Error deleting muscle group");

            return false; // Deletion failed
        }
    }
}
