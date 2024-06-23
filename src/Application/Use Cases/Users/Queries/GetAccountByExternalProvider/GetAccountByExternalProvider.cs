using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Queries.GetAccountByExternalProvider;

public record GetAccountByExternalProviderQuery : IRequest<object>
{
}

public class GetAccountByExternalProviderQueryValidator : AbstractValidator<GetAccountByExternalProviderQuery>
{
    public GetAccountByExternalProviderQueryValidator()
    {
    }
}

public class GetAccountByExternalProviderQueryHandler : IRequestHandler<GetAccountByExternalProviderQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetAccountByExternalProviderQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(GetAccountByExternalProviderQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
