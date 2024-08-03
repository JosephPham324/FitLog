using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Commands.DeleteWorkoutTemplate;

public record DeleteWorkoutTemplateCommand (int Id) : IRequest<Result>;

public class DeleteWorkoutTemplateCommandValidator : AbstractValidator<DeleteWorkoutTemplateCommand>
{
    public DeleteWorkoutTemplateCommandValidator()
    {
        RuleFor(v => v.Id)
           .GreaterThan(0).WithMessage("Invalid workout template ID.");
    }
}

public class DeleteWorkoutTemplateCommandHandler : IRequestHandler<DeleteWorkoutTemplateCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkoutTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteWorkoutTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WorkoutTemplates.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            return Result.Failure(["Workout Template not found"]);
        }

        _context.WorkoutTemplates.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
