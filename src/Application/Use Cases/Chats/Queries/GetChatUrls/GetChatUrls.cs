using FitLog.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Chats.Queries.GetChatUrls;

public record GetChatUrlsQuery : IRequest<List<string>>
{
    public int ChatId { get; set; }
}

public class GetChatUrlsQueryValidator : AbstractValidator<GetChatUrlsQuery>
{
    public GetChatUrlsQueryValidator()
    {
        RuleFor(v => v.ChatId).GreaterThan(0).WithMessage("Invalid Chat Id.");
    }
}

public class GetChatUrlsQueryHandler : IRequestHandler<GetChatUrlsQuery, List<string>>
{
    private readonly IApplicationDbContext _context;

    public GetChatUrlsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(GetChatUrlsQuery request, CancellationToken cancellationToken)
    {
        return await _context.ChatLines
                            .Include(cl => cl.CreatedByNavigation)
            .Where(cl => cl.ChatId == request.ChatId && cl.LinkUrl != null)
            .Select(cl => cl.LinkUrl ?? "")
            .ToListAsync(cancellationToken);
    }
}
