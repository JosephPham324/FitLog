using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Commands.RecoverAccount;

public record RecoverAccountCommand : IRequest<object>
{
}

public class RecoverAccountCommandValidator : AbstractValidator<RecoverAccountCommand>
{
    public RecoverAccountCommandValidator()
    {
    }
}

public class RecoverAccountCommandHandler : IRequestHandler<RecoverAccountCommand, object>
{
    private readonly IApplicationDbContext _context;

    public RecoverAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(RecoverAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
