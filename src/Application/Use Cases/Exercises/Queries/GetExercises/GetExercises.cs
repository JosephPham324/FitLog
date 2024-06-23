using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Exercises.Queries.GetExercises;

public record GetExercisesWithPaginationQuery : IRequest<PaginatedList<ExerciseDTO>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetExercisesWithPaginationQueryValidator : AbstractValidator<GetExercisesWithPaginationQuery>
{
    public GetExercisesWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1.");
    }
}

public class GetExercisesWithPaginationQueryHandler : IRequestHandler<GetExercisesWithPaginationQuery, PaginatedList<ExerciseDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetExercisesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<ExerciseDTO>> Handle(GetExercisesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Exercises
            .OrderBy(e => e.ExerciseName)
            .ProjectTo<ExerciseDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return query;
    }
}
