using FitLog.Application.Chats.Queries.GetChatLinesFromAChat;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.Chats.Commands.EditChatLine;

public record EditChatLineResult
{
    public Result Result { get; set; } = null!;
    public ChatLineDto ChatLine { get; set; } = null!;
}
public record EditChatLineCommand : IRequest<EditChatLineResult>
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

public class EditChatLineCommandHandler : IRequestHandler<EditChatLineCommand, EditChatLineResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EditChatLineCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EditChatLineResult> Handle(EditChatLineCommand request, CancellationToken cancellationToken)
    {
        var chatLine = await _context.ChatLines.FindAsync(new object[] { request.Id }, cancellationToken);
        EditChatLineResult result = new EditChatLineResult()
        {
            ChatLine = _mapper.Map<ChatLineDto>(chatLine)
        };
        if (chatLine == null)
        {
            result.Result = Result.Failure(new string[] { "ChatLine not found." });
            return result;
        }

        chatLine.ChatLineText = request.ChatLineText;
        chatLine.LinkUrl = request.LinkUrl;
        chatLine.AttachmentPath = request.AttachmentPath;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            result.Result= Result.Successful();
        }
        catch (Exception ex)
        {
            result.Result = Result.Failure(new string[] { $"An error occurred while updating the chat line: {ex.Message}" });
        }
        return result;
    }
}
