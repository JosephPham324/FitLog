using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Exercises.Commands.CreateExercise;

public record CreateExerciseCommand : IRequest<int>
{
}

public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
    }
}

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
