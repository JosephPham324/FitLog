using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Commands.DeleteWorkoutTemplate;

public record DeleteWorkoutTemplateCommand (int Id) : IRequest<bool>;

public class DeleteWorkoutTemplateCommandValidator : AbstractValidator<DeleteWorkoutTemplateCommand>
{
    public DeleteWorkoutTemplateCommandValidator()
    {
        RuleFor(v => v.Id)
           .GreaterThan(0).WithMessage("Invalid workout template ID.");
    }
}

public class DeleteWorkoutTemplateCommandHandler : IRequestHandler<DeleteWorkoutTemplateCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkoutTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteWorkoutTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WorkoutTemplates.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(WorkoutTemplate), request.Id + "");
        }

        _context.WorkoutTemplates.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
