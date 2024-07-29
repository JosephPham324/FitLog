using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Users.Commands.ResetPassword
{
    public record AuthenticatedResetPasswordCommandDto
    {
        public string OldPassword { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
    }
    public record AuthenticatedResetPasswordCommand : IRequest<Result>
    {
        [JsonIgnore]
        public string UserId { get; init; } = string.Empty;
        public string OldPassword { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
    }

    public class AuthenticatedResetPasswordValidator : AbstractValidator<AuthenticatedResetPasswordCommand>
    {
        public AuthenticatedResetPasswordValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Old password is required.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required.");
            RuleFor(x => x.NewPassword).MinimumLength(6).WithMessage("New password must be at least 6 characters long.");
        }
    }

    public class AuthenticatedResetPasswordCommandHandler : IRequestHandler<AuthenticatedResetPasswordCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<AspNetUser> _userManager;

        public AuthenticatedResetPasswordCommandHandler(IApplicationDbContext context, UserManager<AspNetUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result> Handle(AuthenticatedResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure(["User not found."]);
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (!passwordCheck)
            {
                return Result.Failure(["Old password is incorrect."]);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (result.Succeeded)
            {
                return Result.Successful();
            }

            return Result.Failure((IEnumerable<string>)result.Errors);
        }
    }
}
