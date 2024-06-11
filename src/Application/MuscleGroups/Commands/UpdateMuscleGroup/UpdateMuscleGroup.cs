using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.MuscleGroups.Commands.UpdateMuscleGroup;

public record UpdateMuscleGroupCommand : IRequest<int>
{
}

public class UpdateMuscleGroupCommandValidator : AbstractValidator<UpdateMuscleGroupCommand>
{
    public UpdateMuscleGroupCommandValidator()
    {
    }
}

public class UpdateMuscleGroupCommandHandler : IRequestHandler<UpdateMuscleGroupCommand, int>
{
    private readonly IApplicationDbContext _context;

    public UpdateMuscleGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public 
        //async 
        Task<int> Handle(UpdateMuscleGroupCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
