using FitLog.Application.Common.Interfaces;
using FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;
using FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;

namespace FitLog.Application.TrainingRecommendation.Queries.GetProgramRecommendations;

public record GetProgramRecommendationsQuery : IRequest<object>
{
    public string UserId { get; init; } = string.Empty;
}

public class GetProgramRecommendationsQueryValidator : AbstractValidator<GetProgramRecommendationsQuery>
{
    public GetProgramRecommendationsQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty();
    }
}

public class GetProgramRecommendationsQueryHandler : IRequestHandler<GetProgramRecommendationsQuery, object>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetProgramRecommendationsQueryHandler(IApplicationDbContext context, IMediator mediator, IMapper mapper)
    {
        _context = context;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<object> Handle(GetProgramRecommendationsQuery request, CancellationToken cancellationToken)
    {
        // Get survey answers
        var surveyAnswerQuery = new GetUserTrainingSurveyQuery { UserId = request.UserId };
        var surveyAnswer = (SurveyAnswer)await _mediator.Send(surveyAnswerQuery, cancellationToken);

        // Retrieve all public programs
        var allPrograms = await _context.Programs
            .Include(p => p.User)
            .Where(p => p.PublicProgram == true)
            .ToListAsync();

        // Filter programs based on survey answers using helper methods
        var recommendations = allPrograms
            .Where(p => p.DaysPerWeek <= surveyAnswer.DaysPerWeek
                        && HasSimilarGoal(p.Goal??"", surveyAnswer.Goal??"")
                        && HasSimilarExperienceLevel(p.ExperienceLevel ?? "", surveyAnswer.ExperienceLevel ?? "")
                        && HasSuitableGymType(p.GymType ?? "", surveyAnswer.GymType ?? ""))
            .ToList();

        return _mapper.Map<List<ProgramOverviewDto>>(recommendations);
    }

    private bool HasSimilarGoal(string ProgramGoal, string UserGoal)
    {
        List<string> programGoals = ProgramGoal.Split(',').ToList();
        List<string> userGoals = UserGoal.Split(',').ToList();

        bool hasSimilarGoal = programGoals.Intersect(userGoals).Any(goal => ProgramAttributes.Goals.Contains(goal));


        return hasSimilarGoal;
    }

    private bool HasSimilarExperienceLevel(string ProgramExperienceLevel, string UserExperienceLevel)
    {
        List<string> ProgramSuitability = ProgramExperienceLevel.Split(',')
        .Select(item => item.Trim().ToUpper())
        .ToList();


        return ProgramSuitability.Contains(UserExperienceLevel.ToUpper());
    }
    private bool HasSuitableGymType(string ProgramGymType, string UserGymType)
    {
        List<string> ProgramGymTypes = ProgramGymType.Split(',')
            .Select(item => item.Trim().ToUpper())
            .ToList();

        return ProgramGymTypes.Contains(UserGymType.ToUpper()) ;
    }
}
