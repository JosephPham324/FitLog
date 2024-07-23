using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Commands.RecoverAccount;
using FitLog.Application.Users.Commands.Regiser;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FitLog.Application.Users.Commands.Register;
public record RegisterCommand : IRequest<Result>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty; // Optional, use if different from email
    public string PhoneNumber { get; init; } = string.Empty; // Optional
}
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly UserManager<AspNetUser> _userManager;

    public RegisterCommandValidator(UserManager<AspNetUser> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MustAsync(BeUniqueUsername).WithMessage("The specified username already exists.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.PhoneNumber)
                .Matches(@"(^\+?[1-9]\d{1,14}$)|(^(0|\+84)(3[2-9]|5[6|8|9]|7[0|6|7|8|9]|8[1-6|8-9]|9[0-9])[0-9]{7}$)")//International format
                //.Matches(@"^(0|\+84)(3[2-9]|5[6|8|9]|7[0|6|7|8|9]|8[1-6|8-9]|9[0-9])[0-9]{7}$") // Vietnam format
                .WithMessage("Invalid phone number format."); 
    }

    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user == null;
    }
}
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
{
    private readonly UserManager<AspNetUser> _userManager;

    public RegisterCommandHandler(UserManager<AspNetUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new AspNetUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            IsDeleted = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            return Result.Successful();
        }
        else
        {
            return Result.Failure(result.Errors.Select(e=>e.Description));
        }
    }
}
