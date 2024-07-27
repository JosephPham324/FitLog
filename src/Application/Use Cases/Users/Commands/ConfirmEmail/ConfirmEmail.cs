using System;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using MediatR;

namespace FitLog.Application.Users.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string Token, string Email) : IRequest<Result>;

    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");
        }
    }

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly UserManager<AspNetUser> _userManager;

        public ConfirmEmailCommandHandler(UserManager<AspNetUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure(new string[] { "No account associated with this email." });
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (result.Succeeded)
            {
                return Result.Successful();
            }
            else
            {
                return Result.Failure(result.Errors.Select(e => e.Description));
            }
        }
    }
}
