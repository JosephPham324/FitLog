using FitLog.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FitLog.Application.CoachingServices.Commands.BookService
{
    public record BookServiceCommand : IRequest<Result>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;
        public int CoachingServiceId { get; init; } 
    }

    public class BookServiceCommandValidator : AbstractValidator<BookServiceCommand>
    {
        public BookServiceCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.CoachingServiceId).NotEmpty().WithMessage("Coaching Service ID is required.");
        }
    }

    public class BookServiceCommandHandler : IRequestHandler<BookServiceCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public BookServiceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(BookServiceCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AspNetUsers.FindAsync(new object[] { request.UserId }, cancellationToken);
            if (user == null)
            {
                return Result.Failure(["User not found."]);
            }

            var service = await _context.CoachingServices.FindAsync(new object[] { request.CoachingServiceId }, cancellationToken);
            if (service == null || !service.ServiceAvailability.GetValueOrDefault())
            {
                return Result.Failure(["Coaching service not available."]);
            }

            var booking = new CoachingBooking
            {
                UserId = request.UserId,
                CoachingServiceId = request.CoachingServiceId,
                Status = "Pending",
                CreatedDate = DateTime.UtcNow
            };

            _context.CoachingBookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Successful();
        }
    }
}
