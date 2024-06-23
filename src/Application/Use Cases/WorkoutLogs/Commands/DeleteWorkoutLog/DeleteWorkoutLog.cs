using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutLogs.Commands.DeleteWorkoutLog;

public record DeleteWorkoutLogCommand : IRequest<bool>
{
    public int WorkoutLogId { get; init; }

}

public class DeleteWorkoutLogCommandValidator : AbstractValidator<DeleteWorkoutLogCommand>
{
    public DeleteWorkoutLogCommandValidator()
    {
        RuleFor(x => x.WorkoutLogId).NotEmpty();
    }
}

public class DeleteWorkoutLogCommandHandler : IRequestHandler<DeleteWorkoutLogCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkoutLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteWorkoutLogCommand request, CancellationToken cancellationToken)
    {
        var workoutLog = await _context.WorkoutLogs
           .Include(wl => wl.ExerciseLogs)
           .FirstOrDefaultAsync(wl => wl.WorkoutLogId == request.WorkoutLogId, cancellationToken);

        if (workoutLog == null)
        {
            throw new NotFoundException(nameof(WorkoutLog), request.WorkoutLogId.ToString());
        }

        // Delete the exercise logs first
        _context.ExerciseLogs.RemoveRange(workoutLog.ExerciseLogs);

        // Delete the workout log
        _context.WorkoutLogs.Remove(workoutLog);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
