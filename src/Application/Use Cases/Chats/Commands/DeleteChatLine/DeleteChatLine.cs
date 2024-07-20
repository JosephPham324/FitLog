using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Chats.Commands.DeleteChatLine;

public record DeleteChatLineCommand : IRequest<Result>
{
   public int Id { get; set; }
}

public class DeleteChatLineCommandValidator : AbstractValidator<DeleteChatLineCommand>
{
    public DeleteChatLineCommandValidator()
    {
    }
}

public class DeleteChatLineCommandHandler : IRequestHandler<DeleteChatLineCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteChatLineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteChatLineCommand request, CancellationToken cancellationToken)
    {
        var chatLine = _context.ChatLines.Find(request.Id);
        if (chatLine == null)
        {
            return Result.Failure(["Chat line not found."]);
        }
        _context.ChatLines.Remove(chatLine);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful();
    }
}
