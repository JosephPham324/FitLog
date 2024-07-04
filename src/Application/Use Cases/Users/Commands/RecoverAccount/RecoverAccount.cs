using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Application.Users.Commands.RecoverAccount;

public record RecoverAccountCommand(string Email) : IRequest<RecoveryResultDTO>;


public class RecoverAccountCommandValidator : AbstractValidator<RecoverAccountCommand>
{
    public RecoverAccountCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required for account recovery.");
    }
}


public class RecoverAccountCommandHandler : IRequestHandler<RecoverAccountCommand, RecoveryResultDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService; // Assuming an email service is available
    private readonly UserManager<AspNetUser> _userManager;

    public RecoverAccountCommandHandler(UserManager<AspNetUser> userManager, IEmailService emailService, IApplicationDbContext context)
    {
        _userManager = userManager;
        _emailService = emailService;
        _context = context;

    }

    public async Task<RecoveryResultDTO> Handle(RecoverAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user == null)
        {
            return new RecoveryResultDTO { Success = false, Message = "No account associated with this email." };
        }

        // Assuming GeneratePasswordResetTokenAsync is a method to generate a reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Send recovery email
        var recoveryLink = $"https://localhost:44777/recover?token={token}&email={user.Email}";
        await _emailService.SendAsync(request.Email, "Recover Your Account", $"Please click on the link to recover your account: {recoveryLink}");

        return new RecoveryResultDTO { Success = true, Message = "Recovery email sent successfully." };
    }
}

