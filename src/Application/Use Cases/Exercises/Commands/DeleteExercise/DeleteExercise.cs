using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace FitLog.Application.Exercises.Commands.DeleteExercise;

public record DeleteExerciseCommand : IRequest<Result>
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

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises
            .Where(e => e.ExerciseId == request.ExerciseId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Exercise not found"]); // Entity not found
        }

        _context.Exercises.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Successful(); // Successfully deleted
        
        
    }
}

