using FitLog.Application.TrainingRecommendation.Queries.GetProgramRecommendations;
using FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class TrainingRecommendation : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
           .MapGet(GetProgramRecommendations, "user");
    }

    public Task<object> GetProgramRecommendations(ISender sender, [AsParameters] GetProgramRecommendationsQuery query)
    {
        return sender.Send(query);
    }

    
}
