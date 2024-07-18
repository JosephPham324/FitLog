using FitLog.Application.Common.Interfaces;
using FitLog.Application.Use_Cases.WorkoutPrograms.DTOs;

namespace FitLog.Application.WorkoutPrograms.Queries.GetWorkoutProgramDetails;

public record GetWorkoutProgramDetailsQuery : IRequest<WorkoutProgramDetailsDto>
{
    public int Id { get; set; }
}

public class GetWorkoutProgramDetailsQueryValidator : AbstractValidator<GetWorkoutProgramDetailsQuery>
{
    public GetWorkoutProgramDetailsQueryValidator()
    {
    }
}

public class GetWorkoutProgramDetailsQueryHandler : IRequestHandler<GetWorkoutProgramDetailsQuery, WorkoutProgramDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkoutProgramDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkoutProgramDetailsDto> Handle(GetWorkoutProgramDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Programs
            .Include(wp => wp.User)
            .Include(wp => wp.ProgramWorkouts)
                .ThenInclude(pw => pw.WorkoutTemplate)
                .ThenInclude(wt => wt!= null ?  wt.WorkoutTemplateExercises : null)
            .FirstOrDefaultAsync(wp => wp.ProgramId == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(request.Id+"","Program not found");
        }

        return _mapper.Map<WorkoutProgramDetailsDto>(entity);
    }
}
