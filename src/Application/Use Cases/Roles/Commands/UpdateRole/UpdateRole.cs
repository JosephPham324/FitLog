using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

namespace FitLog.Application.Roles.Commands.UpdateRole;

public record UpdateRoleCommand : IRequest<Result>
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string RoleDescription { get; set; } = string.Empty;
}

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(v => v.RoleName)
            .MaximumLength(200)
            .NotEmpty();
        RuleFor(v => v.RoleId)
            .GreaterThan(0);    
        RuleFor(v => v.RoleName)
            .MaximumLength(200)
            .NotEmpty();
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

        entity.Name = request.RoleName;

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
