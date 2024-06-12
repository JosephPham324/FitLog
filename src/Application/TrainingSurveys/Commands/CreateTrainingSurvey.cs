using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.TrainingSurveys.Commands;
public record CreateSurveyAnswerCommand : IRequest<TrainingSurveyDTO>
{
    public string? UserId { get; set; }
    public string? Goal { get; set; }
    public int? DaysPerWeek { get; set; }
    public string? ExperienceLevel { get; set; }
    public string? GymType { get; set; }
    public string? MusclesPriority { get; set; }
    public int? Age { get; set; }
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}
    public class CreateSurveyAnswerCommandHandler :  IRequestHandler<CreateSurveyAnswerCommand, TrainingSurveyDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateSurveyAnswerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TrainingSurveyDTO> Handle(CreateSurveyAnswerCommand command, CancellationToken cancellationToken)
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
            LastModified = command.LastModified
        };

        _context.SurveyAnswers.Add(surveyAnswer);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TrainingSurveyDTO>(surveyAnswer);
    }
}
