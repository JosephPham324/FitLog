using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using Google;

namespace FitLog.Application.Chats.Commands.CreateChat;

public record CreateChatCommand : IRequest<Result>
{
}

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
    }
}

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateChatCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat
        {
            CreatedAt = DateTime.UtcNow
        };

        _context.Chats.Add(chat);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
