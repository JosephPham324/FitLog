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
            .Include(ex=>ex.ExerciseLogs)
            .Include(ex=>ex.WorkoutTemplateExercises)
            .Where(e => e.ExerciseId == request.ExerciseId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Exercise not found"]); // Entity not found
        }
        if (entity.WorkoutTemplateExercises.Any())
        {
            return Result.Failure(["Exercise is used in a workout template"]); // Exercise is used in a workout template
        }

        if (entity.ExerciseLogs.Any())
        {
            return Result.Failure(["Exercise is used in an exercise log"]); // Exercise is used in an exercise log
        }

        _context.Exercises.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Successful(); // Successfully deleted
        
        
    }
}

