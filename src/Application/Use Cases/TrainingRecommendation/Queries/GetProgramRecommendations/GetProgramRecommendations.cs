using FitLog.Application.Common.Interfaces;
using FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;
using FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;
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
        //Get survey answers
        var surveyAnswerQuery = new GetUserTrainingSurveyQuery { UserId = request.UserId };
        var surveyAnswer = (SurveyAnswer)await _mediator.Send(surveyAnswerQuery, cancellationToken);

        var recommendations = await
            _context.Programs
                .Include(p => p.User)
            .Where(p =>
            p.DaysPerWeek == surveyAnswer.DaysPerWeek
            && p.Goal == surveyAnswer.Goal
            && p.ExperienceLevel == surveyAnswer.ExperienceLevel
            && p.GymType == surveyAnswer.GymType
            && p.PublicProgram == true)
            .ToListAsync();

        return _mapper.Map<List<ProgramOverviewDto>>(recommendations);
    }
}
