using FitLog.Application.Common.Interfaces;
using Google;

namespace FitLog.Application.Chats.Queries.GetChatLinesFromAChat;

public record GetChatLinesFromAChatQuery : IRequest<List<ChatLineDto>>
{
    public int ChatId { get; set; }
}


public class GetChatLinesFromAChatQueryValidator : AbstractValidator<GetChatLinesFromAChatQuery>
{
    public GetChatLinesFromAChatQueryValidator()
    {
    }
}

public class GetChatLinesFromAChatQueryHandler : IRequestHandler<GetChatLinesFromAChatQuery, List<ChatLineDto>>
{
    private readonly IApplicationDbContext _context;

    public GetChatLinesFromAChatQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChatLineDto>> Handle(GetChatLinesFromAChatQuery request, CancellationToken cancellationToken)
    {
        return await _context.ChatLines
            .Where(cl => cl.ChatId == request.ChatId)
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
