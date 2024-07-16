using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Queries.GetPublicTemplates;

public record GetPublicTemplatesQuery : IRequest<PaginatedList<WorkoutTemplateListDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPublicTemplatesQueryValidator : AbstractValidator<GetPublicTemplatesQuery>
{
    public GetPublicTemplatesQueryValidator()
    {
        RuleFor(v => v.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(v => v.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");
    }
}

public class GetPublicTemplatesQueryHandler : IRequestHandler<GetPublicTemplatesQuery, PaginatedList<WorkoutTemplateListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPublicTemplatesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorkoutTemplateListDto>> Handle(GetPublicTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _context.WorkoutTemplates
            .Include(wt => wt.CreatedByNavigation)
            .Where(wt =>wt.IsPublic == true)
            .OrderBy(wt => wt.TemplateName)
            .ProjectTo<WorkoutTemplateListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
