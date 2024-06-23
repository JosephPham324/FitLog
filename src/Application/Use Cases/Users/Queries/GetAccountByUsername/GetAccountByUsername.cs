using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Queries.GetAccountByUsername;

public record GetAccountByUsernameQuery : IRequest<object>
{
}

public class GetAccountByUsernameQueryValidator : AbstractValidator<GetAccountByUsernameQuery>
{
    public GetAccountByUsernameQueryValidator()
    {
    }
}

public class GetAccountByUsernameQueryHandler : IRequestHandler<GetAccountByUsernameQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetAccountByUsernameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(GetAccountByUsernameQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
