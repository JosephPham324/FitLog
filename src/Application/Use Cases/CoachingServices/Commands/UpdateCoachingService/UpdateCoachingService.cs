using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachingServices.Commands.UpdateCoachingService;

public record UpdateCoachingServiceCommand : IRequest<bool>
{
    public int Id { get; init; }
    public string ServiceName { get; init; } = null!;
    public string? Description { get; init; }
    public int? Duration { get; init; }
    public decimal? Price { get; init; }
    public bool? ServiceAvailability { get; init; }
    public string? AvailabilityAnnouncement { get; init; }
}

public class UpdateCoachingServiceCommandValidator : AbstractValidator<UpdateCoachingServiceCommand>
{
    public UpdateCoachingServiceCommandValidator()
    {

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(v => v.ServiceName)
            .NotEmpty().WithMessage("Service Name is required.")
            .MaximumLength(200).WithMessage("Service Name must not exceed 200 characters.");
    }
}

public class UpdateCoachingServiceCommandHandler : IRequestHandler<UpdateCoachingServiceCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateCoachingServiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCoachingServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CoachingServices
            .FirstOrDefaultAsync(cs => cs.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CoachingService), request.Id + "");
        }

        entity.ServiceName = request.ServiceName;
        entity.Description = request.Description;
        entity.Duration = request.Duration;
        entity.Price = request.Price;
        entity.ServiceAvailability = request.ServiceAvailability;
        entity.AvailabilityAnnouncement = request.AvailabilityAnnouncement;
        entity.LastModified = DateTimeOffset.Now;
        entity.LastModifiedBy = "system"; // Replace with actual user identifier

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
