using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Exercises.Commands.DeleteExercise;

public record DeleteExerciseCommand : IRequest<int>
{
}

public class DeleteExerciseCommandValidator : AbstractValidator<DeleteExerciseCommand>
{
    public DeleteExerciseCommandValidator()
    {
    }
}

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public DeleteExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
