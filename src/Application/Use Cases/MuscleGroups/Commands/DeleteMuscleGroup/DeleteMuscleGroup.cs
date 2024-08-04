using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.MuscleGroups.Commands.DeleteMuscleGroup;

public record DeleteMuscleGroupCommand : IRequest<Result>
{
    public int Id { get; init; }
}


public class DeleteMuscleGroupCommandValidator : AbstractValidator<DeleteMuscleGroupCommand>
{
    public DeleteMuscleGroupCommandValidator()
    {
    }
}

public class DeleteMuscleGroupCommandHandler : IRequestHandler<DeleteMuscleGroupCommand, Result>
{
    private readonly IApplicationDbContext _context;
    public DeleteMuscleGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteMuscleGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MuscleGroups.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Muscle group not found"]); // Entity not found
        }

        //Check if referenced
        var isReferenced =
            await _context.Exercises
            .Include(e => e.ExerciseMuscleGroups)
            .Where(e => e.ExerciseMuscleGroups.Any(em => em.MuscleGroupId == request.Id))
            .AnyAsync();
        if (isReferenced)
        {
            return Result.Failure(["Muscle is referenced by 1 or more exercises!"]); // Entity not found
        }


        _context.MuscleGroups.Remove(entity);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Successful(); // Successfully deleted
        }
        catch (Exception)
        {
            // Log the exception (optional)
            // _logger.LogError(ex, "Error deleting muscle group");

            return Result.Failure(["Error deletig muscle group"]); // Entity not found
        }
    }
}
