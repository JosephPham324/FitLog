using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Exercises.Commands.UpdateExercise;

public record UpdateExerciseCommand : IRequest<int>
{
}

public class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
{
    public UpdateExerciseCommandValidator()
    {
    }
}

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public UpdateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public 
        //async 
        Task<int> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
