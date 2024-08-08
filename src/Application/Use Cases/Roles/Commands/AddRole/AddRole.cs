using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.Roles.Commands.AddRole;

public record AddRoleCommand : IRequest<Result>
{
    public string RoleName { get; set; } = string.Empty;
    public string RoleDesc { get; set; } = string.Empty;

}


public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(v => v.RoleName)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(v => v.RoleDesc)
            .MaximumLength(200);

    }
}

public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public AddRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RoleName))
        {
            return Result.Failure(new List<string> { "Role name cannot be empty." });
        }

        var entity = new AspNetRole(request.RoleName)
        {
            Id  = Guid.NewGuid().ToString(),
            RoleDesc = request.RoleDesc
        };

        _context.AspNetRoles.Add(entity);

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
