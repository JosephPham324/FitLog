using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Queries.FilterWorkoutTemplates;

public record FilterWorkoutTemplatesQuery : IRequest<PaginatedList<WorkoutTemplateListDto>>
{
    public string TemplateName { get; set; } = string.Empty;
    public string? CreatorName { get; set; } = null;
    public string? MinDuration { get; set; } = null;
    public string? MaxDuration { get; set; } = null;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class FilterWorkoutTemplatesQueryValidator : AbstractValidator<FilterWorkoutTemplatesQuery>
{
    public FilterWorkoutTemplatesQueryValidator()
    {
        RuleFor(v => v.PageNumber)
           .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(v => v.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");

    }
}

public class FilterWorkoutTemplatesQueryHandler : IRequestHandler<FilterWorkoutTemplatesQuery, PaginatedList<WorkoutTemplateListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public FilterWorkoutTemplatesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorkoutTemplateListDto>> Handle(FilterWorkoutTemplatesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.WorkoutTemplates.AsQueryable();

        if (!string.IsNullOrEmpty(request.TemplateName))
        {
            query = query.Where(wt => wt.TemplateName != null && wt.TemplateName.Contains(request.TemplateName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(request.CreatorName))
        {
            query = query.Where(wt => wt.CreatedByNavigation.UserName != null && wt.CreatedByNavigation.UserName.Contains(request.CreatorName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(request.MinDuration))
        {
            query = query.Where(wt => string.Compare(wt.Duration, request.MinDuration) >= 0);
        }

        if (!string.IsNullOrEmpty(request.MaxDuration))
        {
            query = query.Where(wt => string.Compare(wt.Duration, request.MaxDuration) <= 0);
        }

        var workoutTemplates = await query
            .ProjectTo<WorkoutTemplateListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return workoutTemplates;
    }
}
