using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Roles.Commands.DeleteRole;

public record DeleteRoleCommand : IRequest<Result>
{
    public int RoleId { get; set; }

}

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
    }
}

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AspNetRoles.FindAsync(request.RoleId);

        if (entity == null)
        {
            return Result.Failure(new List<string> { "Role not found." });
        }

        _context.AspNetRoles.Remove(entity);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Successful();
        }
        catch (Exception ex)
        {
            return Result.Failure(new List<string> { ex.Message });
        }
    }
}
