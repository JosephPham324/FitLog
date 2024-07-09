using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Users.Commands.DeleteAccount;

public record DeleteAccountCommand(string UserId) : IRequest<Result>; // Returns true if deletion is successful


public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.AspNetUsers.FindAsync(request.UserId);

        if (user == null)
        {
            return Result.Failure([$"Account with ID {request.UserId} not found"]);
        }

        // Perform a soft delete by setting the IsDeleted flag
        user.IsDeleted = true;

        _context.AspNetUsers.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}

