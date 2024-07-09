using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.ExerciseLogs.Commands.CreateExerciseLogs;

public record CreateExerciseLogsCommand : IRequest<object>
{
}

public class CreateExerciseLogsCommandValidator : AbstractValidator<CreateExerciseLogsCommand>
{
    public CreateExerciseLogsCommandValidator()
    {
    }
}

public class CreateExerciseLogsCommandHandler : IRequestHandler<CreateExerciseLogsCommand, object>
{
    private readonly IApplicationDbContext _context;

    public CreateExerciseLogsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(CreateExerciseLogsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
