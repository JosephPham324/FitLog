using System.ComponentModel.DataAnnotations;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachingServices.Commands.CreateCoachingService;

public record CreateCoachingServiceCommand : IRequest<Result>
{
    public string ServiceName { get; init; } = null!;
    public string? Description { get; init; }

    [Range(1, 60*24)]
    public int? Duration { get; init; }

    [Range(1,int.MaxValue)]
    public decimal? Price { get; init; }
    public bool? ServiceAvailability { get; init; }
    public string? AvailabilityAnnouncement { get; init; }
}

public class CreateCoachingServiceCommandValidator : AbstractValidator<CreateCoachingServiceCommand>
{
    public CreateCoachingServiceCommandValidator()
    {
        RuleFor(v => v.ServiceName)
            .NotEmpty().WithMessage("Service Name is required.")
            .MaximumLength(200).WithMessage("Service Name must not exceed 200 characters.");

        // Add additional validation rules as needed
    }
}

public class CreateCoachingServiceCommandHandler : IRequestHandler<CreateCoachingServiceCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateCoachingServiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateCoachingServiceCommand request, CancellationToken cancellationToken)
    {

        var defaultUser = await _context.AspNetUsers.FirstOrDefaultAsync(cancellationToken);


        var entity = new CoachingService
        {
            ServiceName = request.ServiceName,
            Description = request.Description,
            Duration = request.Duration,
            Price = request.Price,
            ServiceAvailability = request.ServiceAvailability,
            AvailabilityAnnouncement = request.AvailabilityAnnouncement,
            Created = DateTimeOffset.Now,
            CreatedBy = defaultUser?.Id, // Replace with actual user identifier
            LastModified = DateTimeOffset.Now,
            LastModifiedBy = defaultUser?.Id // Replace with actual user identifier
        };

        _context.CoachingServices.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
