using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Queries.GetPublicTemplates;

public record GetPublicTemplatesQuery : IRequest<PaginatedList<WorkoutTemplate>>
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

public class GetPublicTemplatesQueryHandler : IRequestHandler<GetPublicTemplatesQuery, PaginatedList<WorkoutTemplate>>
{
    private readonly IApplicationDbContext _context;

    public GetPublicTemplatesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<WorkoutTemplate>> Handle(GetPublicTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _context.WorkoutTemplates
            .Where(wt =>wt.IsPublic == true)
            .OrderBy(wt => wt.TemplateName)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
