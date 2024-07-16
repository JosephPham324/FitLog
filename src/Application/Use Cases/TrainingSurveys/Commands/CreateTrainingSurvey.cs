using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;

namespace FitLog.Application.TrainingSurveys.Commands;
public record CreateSurveyAnswerCommand : IRequest<Result>
{
    public string? UserId { get; set; }
    public string? Goal { get; set; }

    [Range(1,7)]
    public int? DaysPerWeek { get; set; }
    
    public string? ExperienceLevel { get; set; }
    public string? GymType { get; set; }
    public string? MusclesPriority { get; set; }

    [Range(13,120)]
    public int? Age { get; set; }

    //public DateTime LastModified { get; set; } = DateTime.UtcNow;
}

public class CreateSurveyAnswerCommandValidator : AbstractValidator<CreateSurveyAnswerCommand>
{
        private readonly IApplicationDbContext _context;

        public CreateSurveyAnswerCommandValidator(IApplicationDbContext context)
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
            return ProgramAttributes.Goals.Contains(goal??"");
        }

        private bool BeAValidExperienceLevel(string? experienceLevel)
        {
            return ProgramAttributes.ExperienceLevels.Contains(experienceLevel??"");
        }

        private bool BeAValidGymType(string? gymType)
        {
            return ProgramAttributes.GymTypes.ContainsKey(gymType??"");
        }

        private bool MusclesPriorityIsValid(string? musclesPriority)
        {
            if (string.IsNullOrEmpty(musclesPriority)) return true; // Allow empty value
            var muscles = musclesPriority.Split(',');
            var validMuscles = _context.MuscleGroups.Select(x => x.MuscleGroupName).ToList();
            return muscles.All(x => validMuscles.Contains(x));
        }
}


public class CreateSurveyAnswerCommandHandler :  IRequestHandler<CreateSurveyAnswerCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateSurveyAnswerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Handle(CreateSurveyAnswerCommand command, CancellationToken cancellationToken)
    {
        var surveyAnswer = new SurveyAnswer
        {
            UserId = command.UserId,
            Goal = command.Goal,
            DaysPerWeek = command.DaysPerWeek,
            ExperienceLevel = command.ExperienceLevel,
            GymType = command.GymType,
            MusclesPriority = command.MusclesPriority,
            Age = command.Age,
            LastModified = DateTime.Now
        };

        _context.SurveyAnswers.Add(surveyAnswer);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
