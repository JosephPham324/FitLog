using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.MuscleGroups.Commands.DeleteMuscleGroup;

public record DeleteMuscleGroupCommand : IRequest<int>
{
}

public class DeleteMuscleGroupCommandValidator : AbstractValidator<DeleteMuscleGroupCommand>
{
    public DeleteMuscleGroupCommandValidator()
    {
    }
}

public class DeleteMuscleGroupCommandHandler : IRequestHandler<DeleteMuscleGroupCommand, int>
{
    private readonly IApplicationDbContext _context;

    public DeleteMuscleGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public 
        //async 
        Task<int> Handle(DeleteMuscleGroupCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
