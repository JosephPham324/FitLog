using FitLog.Application.Common.Interfaces;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;

namespace FitLog.Application.WorkoutPrograms.Queries.GetWorkoutProgramsList;

public record GetWorkoutProgramsListQuery : IRequest<List<WorkoutProgramListDTO>>
{
}

public class GetWorkoutProgramsListQueryValidator : AbstractValidator<GetWorkoutProgramsListQuery>
{
    public GetWorkoutProgramsListQueryValidator()
    {
    }
}

public class GetWorkoutProgramsListQueryHandler : IRequestHandler<GetWorkoutProgramsListQuery, List<WorkoutProgramListDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkoutProgramsListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WorkoutProgramListDTO>> Handle(GetWorkoutProgramsListQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Programs
            .Include(wp => wp.User)
        .ToListAsync(cancellationToken);

        return _mapper.Map<List<WorkoutProgramListDTO>>(entities);
    }
}
