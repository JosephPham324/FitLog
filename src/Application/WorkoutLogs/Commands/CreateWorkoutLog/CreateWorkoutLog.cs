using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;

public record CreateWorkoutLogCommand : IRequest<object>
{
}

public class CreateWorkoutLogCommandValidator : AbstractValidator<CreateWorkoutLogCommand>
{
    public CreateWorkoutLogCommandValidator()
    {
    }
}

public class CreateWorkoutLogCommandHandler : IRequestHandler<CreateWorkoutLogCommand, object>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkoutLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(CreateWorkoutLogCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
