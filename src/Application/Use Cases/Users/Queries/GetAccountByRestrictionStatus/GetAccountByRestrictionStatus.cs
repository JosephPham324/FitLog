using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Queries.GetAccountByRestrictionStatus;

public record GetAccountByRestrictionStatusQuery : IRequest<object>
{
}

public class GetAccountByRestrictionStatusQueryValidator : AbstractValidator<GetAccountByRestrictionStatusQuery>
{
    public GetAccountByRestrictionStatusQueryValidator()
    {
    }
}

public class GetAccountByRestrictionStatusQueryHandler : IRequestHandler<GetAccountByRestrictionStatusQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetAccountByRestrictionStatusQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(GetAccountByRestrictionStatusQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
