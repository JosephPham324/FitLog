using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using Google;

namespace FitLog.Application.Chats.Commands.CreateChat;

public record CreateChatCommand : IRequest<int>
{
}

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
    }
}

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateChatCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat
        {
            CreatedAt = DateTime.UtcNow
        };

        _context.Chats.Add(chat);
        await _context.SaveChangesAsync(cancellationToken);

        return chat.ChatId;
    }
}
