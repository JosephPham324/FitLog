using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.MuscleGroups.Queries.GetMuscleGroupsListWithPagination;

public record GetMuscleGroupsListWithPaginationQuery : IRequest<PaginatedList<MuscleGroupDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}


public class GetMuscleGroupsListWithPaginationQueryValidator : AbstractValidator<GetMuscleGroupsListWithPaginationQuery>
{
    public GetMuscleGroupsListWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1.");
    }
}

public class GetMuscleGroupsListWithPaginationQueryHandler : IRequestHandler<GetMuscleGroupsListWithPaginationQuery, PaginatedList<MuscleGroupDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMuscleGroupsListWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MuscleGroupDTO>> Handle(GetMuscleGroupsListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MuscleGroups
            .OrderBy(mg => mg.MuscleGroupName)
            .Select(mg => new MuscleGroupDTO
            {
                MuscleGroupId = mg.MuscleGroupId,
                MuscleGroupName = mg.MuscleGroupName,
                ImageUrl = mg.ImageUrl
            });

        return await PaginatedList<MuscleGroupDTO>.CreateAsync(query.AsNoTracking(), request.PageNumber, request.PageSize);
    }
}
