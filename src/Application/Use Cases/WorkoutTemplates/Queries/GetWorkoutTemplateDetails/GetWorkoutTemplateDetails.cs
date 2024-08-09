using FitLog.Application.Common.Interfaces;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Queries.GetWorkoutTemplateDetails;
public class GetWorkoutTemplateDetailsQuery : IRequest<WorkoutTemplateDetailsDto>
{
    public int Id { get; set; }
}

public class GetWorkoutTemplateDetailsQueryValidator : AbstractValidator<GetWorkoutTemplateDetailsQuery>
{
    public GetWorkoutTemplateDetailsQueryValidator()
    {
        RuleFor(v => v.Id)
           .GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}

public class GetWorkoutTemplateDetailsQueryHandler : IRequestHandler<GetWorkoutTemplateDetailsQuery, WorkoutTemplateDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkoutTemplateDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkoutTemplateDetailsDto> Handle(GetWorkoutTemplateDetailsQuery request, CancellationToken cancellationToken)
    {
        var query = await _context.WorkoutTemplates
            .Include(wt => wt.CreatedByNavigation)
            .Include(wt => wt.LastModifiedByNavigation)
            .Include(wt => wt.WorkoutTemplateExercises)
                .ThenInclude(wte=>wte.Exercise)
            .FirstOrDefaultAsync(wt => wt.Id == request.Id, cancellationToken);
        var res = _mapper.Map<WorkoutTemplateDetailsDto>(query);
        return res;
    }
}
