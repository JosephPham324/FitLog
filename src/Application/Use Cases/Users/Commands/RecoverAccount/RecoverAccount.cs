using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Users.Commands.RecoverAccount
{
    public record RecoverAccountCommand(string Email) : IRequest<Result>;

    public class RecoverAccountCommandValidator : AbstractValidator<RecoverAccountCommand>
    {
        public RecoverAccountCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email is required for account recovery.");
        }
    }

    public class RecoverAccountCommandHandler : IRequestHandler<RecoverAccountCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly INotificationService _notificationService;

        public RecoverAccountCommandHandler(UserManager<AspNetUser> userManager, IEmailService emailService, IApplicationDbContext context, INotificationService notificationService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(RecoverAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user == null)
            {
                return Result.Failure(new string[] { "No account associated with this email." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var recoveryLink = $"https://localhost:44777/recover?token={token}&email={user.Email}";
            var emailSubject = "Recover Your Account";
            var emailBody = $"Please click on the link to recover your account: {recoveryLink}";

            await _emailService.SendAsync(request.Email, emailSubject, emailBody);
            await _notificationService.SendNotificationAsync(user.Id, "A password recovery request has been initiated for your account.");

            return Result.Successful();
        }
    }
}
