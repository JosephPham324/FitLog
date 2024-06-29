using FitLog.Application.Common.Interfaces;
using FitLog.Application.Users.Commands.Regiser;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<RegisterResultDTO>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}


public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6); // Assuming a minimum password length
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Role).NotEmpty().Must(Common.ValidationRules.ValidationRules.BeAValidRole).WithMessage("Invalid role specified.");
    }

    
}
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, RegisterResultDTO>
{
    private readonly UserManager<AspNetUser> _userManager;

    public CreateUserCommandHandler(UserManager<AspNetUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterResultDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AspNetUser
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var createUserResult = await _userManager.CreateAsync(user, request.Password);
        if (!createUserResult.Succeeded)
        {
            return new RegisterResultDTO { Success = false, Errors = createUserResult.Errors.Select(e => e.Description) };
        }

        // Adding role to user
        var roleAddResult = await _userManager.AddToRoleAsync(user, request.Role);
        if (!roleAddResult.Succeeded)
        {
            return new RegisterResultDTO { Success = false, Errors = roleAddResult.Errors.Select(e => e.Description) };
        }

        return new RegisterResultDTO { Success = true };
    }
}
