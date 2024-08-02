using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupDetails;
using FitLog.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace FitLog.Application.MuscleGroups.Queries.PaginatedSearchMuscleGroup;



public record PaginatedSearchMuscleGroupQuery : IRequest<PaginatedList<MuscleGroupDTO>>
{
    public string? MuscleGroupName { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class PaginatedSearchMuscleGroupQueryValidator : AbstractValidator<PaginatedSearchMuscleGroupQuery>
{
    public PaginatedSearchMuscleGroupQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1.");
    }
}
public class PaginatedSearchMuscleGroupQueryHandler : IRequestHandler<PaginatedSearchMuscleGroupQuery, PaginatedList<MuscleGroupDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PaginatedSearchMuscleGroupQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MuscleGroupDTO>> Handle(PaginatedSearchMuscleGroupQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MuscleGroups.AsQueryable();

        if (!string.IsNullOrEmpty(request.MuscleGroupName))
        {
            query = query.Where(mg => EF.Functions.Like(mg.MuscleGroupName, $"%{request.MuscleGroupName}%"));
        }

        return await  query.OrderBy(mg => mg.MuscleGroupName)
                       .ProjectTo<MuscleGroupDTO>(_mapper.ConfigurationProvider)
                       .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
