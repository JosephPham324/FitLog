using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using FitLog.Application.Common.Mappings;

namespace FitLog.Application.WorkoutTemplates.Queries.GetPersonalTemplate;

public record GetPersonalTemplatesQuery : IRequest<PaginatedList<WorkoutTemplate>>
{
    public string UserToken { get; init; } = ""; 
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

public class GetPersonalTemplatesQueryHandler : IRequestHandler<GetPersonalTemplatesQuery, PaginatedList<WorkoutTemplate>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserTokenService _currentUserService;

    public GetPersonalTemplatesQueryHandler(IApplicationDbContext context, IUserTokenService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<PaginatedList<WorkoutTemplate>> Handle(GetPersonalTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _context.WorkoutTemplates
            .Where(wt => wt.CreatedBy == _currentUserService.GetUserIdFromGivenToken(request.UserToken) && wt.IsPublic == false)
            .OrderBy(wt => wt.TemplateName)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
