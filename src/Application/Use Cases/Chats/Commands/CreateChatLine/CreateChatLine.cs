using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using Google;

namespace FitLog.Application.Chats.Commands.CreateChatLine;

public record CreateChatLineCommand : IRequest<Result>
{
    public int ChatId { get; set; }
    public string UserId { get; set; } = "";
    public string ChatLineText { get; set; } = "";
    public string? LinkUrl { get; set; } = "";
    public string? AttachmentPath { get; set; } = "";
}

public class CreateChatLineCommandValidator : AbstractValidator<CreateChatLineCommand>
{
    public CreateChatLineCommandValidator()
    {
        RuleFor(v => v.ChatId).GreaterThan(0).WithMessage("Invalid Chat Id.");
        RuleFor(v => v.ChatLineText).NotEmpty().WithMessage("Chat line text is required.");
    }
}

public class CreateChatLineCommandHandler : IRequestHandler<CreateChatLineCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateChatLineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateChatLineCommand request, CancellationToken cancellationToken)
    {
        var chatLine = new ChatLine
        {
            ChatId = request.ChatId,
            CreatedBy = request.UserId,
            ChatLineText = request.ChatLineText,
            LinkUrl = request.LinkUrl,
            AttachmentPath = request.AttachmentPath,
            CreatedAt = DateTime.Now
        };

        _context.ChatLines.Add(chatLine);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            throw new GoogleApiException("Failed to save changes to the database.");
        }
        return Result.Successful();
    }
}
