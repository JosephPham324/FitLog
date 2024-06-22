using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Commands.RestrictAccount;

public record RestrictAccountCommand : IRequest<object>
{
}

public class RestrictAccountCommandValidator : AbstractValidator<RestrictAccountCommand>
{
    public RestrictAccountCommandValidator()
    {
    }
}

public class RestrictAccountCommandHandler : IRequestHandler<RestrictAccountCommand, object>
{
    private readonly IApplicationDbContext _context;

    public RestrictAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(RestrictAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
