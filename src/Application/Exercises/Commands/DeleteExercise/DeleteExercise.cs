using FitLog.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FitLog.Application.Exercises.Commands.DeleteExercise;

public record DeleteExerciseCommand : IRequest<bool>
{
    public int ExerciseId { get; init; }
}

public class DeleteExerciseCommandValidator : AbstractValidator<DeleteExerciseCommand>
{
    public DeleteExerciseCommandValidator()
    {
        RuleFor(e => e.ExerciseId).GreaterThan(0);
    }
}

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises.FindAsync(new object[] { request.ExerciseId }, cancellationToken);

        if (entity == null)
        {
            return false; // Entity not found
        }

        _context.Exercises.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
        
        return true; // Successfully deleted
        
        
    }
}

