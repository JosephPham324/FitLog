using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.MuscleGroups.Commands.CreateMuscleGroup;

public record CreateMuscleGroupCommand : IRequest<int>
{
}

public class CreateMuscleGroupCommandValidator : AbstractValidator<CreateMuscleGroupCommand>
{
    public CreateMuscleGroupCommandValidator()
    {
    }
}

public class CreateMuscleGroupCommandHandler : IRequestHandler<CreateMuscleGroupCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateMuscleGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public
        //async 
        Task<int> Handle(CreateMuscleGroupCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
