using FitLog.Application.Common.Interfaces;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.TrainingRecommendation.Queries.GetProgramRecommendations;
using FitLog.Application.TrainingRecommendation.Queries.GetWorkoutRecommendation;
using FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;
using FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Domain.Entities;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class TrainingRecommendation : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public TrainingRecommendation()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }


    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
           .RequireAuthorization()
           .MapGet(GetProgramRecommendations, "/programs-recommendation/user")
           .MapPost(GetWorkoutRecommendation, "/workout-recommendation/user");
    }

    public Task<List<ProgramOverviewDto>> GetProgramRecommendations(ISender sender, [AsParameters] GetProgramRecommendationsQuery query)
    {

        return sender.Send(query);
    }

    public Task<List<ExerciseDTO>> GetWorkoutRecommendation(ISender sender, [FromBody] GetWorkoutRecommendationQuery query)
    {
        
        return sender.Send(query);
    }
}
