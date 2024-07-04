using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using Google;

namespace FitLog.Application.Chats.Commands.CreateChatLine;

public record CreateChatLineCommand : IRequest<int>
{
    public int ChatId { get; set; }
    public string ChatLineText { get; set; } = "";
    public string LinkUrl { get; set; } = "";
    public string AttachmentPath { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class CreateChatLineCommandValidator : AbstractValidator<CreateChatLineCommand>
{
    public CreateChatLineCommandValidator()
    {
    }
}

public class CreateChatLineCommandHandler : IRequestHandler<CreateChatLineCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateChatLineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateChatLineCommand request, CancellationToken cancellationToken)
    {
        var chatLine = new ChatLine
        {
            ChatId = request.ChatId,
            ChatLineText = request.ChatLineText,
            LinkUrl = request.LinkUrl,
            AttachmentPath = request.AttachmentPath,
            CreatedAt = request.CreatedAt
        };

        _context.ChatLines.Add(chatLine);
        await _context.SaveChangesAsync(cancellationToken);

        return chatLine.ChatLineId;
    }
}
