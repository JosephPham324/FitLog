using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Commands.DeleteAccount;

public record DeleteAccountCommand : IRequest<object>
{
}

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
    }
}

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, object>
{
    private readonly IApplicationDbContext _context;

    public DeleteAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
