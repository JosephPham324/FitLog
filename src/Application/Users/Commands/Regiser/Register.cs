using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Users.Commands.Regiser;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Application.Users.Commands.Register;
public record RegisterCommand : IRequest<RegisterResultDTO>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty; // Optional, use if different from email
}
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResultDTO>
{
    private readonly UserManager<AspNetUser> _userManager;

    public RegisterCommandHandler(UserManager<AspNetUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterResultDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new AspNetUser
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            return new RegisterResultDTO { Success = true };
        }
        else
        {
            return new RegisterResultDTO { Success = false, Errors = result.Errors.Select(e => e.Description) };
        }
    }
}
