using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;
using FitLog.Application.WorkoutLogs.Commands.CreateWorkoutLog;
using FitLog.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FitLog.Application.TrainingRecommendation.Queries.GetWorkoutRecommendation;

public record GetWorkoutRecommendationQuery : IRequest<List<ExerciseDTO>>
{
    [JsonIgnore]
    public string UserId { get; init; } = string.Empty;
    public List<int> ExerciseIds { get; init; } = new List<int>();
}

public class GetWorkoutRecommendationQueryValidator : AbstractValidator<GetWorkoutRecommendationQuery>
{
    public GetWorkoutRecommendationQueryValidator()
    {
    }
}

public class GetWorkoutRecommendationQueryHandler : IRequestHandler<GetWorkoutRecommendationQuery, List<ExerciseDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public GetWorkoutRecommendationQueryHandler(IApplicationDbContext context, IMediator mediator, IMapper mapper)
    {
        _context = context;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<List<ExerciseDTO>> Handle(GetWorkoutRecommendationQuery request, CancellationToken cancellationToken)
    {
        // Get survey answers
        var surveyAnswerQuery = new GetUserTrainingSurveyQuery { UserId = request.UserId };
        var surveyAnswer = (SurveyAnswer)await _mediator.Send(surveyAnswerQuery, cancellationToken);

        //get priority from survey
        var priorities = surveyAnswer?.MusclesPriority?.Split(',').ToList();

        //get exercises from ids
        var exercises = await GetExercisesByIdsAsync(request.ExerciseIds.IsNullOrEmpty() ? new List<int>() : request.ExerciseIds);

        //get exercises with prioritized muscles group
        var exercisesListOrderedByPriority = GetExercisesWithPriority(exercises, priorities ?? []);

        return _mapper.Map<List<ExerciseDTO>>(exercisesListOrderedByPriority);
    }
    public async Task<List<Exercise>> GetExercisesByIdsAsync(List<int> exerciseIds)
    {
        // Fetch programs with IDs in the provided list
        var exercises = await _context.Exercises
                .Include(e=>e.ExerciseMuscleGroups)
                    .ThenInclude(em => em.MuscleGroup)
            .Where(exercise => exerciseIds.Contains(exercise.ExerciseId))
            
            .ToListAsync();

        return exercises;
    }

    List<Exercise> GetExercisesWithPriority(List<Exercise> exercises, List<string> musclesPriorities)
    {
        var res = new List<Exercise>();

        // Filter exercises that have at least one prioritized muscle group
        var prioritizedExercises = exercises
           .Where(exercise =>
               exercise.ExerciseMuscleGroups
                   .Select(em => em.MuscleGroup.MuscleGroupName)
                   .Intersect(musclesPriorities)
                   .Any())
           .ToList();

        // Order exercises by the number of muscle groups they target (compounds first)
        var orderedPrioritizedExercises = prioritizedExercises
            .OrderByDescending(exercise => exercise.ExerciseMuscleGroups.Count)
            .ToList();

        // Remove the prioritized exercises from the original list
        var unprioritizedExercises = exercises.Except(prioritizedExercises).ToList();

        // Order unprioritized exercises by the number of muscle groups they target (compounds first)
        var orderedUnprioritizedExercises = unprioritizedExercises
            .OrderByDescending(exercise => exercise.ExerciseMuscleGroups.Count)
            .ToList();

        // Combine the results
        res.AddRange(orderedPrioritizedExercises);
        res.AddRange(orderedUnprioritizedExercises);

        return res;
    }
}
