using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Constants;

namespace FitLog.Application.TrainingSurveys.Commands;
public record UpdateTrainingSurveyAnswersCommand : IRequest<Result>
{
    public int SurveyAnswerId { get; set; }
    public string? UserId { get; set; }
    public string? Goal { get; set; }
    public int? DaysPerWeek { get; set; }
    public string? ExperienceLevel { get; set; }
    public string? GymType { get; set; }
    public string? MusclesPriority { get; set; }
    public int? Age { get; set; }
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}


public class UpdaeteSurveyAnswerCommandValidator : AbstractValidator<UpdateTrainingSurveyAnswersCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdaeteSurveyAnswerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.Goal).NotEmpty().WithMessage("Goal is required.")
            .Must(BeAValidGoal).WithMessage("Invalid goal.");
        RuleFor(x => x.DaysPerWeek).NotEmpty().WithMessage("DaysPerWeek is required.");
        RuleFor(x => x.ExperienceLevel).NotEmpty().WithMessage("ExperienceLevel is required.")
            .Must(BeAValidExperienceLevel).WithMessage("Invalid experience level.");
        RuleFor(x => x.GymType).NotEmpty().WithMessage("GymType is required.")
            .Must(BeAValidGymType).WithMessage("Invalid gym type.");
        RuleFor(x => x.Age).NotEmpty().WithMessage("Age is required.");

        RuleFor(x => x.MusclesPriority).Custom((musclesPriority, context) =>
        {
            if (!MusclesPriorityIsValid(musclesPriority))
            {
                context.AddFailure("Invalid muscles priority.");
            }
        });
    }

    private bool BeAValidGoal(string? goal)
    {
        return ProgramAttributes.Goals.Contains(goal ?? "");
    }

    private bool BeAValidExperienceLevel(string? experienceLevel)
    {
        return ProgramAttributes.ExperienceLevels.Contains(experienceLevel ?? "");
    }

    private bool BeAValidGymType(string? gymType)
    {
        return ProgramAttributes.GymTypes.ContainsKey(gymType ?? "");
    }

    private bool MusclesPriorityIsValid(string? musclesPriority)
    {
        if (string.IsNullOrEmpty(musclesPriority)) return true; // Allow empty value
        var muscles = musclesPriority.Split(',');
        var validMuscles = _context.MuscleGroups.Select(x => x.MuscleGroupName).ToList();
        return muscles.All(x => validMuscles.Contains(x));
    }
}



public class UpdateTrainingSurveyAnswersCommandHandler : IRequestHandler<UpdateTrainingSurveyAnswersCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateTrainingSurveyAnswersCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateTrainingSurveyAnswersCommand command, CancellationToken cancellationToken)
    {
        var surveyAnswer = await _context.SurveyAnswers.FindAsync(command.SurveyAnswerId);

        if (surveyAnswer == null)
        {
            return Result.Failure(["Survey answer not found."]);
        }

        surveyAnswer.UserId = command.UserId;
        surveyAnswer.Goal = command.Goal;
        surveyAnswer.DaysPerWeek = command.DaysPerWeek;
        surveyAnswer.ExperienceLevel = command.ExperienceLevel;
        surveyAnswer.GymType = command.GymType;
        surveyAnswer.MusclesPriority = command.MusclesPriority;
        surveyAnswer.Age = command.Age;
        surveyAnswer.LastModified = command.LastModified;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
