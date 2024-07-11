using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Queries.SearchWorkoutTemplateByName;

public record SearchWorkoutTemplateByNameQuery : IRequest<PaginatedList<WorkoutTemplateListDto>>
{
    public string TemplateName { get; set; } = "";
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
}

public class SearchWorkoutTemplateByNameQueryValidator : AbstractValidator<SearchWorkoutTemplateByNameQuery>
{
    public SearchWorkoutTemplateByNameQueryValidator()
    {
    }
}

public class SearchWorkoutTemplateByNameQueryHandler : IRequestHandler<SearchWorkoutTemplateByNameQuery, PaginatedList<WorkoutTemplateListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchWorkoutTemplateByNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorkoutTemplateListDto>> Handle(SearchWorkoutTemplateByNameQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber ?? 1;
        int pageSize = request.PageSize ?? 10;
        return await _context.WorkoutTemplates
               .Where(wt => wt.TemplateName != null && wt.TemplateName.Contains(request.TemplateName, StringComparison.OrdinalIgnoreCase))
               .ProjectTo<WorkoutTemplateListDto>(_mapper.ConfigurationProvider)
               .PaginatedListAsync(pageNumber, pageSize);
    }
}
