using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;

public record GetUserTrainingSurveyQuery : IRequest<SurveyAnswer>
{
    public string UserId { get; init; } = string.Empty;
}

public class GetUserTrainingSurveyQueryValidator : AbstractValidator<GetUserTrainingSurveyQuery>
{
    public GetUserTrainingSurveyQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty();
    }
}

public class GetUserTrainingSurveyQueryHandler : IRequestHandler<GetUserTrainingSurveyQuery, SurveyAnswer>
{
    private readonly IApplicationDbContext _context;

    public GetUserTrainingSurveyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SurveyAnswer> Handle(GetUserTrainingSurveyQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.SurveyAnswers
            .Where(answer => answer.UserId != null? answer.UserId.Equals(request.UserId) : false)
            .FirstOrDefaultAsync(cancellationToken);
        if (res == null)
            throw new NotFoundException(nameof(SurveyAnswer), request.UserId);

        return res;
    }
}
