using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.CoachingServices.Commands.DeleteCoachingService;

public record DeleteCoachingServiceCommand : IRequest<Result>
{
    public int Id { get; init; }
}

public class DeleteCoachingServiceCommandValidator : AbstractValidator<DeleteCoachingServiceCommand>
{
    public DeleteCoachingServiceCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}

public class DeleteCoachingServiceCommandHandler : IRequestHandler<DeleteCoachingServiceCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteCoachingServiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteCoachingServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CoachingServices
            .FirstOrDefaultAsync(cs => cs.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CoachingService), request.Id + "");
        }

        _context.CoachingServices.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
