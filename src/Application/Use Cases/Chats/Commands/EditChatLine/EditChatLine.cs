using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Chats.Commands.EditChatLine;

public record EditChatLineCommand : IRequest<object>
{
}

public class EditChatLineCommandValidator : AbstractValidator<EditChatLineCommand>
{
    public EditChatLineCommandValidator()
    {
    }
}

public class EditChatLineCommandHandler : IRequestHandler<EditChatLineCommand, object>
{
    private readonly IApplicationDbContext _context;

    public EditChatLineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<object> Handle(EditChatLineCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
