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
    private readonly IMapper _mapper;

    public GetChatLinesFromAChatQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ChatLineDto>> Handle(GetChatLinesFromAChatQuery request, CancellationToken cancellationToken)
    {
        return await _context.ChatLines
            .Where(cl => cl.ChatId == request.ChatId)
                .Include(cl => cl.CreatedByNavigation)
            .ProjectTo<ChatLineDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
