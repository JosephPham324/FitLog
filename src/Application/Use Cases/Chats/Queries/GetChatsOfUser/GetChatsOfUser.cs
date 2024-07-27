using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.Chats.Queries.GetChatsOfUser;

public record GetChatsOfUserQuery(string UserId) : IRequest<List<Chat>>;

public class GetChatsOfUserQueryValidator : AbstractValidator<GetChatsOfUserQuery>
{
    public GetChatsOfUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetChatsOfUserQueryHandler : IRequestHandler<GetChatsOfUserQuery, List<Chat>>
{
    private readonly IApplicationDbContext _context;

    public GetChatsOfUserQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Chat>> Handle(GetChatsOfUserQuery request, CancellationToken cancellationToken)
    {
        return await _context.Chats
            .Where
            (c => c.CreatedBy.Equals(request.UserId) || c.TargetUserId.Equals(request.UserId))
            .ToListAsync();
    }
}
