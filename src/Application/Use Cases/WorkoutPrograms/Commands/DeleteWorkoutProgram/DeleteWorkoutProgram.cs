using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.WorkoutPrograms.Commands.DeleteWorkoutProgram;

public record DeleteWorkoutProgramCommand : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteWorkoutProgramCommandValidator : AbstractValidator<DeleteWorkoutProgramCommand>
{
    public DeleteWorkoutProgramCommandValidator()
    {
    }
}

public class DeleteWorkoutProgramCommandHandler : IRequestHandler<DeleteWorkoutProgramCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkoutProgramCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteWorkoutProgramCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Programs.FindAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(["Workout program not found"]);
        }

        _context.Programs.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
