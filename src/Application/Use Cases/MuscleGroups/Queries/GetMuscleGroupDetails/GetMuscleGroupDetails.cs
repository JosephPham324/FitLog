using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.MuscleGroups.Queries.GetMuscleGroupDetails;

public record GetMuscleGroupDetailsQuery : IRequest<MuscleGroupDTO>
{
}

public class GetMuscleGroupDetailsQueryValidator : AbstractValidator<GetMuscleGroupDetailsQuery>
{
    public GetMuscleGroupDetailsQueryValidator()
    {
    }
}

public class GetMuscleGroupDetailsQueryHandler : IRequestHandler<GetMuscleGroupDetailsQuery, MuscleGroupDTO>
{
    private readonly IApplicationDbContext _context;

    public GetMuscleGroupDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public 
        //async
        Task<MuscleGroupDTO> Handle(GetMuscleGroupDetailsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
