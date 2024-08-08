using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Commands.Regiser;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<Result>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}


public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly UserManager<AspNetUser> _userManager;
    public CreateUserCommandValidator(UserManager<AspNetUser> userManager)
    {
        _userManager = userManager;
        RuleFor(x => x.UserName)
               .NotEmpty().WithMessage("Username is required.")
               .MustAsync(BeUniqueUsername).WithMessage("The specified username already exists.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmail).WithMessage("The specified email is already in use.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user == null;
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null;
    }

}
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
{
    private readonly UserManager<AspNetUser> _userManager;

    public CreateUserCommandHandler(UserManager<AspNetUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AspNetUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            IsDeleted = false
        };

        var createUserResult = await _userManager.CreateAsync(user, request.Password);
        if (!createUserResult.Succeeded)
        {
            return Result.Failure(createUserResult.Errors.Select(e => e.Description));
        }

        // Adding role to user
        var roleAddResult = await _userManager.AddToRoleAsync(user, request.Role);
        if (!roleAddResult.Succeeded)
        {
            return Result.Failure(roleAddResult.Errors.Select(e => e.Description));
        }

        return Result.Successful();
    }
}
