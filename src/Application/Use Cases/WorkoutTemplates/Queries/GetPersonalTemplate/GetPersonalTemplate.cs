using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Use_Cases.WorkoutTemplates.Queries;

namespace FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;

public record GetPersonalTemplatesQuery : IRequest<PaginatedList<WorkoutTemplateListDto>>
{
    public string UserId { get; init; } = ""; 
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPersonalTemplatesQueryValidator : AbstractValidator<GetPersonalTemplatesQuery>
{
    public GetPersonalTemplatesQueryValidator()
    {
        RuleFor(v => v.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(v => v.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");
    }
}

public class GetPersonalTemplatesQueryHandler : IRequestHandler<GetPersonalTemplatesQuery, PaginatedList<WorkoutTemplateListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPersonalTemplatesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<WorkoutTemplateListDto>> Handle(GetPersonalTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _context.WorkoutTemplates
            .Where(wt => wt.CreatedBy == request.UserId)
            .OrderBy(wt => wt.TemplateName)
            .ProjectTo<WorkoutTemplateListDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
