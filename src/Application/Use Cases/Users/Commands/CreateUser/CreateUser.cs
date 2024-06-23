using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<object>
{
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, object>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
