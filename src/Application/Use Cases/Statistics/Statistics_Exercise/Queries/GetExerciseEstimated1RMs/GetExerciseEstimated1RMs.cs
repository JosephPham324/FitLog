using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Statistics_Exercise.Queries.GetExerciseLogHistory;
using FitLog.Domain.Entities;

namespace FitLog.Application.Statistics_Exercise.Queries.GetExerciseEstimated1RMs;
public record OneRepMaxRecord(
        double Epley,
        double Brzycki,
        double Lander,
        double Lombardi,
        double Mayhew,
        double OConner,
        double Wathan
);

public record GetExerciseEstimated1RMsQuery : IRequest<Dictionary<DateTime, OneRepMaxRecord>>
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public int ExerciseId { get; init; }
}

public class GetExerciseEstimated1RMsQueryValidator : AbstractValidator<GetExerciseEstimated1RMsQuery>
{
    public GetExerciseEstimated1RMsQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty();
        RuleFor(v => v.ExerciseId)
                .NotEmpty()
                .WithMessage("ExerciseId is required.")
                .GreaterThan(0)
                .WithMessage("ExerciseId must be more than 0");
    }
}

public class GetExerciseEstimated1RMsQueryHandler : IRequestHandler<GetExerciseEstimated1RMsQuery, Dictionary<DateTime, OneRepMaxRecord>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;


    public GetExerciseEstimated1RMsQueryHandler(IApplicationDbContext context, IMediator mediator )
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Dictionary<DateTime, OneRepMaxRecord>> Handle(GetExerciseEstimated1RMsQuery request, CancellationToken cancellationToken)
    {
        //get data
        var logsHistoryQuery = new GetExerciseLogHistoryQuery
        {
            UserId = request.UserId,
            ExerciseId = request.ExerciseId
        };
        var logsHistory = await _mediator.Send(logsHistoryQuery, cancellationToken);
        //Check if there is data
        if (logsHistory == null || logsHistory.Count() == 0)
            throw new NotFoundException(nameof(ExerciseLog), "No record found for this exercise");


        //process data

        Dictionary<DateTime, OneRepMaxRecord> estimated1RMByLog = new Dictionary<DateTime, OneRepMaxRecord>();

        foreach (var log in logsHistory)
        {
            var weights = log.GetWeightsUsed();
            var reps = log.GetNumberOfReps();

            var max1RM = GetMax1RM(weights?? new List<double>(), reps ?? new List<int>());
            
            if (estimated1RMByLog.ContainsKey(log.DateCreated.Date))
            {//If there's existing record at the same date, get the higher 1RM
                var existing1RM = estimated1RMByLog[log.DateCreated.Date];
                var higher1RM = GetHigher1RM(existing1RM, max1RM);
                estimated1RMByLog[log.DateCreated.Date] = higher1RM;
            }
            else //Else add new record
                estimated1RMByLog.Add(log.DateCreated.Date, max1RM);
        }
        return estimated1RMByLog;
    }

    
    private OneRepMaxRecord GetHigher1RM(OneRepMaxRecord record1, OneRepMaxRecord record2)
    {
        return new OneRepMaxRecord(
            Epley: Math.Max(record1.Epley, record2.Epley),
            Brzycki: Math.Max(record1.Brzycki, record2.Brzycki),
            Lander: Math.Max(record1.Lander, record2.Lander),
            Lombardi: Math.Max(record1.Lombardi, record2.Lombardi),
            Mayhew: Math.Max(record1.Mayhew, record2.Mayhew),
            OConner: Math.Max(record1.OConner, record2.OConner),
            Wathan: Math.Max(record1.Wathan, record2.Wathan)
        );
    }

    private OneRepMaxRecord GetMax1RM(List<double> weights, List<int> reps)
    {
        double maxEpley = double.MinValue;
        double maxBrzycki = double.MinValue;
        double maxLander = double.MinValue;
        double maxLombardi = double.MinValue;
        double maxMayhew = double.MinValue;
        double maxOConner = double.MinValue;
        double maxWathan = double.MinValue;

        for (int i = 0; i < weights.Count; i++)
        {
            double weight = weights[i];
            int rep = reps[i];

            double epley = Epley(weight, rep);
            double brzycki = Brzycki(weight, rep);
            double lander = Lander(weight, rep);
            double lombardi = Lombardi(weight, rep);
            double mayhew = Mayhew(weight, rep);
            double oConner = OConner(weight, rep);
            double wathan = Wathan(weight, rep);

            if (epley > maxEpley) maxEpley = epley;
            if (brzycki > maxBrzycki) maxBrzycki = brzycki;
            if (lander > maxLander) maxLander = lander;
            if (lombardi > maxLombardi) maxLombardi = lombardi;
            if (mayhew > maxMayhew) maxMayhew = mayhew;
            if (oConner > maxOConner) maxOConner = oConner;
            if (wathan > maxWathan) maxWathan = wathan;
        }

        return new OneRepMaxRecord(maxEpley, maxBrzycki, maxLander, maxLombardi, maxMayhew, maxOConner, maxWathan);
    }


    private static double Epley(double weight, int reps)
    {
        return weight * (1 + reps / 30.0);
    }

    private static double Brzycki(double weight, int reps)
    {
        return weight / (1.0278 - 0.0278 * reps);
    }

    private static double Lander(double weight, int reps)
    {
        return (100 * weight) / (101.3 - 2.67123 * reps);
    }

    private static double Lombardi(double weight, int reps)
    {
        return weight * Math.Pow(reps, 0.10);
    }

    private static double Mayhew(double weight, int reps)
    {
        return (100 * weight) / (52.2 + 41.9 * Math.Exp(-0.055 * reps));
    }

    private static double OConner(double weight, int reps)
    {
        return weight * (1 + 0.025 * reps);
    }

    private static double Wathan(double weight, int reps)
    {
        return (100 * weight) / (48.8 + 53.8 * Math.Exp(-0.075 * reps));
    }
}
