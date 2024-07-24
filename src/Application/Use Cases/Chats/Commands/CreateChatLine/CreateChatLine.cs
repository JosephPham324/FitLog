using FitLog.Application.Chats.Queries.GetChatLinesFromAChat;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using Google;

namespace FitLog.Application.Chats.Commands.CreateChatLine;
public record CreateChatLineResult 
{
    public Result Result { get; set; } = null!;
    public ChatLineDto ChatLine { get; set; } = null!;
}
public record CreateChatLineCommand : IRequest<CreateChatLineResult>
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

public class CreateChatLineCommandHandler : IRequestHandler<CreateChatLineCommand, CreateChatLineResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateChatLineCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreateChatLineResult> Handle(CreateChatLineCommand request, CancellationToken cancellationToken)
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
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateChatLineResult { Result = Result.Successful(), ChatLine = _mapper.Map<ChatLineDto>(chatLine)};
    }
}
