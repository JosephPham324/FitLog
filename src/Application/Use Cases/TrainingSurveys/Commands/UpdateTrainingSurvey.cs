using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;

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
