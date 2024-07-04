using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Queries.GetAccountsByRole;

public record GetAccountsByRoleQuery : IRequest<object>
{
}

public class GetAccountsByRoleQueryValidator : AbstractValidator<GetAccountsByRoleQuery>
{
    public GetAccountsByRoleQueryValidator()
    {
    }
}

public class GetAccountsByRoleQueryHandler : IRequestHandler<GetAccountsByRoleQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetAccountsByRoleQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(GetAccountsByRoleQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
