using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;

public record GetWorkoutLogsWithPaginationQuery : IRequest<PaginatedList<WorkoutLogDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetWorkoutLogsWithPaginationQueryValidator : AbstractValidator<GetWorkoutLogsWithPaginationQuery>
{
    public GetWorkoutLogsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1.");
    }
}


public class GetWorkoutLogsWithPaginationQueryHandler : IRequestHandler<GetWorkoutLogsWithPaginationQuery, PaginatedList<WorkoutLogDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkoutLogsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorkoutLogDTO>> Handle(GetWorkoutLogsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.WorkoutLogs
            .OrderBy(w => w.Id)
            .ProjectTo<WorkoutLogDTO>(_mapper.ConfigurationProvider);

        return await PaginatedList<WorkoutLogDTO>.CreateAsync(query.AsNoTracking(), request.PageNumber, request.PageSize);
    }
}

