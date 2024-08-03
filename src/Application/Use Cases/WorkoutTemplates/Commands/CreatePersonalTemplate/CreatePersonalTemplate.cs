using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.WorkoutTemplates.Commands.CreatePersonalTemplate;

public record CreatePersonalTemplateCommand : IRequest<Result>
{
    [JsonIgnore]
    public string UserId { get; set; } = "";//Temp user token
    public string? TemplateName { get; set; }
    public string? Duration { get; set; }
    public ICollection<PersonalTemplateExerciseDto> WorkoutTemplateExercises { get; init; } = new List<PersonalTemplateExerciseDto>();
}

public record PersonalTemplateExerciseDto
{
    public int? ExerciseId { get; set; }
    public int? OrderInSession { get; set; }
    public int? OrderInSuperset { get; set; }
    public string? Note { get; set; }
    public int? SetsRecommendation { get; set; }
    public int? IntensityPercentage { get; set; }
    public int? RpeRecommendation { get; set; }
    public string? WeightsUsed { get; set; }
    public string? NumbersOfReps { get; set; }
}

public class CreatePersonalTemplateCommandValidator : AbstractValidator<CreatePersonalTemplateCommand>
{
    public CreatePersonalTemplateCommandValidator()
    {
        RuleFor(v => v.TemplateName)
            .NotEmpty().WithMessage("Template name is required.")
            .MaximumLength(100).WithMessage("Template name must not exceed 100 characters.");

        RuleFor(v => v.Duration)
            .MaximumLength(50).WithMessage("Duration must not exceed 50 characters.");

        RuleForEach(v => v.WorkoutTemplateExercises).SetValidator(new PersonalTemplateExerciseValidator());
    }
}

public class PersonalTemplateExerciseValidator : AbstractValidator<PersonalTemplateExerciseDto>
{
    public PersonalTemplateExerciseValidator()
    {
        RuleFor(v => v.ExerciseId)
            .NotNull().WithMessage("Exercise ID is required.");
    }
}

public class CreatePersonalTemplateCommandHandler : IRequestHandler<CreatePersonalTemplateCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserTokenService _currentUserService;

    public CreatePersonalTemplateCommandHandler(IApplicationDbContext context, IUserTokenService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(CreatePersonalTemplateCommand request, CancellationToken cancellationToken)
    {
        //var userId = request.UserToken;

        var personalTemplate = new WorkoutTemplate
        {
            TemplateName = request.TemplateName,
            Duration = request.Duration,
            IsPublic = false, // Personal templates are not public
            CreatedBy = request.UserId,
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = request.UserId,
        };

        foreach (var exerciseDto in request.WorkoutTemplateExercises)
        {
            var workoutTemplateExercise = new WorkoutTemplateExercise
            {
                ExerciseId = exerciseDto.ExerciseId,
                OrderInSession = exerciseDto.OrderInSession,
                OrderInSuperset = exerciseDto.OrderInSuperset,
                Note = exerciseDto.Note,
                SetsRecommendation = exerciseDto.SetsRecommendation,
                IntensityPercentage = exerciseDto.IntensityPercentage,
                RpeRecommendation = exerciseDto.RpeRecommendation,
                WeightsUsed = exerciseDto.WeightsUsed,
                NumbersOfReps = exerciseDto.NumbersOfReps,
            };

            personalTemplate.WorkoutTemplateExercises.Add(workoutTemplateExercise);
        }

        _context.WorkoutTemplates.Add(personalTemplate);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }

}
