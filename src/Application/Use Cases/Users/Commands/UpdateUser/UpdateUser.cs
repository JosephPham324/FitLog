using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitLog.Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Result>
    {
        public string? UserId { get; set; } = string.Empty;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? Gender { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public string? UserName { get; init; }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly UserManager<AspNetUser> _userManager;

        public UpdateUserCommandValidator(UserManager<AspNetUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.FirstName).MaximumLength(50).WithMessage("First name must be less than 50 characters.");
            RuleFor(x => x.LastName).MaximumLength(50).WithMessage("Last name must be less than 50 characters.");
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address.")
                .MustAsync(BeUniqueEmail).WithMessage("The specified email is already in use.");
            RuleFor(x => x.PhoneNumber)
                .Matches(@"(^\+?[1-9]\d{1,14}$)|(^(0|\+84)(3[2-9]|5[6|8|9]|7[0|6|7|8|9]|8[1-6|8-9]|9[0-9])[0-9]{7}$)") // International format
                .WithMessage("Invalid phone number format.");
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MustAsync(BeUniqueUsername).WithMessage("The specified username already exists.");
        }

        private async Task<bool> BeUniqueEmail(UpdateUserCommand command, string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null || user.Id == command.UserId;
        }

        private async Task<bool> BeUniqueUsername(UpdateUserCommand command, string? username, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(username??"");
            return user == null || user.Id == command.UserId;
        }

    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                return Result.Failure(["User not found."]);
            }

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                user.FirstName = request.FirstName;
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                user.LastName = request.LastName;
            }

            if (request.DateOfBirth.HasValue)
            {
                user.DateOfBirth = request.DateOfBirth.Value;
            }

            if (!string.IsNullOrEmpty(request.Gender))
            {
                user.Gender = request.Gender;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                user.PhoneNumber = request.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(request.UserName))
            {
                user.UserName = request.UserName;
            }

            _context.AspNetUsers.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Successful();
        }
    }
}
