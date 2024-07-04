using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<object>
{
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, object>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
