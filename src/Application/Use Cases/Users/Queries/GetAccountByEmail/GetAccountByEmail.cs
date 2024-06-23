using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Queries.GetAccountByEmail;

public record GetAccountByEmailQuery : IRequest<object>
{
}

public class GetAccountByEmailQueryValidator : AbstractValidator<GetAccountByEmailQuery>
{
    public GetAccountByEmailQueryValidator()
    {
    }
}

public class GetAccountByEmailQueryHandler : IRequestHandler<GetAccountByEmailQuery, object>
{
    private readonly IApplicationDbContext _context;

    public GetAccountByEmailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(GetAccountByEmailQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
