using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Chats.Commands.DeleteChatLine;

public record DeleteChatLineCommand : IRequest<object>
{
}

public class DeleteChatLineCommandValidator : AbstractValidator<DeleteChatLineCommand>
{
    public DeleteChatLineCommandValidator()
    {
    }
}

public class DeleteChatLineCommandHandler : IRequestHandler<DeleteChatLineCommand, object>
{
    private readonly IApplicationDbContext _context;

    public DeleteChatLineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(DeleteChatLineCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
