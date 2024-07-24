using FitLog.Application.Chats.Queries.GetChatLinesFromAChat;
using FitLog.Application.Common.Interfaces;

namespace FitLog.Application.Chats.Queries.SearchChat;

public record SearchChatQuery : IRequest<List<ChatLineDto>>
{
    public int ChatId { get; set; }
    public string Keyword { get; set; } = "";
}

public class SearchChatQueryValidator : AbstractValidator<SearchChatQuery>
{
    public SearchChatQueryValidator()
    {
        RuleFor(v => v.ChatId).GreaterThan(0).WithMessage("Invalid Chat Id.");
        RuleFor(v => v.Keyword).NotEmpty().WithMessage("Search keyword is required.");
    }
}

public class SearchChatQueryHandler : IRequestHandler<SearchChatQuery, List<ChatLineDto>>
{
    private readonly IApplicationDbContext _context;

    public SearchChatQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChatLineDto>> Handle(SearchChatQuery request, CancellationToken cancellationToken)
    {
        return await _context.ChatLines.Include(cl => cl.CreatedByNavigation)
            .Where(cl => cl.ChatId == request.ChatId &&
                         (cl.ChatLineText != null && cl.ChatLineText.Contains(request.Keyword) ||
                          cl.LinkUrl != null && cl.LinkUrl.Contains(request.Keyword) ||
                          cl.AttachmentPath != null && cl.AttachmentPath.Contains(request.Keyword)))
            .Select(cl => new ChatLineDto
            {
                ChatLineId = cl.ChatLineId,
                ChatLineText = cl.ChatLineText ?? "",
                LinkUrl = cl.LinkUrl ?? "",
                AttachmentPath = cl.AttachmentPath ?? "",
                CreatedAt = cl.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
