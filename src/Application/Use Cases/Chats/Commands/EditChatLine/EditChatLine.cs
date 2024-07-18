using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Chats.Commands.EditChatLine;

public record EditChatLineCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string ChatLineText { get; set; } = "";
    public string? LinkUrl { get; set; } = "";
    public string? AttachmentPath { get; set; } = "";
}

public class EditChatLineCommandValidator : AbstractValidator<EditChatLineCommand>
{
    public EditChatLineCommandValidator()
    {
        RuleFor(v => v.ChatLineText).NotEmpty().WithMessage("Chat line text is required.");
        RuleFor(v => v.Id).GreaterThan(0).WithMessage("Invalid ChatLine Id.");
    }
}

public class EditChatLineCommandHandler : IRequestHandler<EditChatLineCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public EditChatLineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(EditChatLineCommand request, CancellationToken cancellationToken)
    {
        var chatLine = await _context.ChatLines.FindAsync(new object[] { request.Id }, cancellationToken);

        if (chatLine == null)
        {
            return Result.Failure(new string[] { "ChatLine not found." });
        }

        chatLine.ChatLineText = request.ChatLineText;
        chatLine.LinkUrl = request.LinkUrl;
        chatLine.AttachmentPath = request.AttachmentPath;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Successful();
        }
        catch (Exception ex)
        {
            return Result.Failure(new string[] { $"An error occurred while updating the chat line: {ex.Message}" });
        }
    }
}
