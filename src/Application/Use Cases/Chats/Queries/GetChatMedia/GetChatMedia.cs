using FitLog.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Chats.Queries.GetChatMedia;

public record GetChatMediaQuery : IRequest<List<string>>
{
    public int ChatId { get; set; }
}

public class GetChatMediaQueryValidator : AbstractValidator<GetChatMediaQuery>
{
    public GetChatMediaQueryValidator()
    {
        RuleFor(v => v.ChatId).GreaterThan(0).WithMessage("Invalid Chat Id.");
    }
}

public class GetChatMediaQueryHandler : IRequestHandler<GetChatMediaQuery, List<string>>
{
    private readonly IApplicationDbContext _context;

    public GetChatMediaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(GetChatMediaQuery request, CancellationToken cancellationToken)
    {
        return await _context.ChatLines
                            .Include(cl => cl.CreatedByNavigation)
            .Where(cl => cl.ChatId == request.ChatId && cl.AttachmentPath != null)
            .Select(cl => cl.AttachmentPath ?? "")
            .ToListAsync(cancellationToken);
    }
}
