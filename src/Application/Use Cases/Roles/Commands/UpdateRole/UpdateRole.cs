using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Roles.Commands.UpdateRole;

public record UpdateRoleCommand : IRequest<Result>
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleDescription { get; set; } = string.Empty;
}

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
       
        RuleFor(v => v.RoleId)
            .MaximumLength(200);    
    }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AspNetRoles.FindAsync(request.RoleId);

        if (entity == null)
        {
            return Result.Failure(new List<string> { "Role not found." });
        }

        entity.RoleDesc = request.RoleDescription;

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
